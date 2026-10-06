"""2.1 araçları: Araçlar menüsündeki tüm komutlar.

KalemWindow bu sınıftan türer; burada seçme, metin, lazer, kaybolan
mürekkep, dolgu, ölçme araçları, spot ışığı, büyüteç, sayaç, kura, ekran
görüntüsü, sayfalar, kaydetme / açma / dışa aktarma, paylaşım ve renk
paleti bulunur.
"""

import logging
import math
import os
import random
import time

import cairo
import gi

gi.require_version("Gdk", "3.0")
gi.require_version("Gtk", "3.0")
from gi.repository import Gdk, GLib, Gtk  # noqa: E402

from . import lesson, qr, render, share  # noqa: E402
from . import recognize as rec  # noqa: E402
from . import styles as st  # noqa: E402
from . import tools as tl  # noqa: E402
from .config import data_home, hex_to_rgb, rgb_to_hex  # noqa: E402
from .i18n import _  # noqa: E402
from .ink import Stroke, densify, render_stroke  # noqa: E402
from .scene import FillItem, Scene, TextItem, point_in_polygon, polygon_area  # noqa: E402

VANISH_HOLD = 2.0     # kaybolan mürekkep bu süre tam görünür
VANISH_FADE = 1.5     # sonra bu sürede söner


def _union(a, b):
    if a is None:
        return b
    if b is None:
        return a
    return (min(a[0], b[0]), min(a[1], b[1]), max(a[2], b[2]), max(a[3], b[3]))


class FeaturesMixin:
    # ================================================================ durum
    def _init_features(self):
        self.stroke_mode = "normal"        # normal | dash | dot | arrow
        self.overlays = []
        self.active_overlay = None
        self.snap_edge = None
        self.laser = tl.LaserTrail()
        self.vanishing = []                # [(Stroke, bitiş_zamanı)]
        self._anim_source = 0
        self.selection = []                # seçili nesneler (strokes/texts/fills)
        self.sel_mode = None               # lasso | move | scale
        self.lasso = None
        self.sel_grab = None
        self.sel_before = None
        self.share_server = None
        self.screen_snapshot = None
        self._picked_lots = set()
        self._dirty = False
        self._autosave_source = GLib.timeout_add_seconds(60, self._autosave_tick)

    # ------------------------------------------------------- etkin araçlar
    def active_tools(self):
        out = {"stroke:" + self.stroke_mode}
        if self.settings["ShapeRecognition"]:
            out.add("recognize")
        names = {st.SELECT: "select", st.TEXT: "text", st.LASER: "laser",
                 st.VANISH: "vanish", st.FILL: "fill"}
        if self.draw_state in names:
            out.add(names[self.draw_state])
        for ov in self.overlays:
            out.add(ov.kind)
        if self.share_server is not None:
            out.add("share")
        return out

    # ===================================================== Araçlar komutları
    def perform_tool(self, name):
        if name.startswith("stroke:"):
            self.stroke_mode = name.split(":")[1]
            self.collapse_submenus()
            if self.draw_state not in (st.PEN, st.VANISH):
                self.activate_pen()
            self.queue_draw()
            return
        if name.startswith("recent:") or name.startswith("palette:"):
            self._swatch_clicked(name)
            return
        handlers = {
            "recognize": self._toggle_recognition,
            "select": lambda: self._set_tool_state(st.SELECT),
            "text": lambda: self._set_tool_state(st.TEXT),
            "laser": lambda: self._set_tool_state(st.LASER),
            "vanish": lambda: self._set_tool_state(st.VANISH),
            "fill": lambda: self._set_tool_state(st.FILL),
            "ruler": lambda: self._toggle_overlay("ruler"),
            "setsquare": lambda: self._toggle_overlay("setsquare"),
            "protractor": lambda: self._toggle_overlay("protractor"),
            "compass": lambda: self._toggle_overlay("compass"),
            "spotlight": lambda: self._toggle_overlay("spotlight"),
            "magnifier": lambda: self._toggle_overlay("magnifier"),
            "timer": lambda: self._toggle_overlay("timer"),
            "lots": self.show_lots_dialog,
            "screenshot": self.screenshot_to_page,
            "clear": self.clear_page_confirm,
            "png": self.export_png_dialog,
            "pdf": self.export_pdf_dialog,
            "save": self.save_lesson_dialog,
            "open": self.open_lesson_dialog,
            "share": self.toggle_share,
            "page:prev": lambda: self.go_page(self.book.index - 1),
            "page:next": lambda: self.go_page(self.book.index + 1),
            "page:new": self.new_page,
            "page:delete": self.delete_page_confirm,
        }
        fn = handlers.get(name)
        if fn is None:
            return
        keep_open = name.startswith("page:")
        if not keep_open:
            self.collapse_submenus()
        fn()
        self.queue_draw()

    def _set_tool_state(self, state):
        if self.draw_state == state:
            self.activate_pen()
            return
        self.cancel_triangle()
        self.place_library_item()
        self.clear_selection()
        self.draw_state = state
        self._update_input_shape()
        self.queue_draw()

    def _toggle_recognition(self):
        self.settings["ShapeRecognition"] = not self.settings["ShapeRecognition"]
        self.settings.save()

    # =========================================================== animasyon
    def _ensure_anim(self):
        if self._anim_source:
            return

        def tick():
            busy = False
            if self.laser.points:
                bb = self.laser.bbox()
                busy = self.laser.prune() or busy
                self._queue_bbox(bb)
            if self.vanishing:
                now = time.monotonic()
                dirty = None
                for s, end in self.vanishing:
                    dirty = _union(dirty, s.bbox())
                self.vanishing = [(s, end) for s, end in self.vanishing if now < end]
                self._queue_bbox(dirty)
                busy = busy or bool(self.vanishing)
            for ov in self.overlays:
                if isinstance(ov, tl.TimerWidget):
                    if ov.tick():
                        self._timer_finished()
                    if ov.running() or ov.finished:
                        self._queue_bbox(ov.bbox())
                        busy = True
            if not busy:
                self._anim_source = 0
                return False
            return True
        self._anim_source = GLib.timeout_add(40, tick)

    def _timer_finished(self):
        display = Gdk.Display.get_default()
        for i in range(3):
            GLib.timeout_add(i * 400, lambda: (display.beep(), False)[1])

    # ======================================================= çizim kalıbı
    def _snap_start(self, x, y):
        self.snap_edge = tl.snap_point(x, y, self.overlays) if self.overlays else None
        if self.snap_edge is not None:
            return tl.project_on(self.snap_edge, x, y)
        return x, y

    def _snapped(self, x, y):
        if self.snap_edge is not None:
            return tl.project_on(self.snap_edge, x, y)
        return x, y

    def _finish_stroke(self, s):
        """Bitmiş kalem çizgisine ayarları uygular; eklenecek çizgileri döndürür."""
        if self.snap_edge is None:
            if self.settings["ShapeRecognition"]:
                shaped = rec.recognized_strokes(s)
                if shaped:
                    s = shaped[0]
            elif self.settings["SmoothInk"] and not s.highlighter:
                s = s.copy_with_points(rec.smooth(s.points))
        self.snap_edge = None
        if self.stroke_mode == "dash":
            out = rec.dashed(s)
        elif self.stroke_mode == "dot":
            out = rec.dotted(s)
        elif self.stroke_mode == "arrow":
            out = rec.with_arrow(s)
        else:
            out = [s]
        result = []
        for part in out:
            result.extend(rec.chunk(part))
        return result

    def mark_dirty(self):
        self._dirty = True

    # ======================================================== kaybolan / lazer
    def add_vanishing(self, stroke):
        self.vanishing.append((stroke, time.monotonic() + VANISH_HOLD + VANISH_FADE))
        self._ensure_anim()

    def draw_vanishing(self, cr):
        now = time.monotonic()
        for s, end in self.vanishing:
            left = end - now
            alpha = 1.0 if left > VANISH_FADE else max(0.0, left / VANISH_FADE)
            cr.push_group()
            render_stroke(cr, s)
            cr.pop_group_to_source()
            cr.paint_with_alpha(alpha)

    def laser_point(self, x, y):
        self.laser.add(x, y)
        self._queue_bbox(self.laser.bbox())
        self._ensure_anim()

    # ================================================================ seçim
    def clear_selection(self):
        if self.selection or self.lasso:
            self.selection = []
            self.lasso = None
            self.sel_mode = None
            self.queue_draw()

    def _item_bbox(self, item):
        if isinstance(item, TextItem):
            return render.text_bbox(item)
        return item.bbox()

    def selection_bbox(self):
        bb = None
        for it in self.selection:
            bb = _union(bb, self._item_bbox(it))
        return bb

    def _sel_handles(self):
        bb = self.selection_bbox()
        if bb is None:
            return None
        x0, y0, x1, y1 = bb
        pad = 10
        return {"box": (x0 - pad, y0 - pad, x1 + pad, y1 + pad),
                "scale": (x1 + pad, y1 + pad),
                "delete": (x1 + pad, y0 - pad),
                "copy": (x0 - pad, y0 - pad)}

    def select_press(self, x, y):
        h = self._sel_handles()
        if h is not None:
            dx, dy = h["delete"]
            if math.hypot(x - dx, y - dy) <= tl.HANDLE:
                self._delete_selection()
                return
            cx, cy = h["copy"]
            if math.hypot(x - cx, y - cy) <= tl.HANDLE:
                self._duplicate_selection()
                return
            sx, sy = h["scale"]
            bx0, by0, bx1, by1 = h["box"]
            if math.hypot(x - sx, y - sy) <= tl.HANDLE + 4:
                self.sel_mode = "scale"
                self.sel_grab = (bx0 + 10, by0 + 10, max(1.0, math.hypot(sx - bx0, sy - by0)), x, y)
                self.sel_before = self.history.scene
                self._sel_orig = list(self.selection)
                return
            if bx0 <= x <= bx1 and by0 <= y <= by1:
                self.sel_mode = "move"
                self.sel_grab = (x, y)
                self.sel_before = self.history.scene
                self._sel_orig = list(self.selection)
                return
        self.selection = []
        self.sel_mode = "lasso"
        self.lasso = [(x, y)]
        self.queue_draw()

    def select_move(self, x, y):
        if self.sel_mode == "lasso":
            self.lasso.append((x, y))
            xs = [p[0] for p in self.lasso]
            ys = [p[1] for p in self.lasso]
            self._queue_bbox((min(xs) - 4, min(ys) - 4, max(xs) + 4, max(ys) + 4))
        elif self.sel_mode in ("move", "scale"):
            if self.sel_mode == "move":
                gx, gy = self.sel_grab
                dx, dy, scale, origin = x - gx, y - gy, 1.0, None
            else:
                ox, oy, d0, _, _ = self.sel_grab
                d = max(10.0, math.hypot(x - ox, y - oy))
                dx, dy, scale, origin = 0.0, 0.0, d / d0, (ox, oy)
            self._apply_transform(dx, dy, scale, origin)

    def _transform_item(self, it, dx, dy, scale, origin):
        if isinstance(it, Stroke):
            if origin is None:
                pts = [(px + dx, py + dy, p) for px, py, p in it.points]
            else:
                ox, oy = origin
                pts = [(ox + (px - ox) * scale, oy + (py - oy) * scale, p) for px, py, p in it.points]
            return it.copy_with_points(densify(pts) if scale > 1.05 else pts)
        return it.moved(dx, dy, scale, origin)

    def _apply_transform(self, dx, dy, scale, origin):
        before = self.sel_before
        old_bb = self.selection_bbox()
        mapping = {id(it): self._transform_item(it, dx, dy, scale, origin) for it in self._sel_orig}

        def repl(seq):
            return tuple(mapping.get(id(i), i) for i in seq)
        new = before.replace(strokes=repl(before.strokes), texts=repl(before.texts),
                             fills=repl(before.fills))
        self.history.set_without_history(new)
        self.selection = list(mapping.values())
        dirty = _union(old_bb, self.selection_bbox())
        if dirty:
            pad = 40
            dirty = (dirty[0] - pad, dirty[1] - pad, dirty[2] + pad, dirty[3] + pad)
        self._rebuild_base(dirty)
        self._queue_bbox(dirty)

    def select_release(self, x, y):
        if self.sel_mode == "lasso":
            poly = self.lasso or []
            self.lasso = None
            self.sel_mode = None
            if len(poly) < 3:
                self.queue_draw()
                return
            sc = self.history.scene
            chosen = []
            for s in sc.strokes:
                inside = sum(1 for p in s.points[::3] if point_in_polygon(p[0], p[1], poly))
                if inside >= max(1, 0.6 * len(s.points[::3])):
                    chosen.append(s)
            for t in sc.texts:
                x0, y0, x1, y1 = render.text_bbox(t)
                if point_in_polygon((x0 + x1) / 2, (y0 + y1) / 2, poly):
                    chosen.append(t)
            for f in sc.fills:
                x0, y0, x1, y1 = f.bbox()
                if point_in_polygon((x0 + x1) / 2, (y0 + y1) / 2, poly):
                    chosen.append(f)
            self.selection = chosen
            self.queue_draw()
        elif self.sel_mode in ("move", "scale"):
            after = self.history.scene
            if after is not self.sel_before:
                self.history.set_without_history(self.sel_before)
                self.history.commit(after)
                self.mark_dirty()
            self.sel_mode = None
            self.sel_before = None
            self.scene_changed()

    def _delete_selection(self):
        ids = {id(i) for i in self.selection}
        sc = self.history.scene
        new = sc.replace(strokes=[s for s in sc.strokes if id(s) not in ids],
                         texts=[t for t in sc.texts if id(t) not in ids],
                         fills=[f for f in sc.fills if id(f) not in ids])
        self.history.commit(new)
        self.mark_dirty()
        self.selection = []
        self.scene_changed()

    def _duplicate_selection(self):
        copies = [self._transform_item(it, 30, 30, 1.0, None) for it in self.selection]
        sc = self.history.scene
        self.history.commit(sc.replace(
            strokes=sc.strokes + tuple(c for c in copies if isinstance(c, Stroke)),
            texts=sc.texts + tuple(c for c in copies if isinstance(c, TextItem)),
            fills=sc.fills + tuple(c for c in copies if isinstance(c, FillItem))))
        self.mark_dirty()
        self.selection = copies
        self.scene_changed()

    def draw_selection(self, cr):
        if self.lasso and len(self.lasso) > 1:
            cr.save()
            cr.move_to(*self.lasso[0])
            for p in self.lasso[1:]:
                cr.line_to(*p)
            cr.set_source_rgba(0.12, 0.47, 0.85, 0.9)
            cr.set_line_width(1.5)
            cr.set_dash([6, 4])
            cr.stroke()
            cr.restore()
        h = self._sel_handles()
        if h is None:
            return
        x0, y0, x1, y1 = h["box"]
        cr.save()
        cr.rectangle(x0, y0, x1 - x0, y1 - y0)
        cr.set_source_rgba(0.12, 0.47, 0.85, 0.08)
        cr.fill_preserve()
        cr.set_source_rgba(0.12, 0.47, 0.85, 0.9)
        cr.set_line_width(1.5)
        cr.set_dash([6, 4])
        cr.stroke()
        cr.restore()
        tl._handle(cr, *h["scale"], icon="radius")
        tl._close_button(cr, *h["delete"])
        # kopyala düğmesi
        cx, cy = h["copy"]
        cr.save()
        cr.arc(cx, cy, 14, 0, 2 * math.pi)
        cr.set_source_rgba(0.12, 0.47, 0.85, 0.95)
        cr.fill()
        cr.set_source_rgb(1, 1, 1)
        cr.set_line_width(2)
        cr.rectangle(cx - 6, cy - 4, 8, 9)
        cr.rectangle(cx - 2, cy - 7, 8, 9)
        cr.stroke()
        cr.restore()

    # ================================================================ metin
    def text_press(self, x, y):
        sc = self.history.scene
        for t in reversed(sc.texts):
            x0, y0, x1, y1 = render.text_bbox(t)
            if x0 - 6 <= x <= x1 + 6 and y0 - 6 <= y <= y1 + 6:
                self.show_text_dialog(x, y, existing=t)
                return
        self.show_text_dialog(x, y)

    def show_text_dialog(self, x, y, existing=None):
        dlg = Gtk.Dialog(title=_("Metin yazın"), transient_for=self, modal=True)
        dlg.set_keep_above(True)
        dlg.set_default_size(560, 260)
        dlg.add_button(_("Vazgeç"), Gtk.ResponseType.CANCEL)
        if existing is not None:
            dlg.add_button(_("Sil"), Gtk.ResponseType.REJECT)
        ok = dlg.add_button(_("Ekle") if existing is None else _("Kaydet"), Gtk.ResponseType.OK)
        ok.get_style_context().add_class("suggested-action")
        box = dlg.get_content_area()
        box.set_spacing(10)
        box.set_border_width(12)
        view = Gtk.TextView(wrap_mode=Gtk.WrapMode.WORD_CHAR)
        view.set_size_request(-1, 140)
        view.get_buffer().set_text(existing.text if existing else "")
        css = Gtk.CssProvider()
        css.load_from_data(b"textview { font-size: 20pt; }")
        view.get_style_context().add_provider(css, Gtk.STYLE_PROVIDER_PRIORITY_APPLICATION)
        scroll = Gtk.ScrolledWindow()
        scroll.add(view)
        box.pack_start(scroll, True, True, 0)
        row = Gtk.Box(spacing=8)
        row.pack_start(Gtk.Label(label=_("Yazı boyutu")), False, False, 0)
        size = Gtk.SpinButton.new_with_range(12, 160, 2)
        size.set_value(existing.size if existing else self._text_size_default())
        row.pack_start(size, False, False, 0)
        box.pack_start(row, False, False, 0)
        dlg.set_default_response(Gtk.ResponseType.OK)

        def key(_w, ev):
            # Ctrl+Enter: ekle
            if ev.keyval in (Gdk.KEY_Return, Gdk.KEY_KP_Enter) and ev.state & Gdk.ModifierType.CONTROL_MASK:
                dlg.response(Gtk.ResponseType.OK)
                return True
            return False
        view.connect("key-press-event", key)
        dlg.show_all()
        view.grab_focus()
        resp = dlg.run()
        buf = view.get_buffer()
        text = buf.get_text(buf.get_start_iter(), buf.get_end_iter(), False).rstrip()
        sz = size.get_value()
        dlg.destroy()
        sc = self.history.scene
        if resp == Gtk.ResponseType.REJECT and existing is not None:
            self.history.commit(sc.replace(texts=[t for t in sc.texts if t is not existing]))
        elif resp == Gtk.ResponseType.OK and text:
            color = existing.color if existing else self.ink_rgb
            item = TextItem(text, existing.x if existing else x, existing.y if existing else y,
                            sz, color)
            texts = [t for t in sc.texts if t is not existing] + [item]
            self.history.commit(sc.replace(texts=texts))
        else:
            return
        self.mark_dirty()
        self.scene_changed()

    def _text_size_default(self):
        return {1: 20, 2: 26, 3: 32, 4: 40, 5: 52, 6: 72}.get(self.ink_size, 32)

    # ================================================================ dolgu
    def fill_at(self, x, y):
        sc = self.history.scene
        best = None
        for s in sc.strokes:
            pts = s.points
            if len(pts) < 6:
                continue
            length = rec.path_length(pts)
            gap = math.hypot(pts[0][0] - pts[-1][0], pts[0][1] - pts[-1][1])
            if gap > max(24.0, 0.15 * length):
                continue
            poly = [(p[0], p[1]) for p in pts[::2]]
            if point_in_polygon(x, y, poly):
                area = polygon_area(poly)
                if best is None or area < best[0]:
                    best = (area, poly)
        if best is None:
            Gdk.Display.get_default().beep()
            return
        item = FillItem(best[1], self.ink_rgb, 0.45)
        self.history.commit(sc.replace(fills=sc.fills + (item,)))
        self.mark_dirty()
        self.scene_changed(item.bbox())

    # ======================================================== ölçme araçları
    def _toggle_overlay(self, kind):
        for ov in self.overlays:
            if ov.kind == kind:
                self.overlays.remove(ov)
                self._overlays_changed()
                return
        if self.draw_state in (st.NOPEN,) or self.minimized:
            self.activate_pen()
        cx, cy = self.W / 2.0, self.H / 2.0
        if kind == "ruler":
            ov = tl.Ruler(cx, cy)
        elif kind == "setsquare":
            ov = tl.SetSquare(cx - 150, cy - 150)
        elif kind == "protractor":
            ov = tl.Protractor(cx, cy + 120)
        elif kind == "compass":
            ov = tl.Compass(cx, cy)
            ov.on_arc = self._compass_arc
        elif kind == "spotlight":
            ov = tl.Spotlight(self.W, self.H)
        elif kind == "magnifier":
            self._take_screen_snapshot()
            ov = tl.Magnifier(cx, cy, lambda: [self.screen_snapshot, self.base])
        elif kind == "timer":
            ov = tl.TimerWidget(self.W - 360, 40)
        else:
            return
        self.overlays.append(ov)
        self._overlays_changed()
        if kind == "timer":
            self._ensure_anim()

    def _overlays_changed(self):
        self._update_input_shape()
        self.queue_draw()

    def _compass_arc(self, points, done):
        w, h, tip, rot, hl = st.pen_attributes(self.pen_style, self.ink_size)
        stroke = Stroke(densify([(x, y, 0.5) for x, y in points]), self.ink_rgb, w, h,
                        tip, rot, hl)
        old = self._preview_bbox()
        if done:
            self.preview = []
            self._commit_shapes([stroke])
            self.mark_dirty()
        else:
            self.preview = [stroke]
            self._queue_bbox(old)
            self._queue_bbox(self._preview_bbox())

    def overlay_press(self, x, y):
        pen_like = self.draw_state in (st.PEN, st.VANISH)
        for ov in reversed(self.overlays):
            if pen_like and ov.in_edge_zone(x, y):
                return False      # kenara yakın: cetvel boyunca çizim
            if ov.hit(x, y):
                self.active_overlay = ov
                self._ov_bbox = ov.bbox()
                ov.press(x, y)
                self._after_overlay_event(ov)
                return True
        return False

    def overlay_move(self, x, y):
        ov = self.active_overlay
        if ov is None:
            return
        old = ov.bbox()
        ov.move(x, y)
        self._queue_bbox(_union(old, ov.bbox()))

    def overlay_release(self, x, y):
        ov = self.active_overlay
        self.active_overlay = None
        if ov is not None:
            ov.release(x, y)
            self._after_overlay_event(ov)

    def _after_overlay_event(self, ov):
        if ov.closed:
            if ov in self.overlays:
                self.overlays.remove(ov)
            self.active_overlay = None
            self._overlays_changed()
        else:
            self._queue_bbox(ov.bbox())
            if isinstance(ov, tl.TimerWidget):
                self._ensure_anim()
            if not self.composited and not self.wants_full_input():
                self._update_input_shape()

    def overlay_input_rects(self):
        out = []
        for ov in self.overlays:
            x0, y0, x1, y1 = ov.bbox()
            out.append((x0, y0, x1 - x0, y1 - y0))
        return out

    def draw_overlays(self, cr, top=False):
        for ov in self.overlays:
            if isinstance(ov, tl.Spotlight) == top:
                ov.draw(cr)

    # ================================================== ekran görüntüsü
    def _capture_screen(self):
        root = Gdk.get_default_root_window()
        try:
            pb = Gdk.pixbuf_get_from_window(root, self.area.x, self.area.y, self.W, self.H)
        except Exception:
            pb = None
        if pb is None:
            return None
        surf = cairo.ImageSurface(cairo.FORMAT_RGB24, self.W, self.H)
        cr = cairo.Context(surf)
        Gdk.cairo_set_source_pixbuf(cr, pb, 0, 0)
        cr.set_operator(cairo.OPERATOR_SOURCE)
        cr.paint()
        return surf

    def _hidden_capture(self, callback):
        """Pencereyi kısa süre gizleyip alttaki ekranı yakalar."""
        if self.backdrop is not None and not self.composited:
            callback(self.backdrop)
            return
        self.shape_combine_region(cairo.Region())
        self.input_shape_combine_region(cairo.Region())

        def grab():
            surf = self._capture_screen()
            self.shape_combine_region(None)
            self._update_input_shape()
            callback(surf)
            self.queue_draw()
            return False
        GLib.timeout_add(250, grab)

    def _take_screen_snapshot(self):
        def done(surf):
            self.screen_snapshot = surf
        self._hidden_capture(done)

    def screenshot_to_page(self):
        """Ekranın görüntüsünü alıp arka plan sayfası yapar (üzerine yazılır)."""
        def done(surf):
            if surf is None:
                Gdk.Display.get_default().beep()
                return
            d = os.path.join(data_home(), "Ekran Görüntüleri")
            os.makedirs(d, exist_ok=True)
            path = os.path.join(d, time.strftime("Ekran %Y-%m-%d %H.%M.%S.png"))
            surf.write_to_png(path)
            self._image_cache.pop(path, None)
            sc = self.history.scene
            self.history.commit(sc.replace(backgrounds=sc.backgrounds + (path,)))
            self.mark_dirty()
            self.scene_changed()
        if self.draw_state == st.NOPEN:
            self.activate_pen()
        self._hidden_capture(done)

    # ========================================================== temizleme
    def _confirm(self, title, text):
        dlg = Gtk.MessageDialog(transient_for=self, modal=True,
                                message_type=Gtk.MessageType.QUESTION,
                                buttons=Gtk.ButtonsType.NONE, text=title)
        dlg.format_secondary_text(text)
        dlg.add_button(_("Vazgeç"), Gtk.ResponseType.CANCEL)
        btn = dlg.add_button(_("Sil"), Gtk.ResponseType.OK)
        btn.get_style_context().add_class("destructive-action")
        dlg.set_keep_above(True)
        resp = dlg.run()
        dlg.destroy()
        return resp == Gtk.ResponseType.OK

    def clear_page_confirm(self):
        if self.history.scene.is_empty():
            return
        if self._confirm(_("Tüm çizimler silinsin mi?"),
                         _("Bu sayfadaki her şey silinir. Geri al ile geri getirebilirsiniz.")):
            self.clear_selection()
            self.history.commit(Scene())
            self.mark_dirty()
            self.scene_changed()

    # ============================================================ sayfalar
    def _page_switched(self):
        self.cancel_triangle()
        self.clear_selection()
        self.base = None
        self.scene_changed()

    def go_page(self, index):
        self.place_library_item()
        if self.book.go(index):
            self._page_switched()

    def new_page(self):
        self.place_library_item()
        if self.book.new_page():
            self.mark_dirty()
            self._page_switched()

    def delete_page_confirm(self):
        if not self.history.scene.is_empty() and not self._confirm(
                _("Bu sayfa silinsin mi?"),
                _("Bu sayfadaki her şey silinir. Geri al ile geri getirebilirsiniz.")):
            return
        self.book.delete_page()
        self.mark_dirty()
        self._page_switched()

    # ====================================================== renk paleti
    def remember_color(self, rgb):
        self.settings.add_recent_color(rgb)
        self.settings.save()

    def _swatch_clicked(self, name):
        if name == "palette:add":
            h = rgb_to_hex(self.ink_rgb)
            pal = [c for c in self.settings["CustomPalette"] if c != h]
            self.settings["CustomPalette"] = (pal + [h])[-4:]
            self.settings.save()
            self.queue_draw()
            return
        kind, idx = name.split(":")
        lst = self.settings["RecentColors"] if kind == "recent" else self.settings["CustomPalette"]
        idx = int(idx)
        if idx >= len(lst):
            return
        self.custom_rgb = hex_to_rgb(lst[idx])
        self.color_no = 7
        self.collapse_submenus()
        if self.draw_state not in st.SHAPES and self.draw_state not in (st.VANISH, st.TEXT, st.FILL):
            self.activate_pen()
        self.queue_draw()

    def remove_palette_color(self, name):
        kind, idx = name.split(":")
        key = "RecentColors" if kind == "recent" else "CustomPalette"
        lst = list(self.settings[key])
        if int(idx) < len(lst):
            del lst[int(idx)]
            self.settings[key] = lst
            self.settings.save()
            self.queue_draw()

    # ================================================= dosya iletişim kutuları
    def _file_dialog(self, title, action, name=None, folder=None, patterns=(), filter_name=""):
        test_dir = os.environ.get("FATIHKALEM_TEST_DIR")
        if test_dir:           # otomatik testler için
            if action == Gtk.FileChooserAction.SAVE:
                return os.path.join(test_dir, name or "test")
            return os.environ.get("FATIHKALEM_TEST_OPEN")
        dlg = Gtk.FileChooserDialog(title=title, transient_for=self, action=action)
        dlg.add_buttons(_("Vazgeç"), Gtk.ResponseType.CANCEL,
                        _("Kaydet") if action == Gtk.FileChooserAction.SAVE else _("Aç"),
                        Gtk.ResponseType.ACCEPT)
        dlg.set_keep_above(True)
        dlg.set_modal(True)
        if action == Gtk.FileChooserAction.SAVE:
            dlg.set_do_overwrite_confirmation(True)
            if name:
                dlg.set_current_name(name)
        if patterns:
            flt = Gtk.FileFilter()
            flt.set_name(filter_name)
            for p in patterns:
                flt.add_pattern(p)
                flt.add_pattern(p.upper())
            dlg.add_filter(flt)
        if folder and os.path.isdir(folder):
            dlg.set_current_folder(folder)
        for extra in (lesson.lessons_dir(), lesson.autosave_dir()):
            try:
                dlg.add_shortcut_folder(extra)
            except GLib.Error:
                pass
        for media in ("/media/" + os.environ.get("USER", ""), "/run/media/" + os.environ.get("USER", "")):
            if os.path.isdir(media):
                for sub in os.listdir(media):
                    try:
                        dlg.add_shortcut_folder(os.path.join(media, sub))
                    except GLib.Error:
                        pass
        resp = dlg.run()
        path = dlg.get_filename() if resp == Gtk.ResponseType.ACCEPT else None
        dlg.destroy()
        return path

    def _notify(self, text, error=False):
        if error:
            logging.getLogger("fatih-kalem").exception(text)
        dlg = Gtk.MessageDialog(transient_for=self, modal=False,
                                message_type=Gtk.MessageType.ERROR if error else Gtk.MessageType.INFO,
                                buttons=Gtk.ButtonsType.OK, text=text)
        dlg.set_keep_above(True)
        dlg.connect("response", lambda d, r: d.destroy())
        dlg.show()
        if not error:
            GLib.timeout_add_seconds(4, lambda: (dlg.destroy(), False)[1])

    def _pictures_dir(self):
        from .system import default_pictures_dir
        return default_pictures_dir()

    def export_png_dialog(self):
        path = self._file_dialog(_("Çizimleri resim olarak kaydet"), Gtk.FileChooserAction.SAVE,
                                 lesson.default_name(".png"), self._pictures_dir(),
                                 ("*.png",), "PNG")
        if not path:
            return
        try:
            out = lesson.export_png(path, self.history.scene, self.W, self.H, self._load_picture)
            self._notify(_("Kaydedildi: %s") % out)
        except Exception as e:
            self._notify(_("Kaydedilemedi: %s") % e, error=True)

    def export_pdf_dialog(self):
        path = self._file_dialog(_("Tüm sayfaları PDF olarak kaydet"), Gtk.FileChooserAction.SAVE,
                                 lesson.default_name(".pdf"), lesson.lessons_dir(),
                                 ("*.pdf",), "PDF")
        if not path:
            return
        try:
            out = lesson.export_pdf(path, [h.scene for h in self.book.pages], self.W, self.H,
                                    self._load_picture)
            self._notify(_("Kaydedildi: %s") % out)
        except Exception as e:
            self._notify(_("Kaydedilemedi: %s") % e, error=True)

    def save_lesson_dialog(self):
        path = self._file_dialog(_("Dersi kaydet"), Gtk.FileChooserAction.SAVE,
                                 lesson.default_name(lesson.EXTENSION), lesson.lessons_dir(),
                                 ("*" + lesson.EXTENSION,), _("Fatih Kalem dersi"))
        if not path:
            return
        try:
            out = lesson.save_lesson(path, self.book, self.W, self.H)
            self._dirty = False
            self._notify(_("Kaydedildi: %s") % out)
        except Exception as e:
            self._notify(_("Kaydedilemedi: %s") % e, error=True)

    def open_lesson_dialog(self):
        path = self._file_dialog(_("Ders aç"), Gtk.FileChooserAction.OPEN, None,
                                 lesson.lessons_dir(), ("*" + lesson.EXTENSION,),
                                 _("Fatih Kalem dersi"))
        if path:
            self.open_lesson(path)

    def open_lesson(self, path):
        try:
            book, _size = lesson.load_lesson(path)
        except Exception as e:
            self._notify(_("Açılamadı: %s") % e, error=True)
            return
        self.place_library_item()
        self.book = book
        if self.minimized:
            self.toggle_minimized()
        self._page_switched()

    # ============================================================ paylaşım
    def toggle_share(self):
        if self.share_server is not None:
            self.share_server.stop()
            self.share_server = None
            return
        if share.local_ip() is None:
            self._notify(_("Ağ bağlantısı bulunamadı."), error=True)
            return
        d = os.path.join(os.path.expanduser("~/.cache"), "fatih-kalem", "paylasim")
        os.makedirs(d, exist_ok=True)
        path = os.path.join(d, time.strftime("Ders %Y-%m-%d.pdf"))
        try:
            lesson.export_pdf(path, [h.scene for h in self.book.pages], self.W, self.H,
                              self._load_picture)
            self.share_server = share.FileShare(path)
            url = self.share_server.start()
        except Exception as e:
            self.share_server = None
            self._notify(_("Kaydedilemedi: %s") % e, error=True)
            return
        self._show_share_dialog(url)

    def _show_share_dialog(self, url):
        matrix = qr.encode(url)
        dlg = Gtk.Window(title=_("Paylaş"), transient_for=self)
        dlg.set_keep_above(True)
        dlg.set_position(Gtk.WindowPosition.CENTER_ALWAYS)
        box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=10)
        box.set_border_width(18)
        title = Gtk.Label()
        title.set_markup("<span size='x-large' weight='bold'>%s</span>" %
                         GLib.markup_escape_text(_("Telefonunuzla QR kodu okutun")))
        box.pack_start(title, False, False, 0)
        area = Gtk.DrawingArea()
        area.set_size_request(380, 380)
        area.connect("draw", lambda w, cr: qr.draw(cr, matrix, 0, 0, w.get_allocated_height()))
        box.pack_start(area, False, False, 0)
        lbl = Gtk.Label(label=_("Aynı ağdaki cihazlar bu adresten indirebilir:"))
        box.pack_start(lbl, False, False, 0)
        link = Gtk.Label()
        link.set_selectable(True)
        link.set_markup("<tt>%s</tt>" % GLib.markup_escape_text(url))
        box.pack_start(link, False, False, 0)
        stop = Gtk.Button(label=_("Paylaşımı durdur"))
        stop.set_size_request(-1, 44)
        box.pack_start(stop, False, False, 0)
        dlg.add(box)

        def close(*_a):
            if self.share_server is not None:
                self.share_server.stop()
                self.share_server = None
            dlg.destroy()
            self.queue_draw()
        stop.connect("clicked", close)
        dlg.connect("delete-event", lambda *a: (close(), True)[1])
        dlg.show_all()

    # ================================================================ kura
    def show_lots_dialog(self):
        dlg = Gtk.Dialog(title=_("Kura çek"), transient_for=self, modal=False)
        dlg.set_keep_above(True)
        dlg.set_default_size(520, 520)
        box = dlg.get_content_area()
        box.set_spacing(8)
        box.set_border_width(14)
        box.pack_start(Gtk.Label(label=_("Öğrenci listesi (her satıra bir isim)"), xalign=0),
                       False, False, 0)
        view = Gtk.TextView()
        view.get_buffer().set_text(self.settings["StudentList"])
        scroll = Gtk.ScrolledWindow()
        scroll.set_min_content_height(150)
        scroll.add(view)
        box.pack_start(scroll, True, True, 0)
        row = Gtk.Box(spacing=8)
        row.pack_start(Gtk.Label(label=_("ya da numara aralığı")), False, False, 0)
        rng = Gtk.Entry(text=self.settings["LotRange"])
        rng.set_width_chars(10)
        row.pack_start(rng, False, False, 0)
        box.pack_start(row, False, False, 0)
        no_repeat = Gtk.CheckButton(label=_("Seçilenleri tekrar seçme"))
        no_repeat.set_active(True)
        box.pack_start(no_repeat, False, False, 0)
        result = Gtk.Label()
        result.set_size_request(-1, 110)
        box.pack_start(result, False, False, 0)
        btn = Gtk.Button(label=_("Çek"))
        btn.set_size_request(-1, 54)
        btn.get_style_context().add_class("suggested-action")
        box.pack_start(btn, False, False, 0)

        def candidates():
            buf = view.get_buffer()
            names = [n.strip() for n in buf.get_text(buf.get_start_iter(), buf.get_end_iter(),
                                                     False).splitlines() if n.strip()]
            if names:
                return names
            text = rng.get_text().replace(" ", "")
            try:
                a, b = text.split("-")
                a, b = int(a), int(b)
                if a > b:
                    a, b = b, a
                return [str(i) for i in range(a, min(b, a + 999) + 1)]
            except ValueError:
                return []

        def show(text, final=False):
            color = "#f05a28" if final else "#525252"
            result.set_markup("<span size='48000' weight='bold' foreground='%s'>%s</span>"
                              % (color, GLib.markup_escape_text(text)))

        def draw(*_a):
            buf = view.get_buffer()
            self.settings["StudentList"] = buf.get_text(buf.get_start_iter(), buf.get_end_iter(), False)
            self.settings["LotRange"] = rng.get_text()
            self.settings.save()
            pool = candidates()
            if no_repeat.get_active():
                left = [c for c in pool if c not in self._picked_lots]
                if not left:
                    self._picked_lots.clear()
                    left = pool
                pool = left
            if not pool:
                return
            winner = random.choice(pool)
            self._picked_lots.add(winner)
            steps = [0]

            def spin():
                steps[0] += 1
                if steps[0] >= 14:
                    show(winner, True)
                    return False
                show(random.choice(pool))
                return True
            GLib.timeout_add(70, spin)

        btn.connect("clicked", draw)
        dlg.connect("response", lambda d, r: d.destroy())
        dlg.show_all()

    # ======================================================= otomatik kayıt
    def _autosave_tick(self):
        if self.settings["Autosave"] and self._dirty:
            self.autosave_now()
        return True

    def autosave_now(self):
        try:
            if lesson.autosave(self.book, self.W, self.H):
                self._dirty = False
        except Exception:
            pass

    # ============================================================ güncelleme
    def new_version_available(self, tag, url):
        dlg = Gtk.MessageDialog(transient_for=self, modal=False,
                                message_type=Gtk.MessageType.INFO,
                                buttons=Gtk.ButtonsType.NONE,
                                text=_("Yeni bir sürüm var: %s") % tag)
        dlg.add_button(_("Kapat"), Gtk.ResponseType.CLOSE)
        dlg.add_button(_("İndirme sayfasını aç"), Gtk.ResponseType.ACCEPT)
        dlg.set_keep_above(True)

        def resp(d, r):
            if r == Gtk.ResponseType.ACCEPT:
                Gtk.show_uri_on_window(None, url, Gdk.CURRENT_TIME)
            else:
                self.settings["DismissedVersion"] = tag
                self.settings.save()
            d.destroy()
        dlg.connect("response", resp)
        dlg.show()
