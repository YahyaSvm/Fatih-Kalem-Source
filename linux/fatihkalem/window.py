"""Fatih Kalem ana penceresi.

Ekranı (çalışma alanını) kaplayan saydam, her zaman üstte duran bir pencere.
  * El modu (küçültülmüş): yalnızca küçük menü görünür, ekranın geri kalanı
    tıklamaları alttaki uygulamalara geçirir (X11 giriş şekli).
  * Kalem modu: dokunma/kalem/fare ile çizim, silgi, şekiller, perde,
    kütüphane görselleri ve arka plan sayfaları.
  * Kalemsiz mod (NoPen): çizimler ekranda kalır ama tıklamalar alttaki
    uygulamalara geçer.

Bileşikleştirici (compositor) olmayan masaüstlerinde saydamlık yerine ekran
görüntüsü + X11 şekil (SHAPE) yöntemi kullanılır; böylece eski Faz-1
tahtalarında da çalışır.
"""

import math
import os
import time

import cairo
import gi

gi.require_version("Gdk", "3.0")
gi.require_version("Gtk", "3.0")
from gi.repository import Gdk, GLib, Gtk  # noqa: E402

from . import i18n, papers, render, resources as res  # noqa: E402
from . import styles as st  # noqa: E402
from . import tools as tl  # noqa: E402
from .config import MAX_FAVOURITES, hex_to_rgb, rgb_to_hex  # noqa: E402
from .features import FeaturesMixin, _union  # noqa: E402
from .i18n import _  # noqa: E402
from .ink import (Eraser, Stroke, densify, draw_segments,  # noqa: E402
                  interpolate_centers, render_strokes)
from .scene import Book, LibraryItem  # noqa: E402
from .toolbar import (FRAME_BLUE, MINIMIZED_HEIGHT, ORANGE,  # noqa: E402
                      SUBMENU_ACTIONS, Toolbar, expanded_height,
                      favourite_tag_for, tools_submenu_action)

LONG_PRESS_MS = 650
GESTURE_RADIUS = 125.0
TICK_MS = 15


class Mover:
    """Menünün sürtünmeli kayması (tmrAcceleration / tmrSideArrows)."""

    def __init__(self, vx, vy, decay):
        self.vx, self.vy = vx, vy      # px/ms
        self.decay = decay             # ms başına çarpan
        self.last = time.monotonic()

    def step(self, toolbar, width, height):
        now = time.monotonic()
        dt = min(50.0, (now - self.last) * 1000.0)
        self.last = now
        k = self.decay ** dt
        self.vx *= k
        self.vy *= k
        bx, by = toolbar.border_pos()
        bx += self.vx * dt
        by += self.vy * dt
        bw, bh = toolbar.border_width(), toolbar.border_height()
        if bx < 0:
            bx = 0
            self.vx = abs(self.vx)
        elif bx + bw > width:
            bx = width - bw
            self.vx = -abs(self.vx)
        if by < 0:
            by = 0
            self.vy = abs(self.vy)
        elif by + bh > height:
            by = height - bh
            self.vy = -abs(self.vy)
        toolbar.set_border_pos(bx, by)
        return abs(self.vx) > 0.01 or abs(self.vy) > 0.01


class KalemWindow(FeaturesMixin, Gtk.Window):
    def __init__(self, application, settings, monitor_index=None):
        super().__init__(application=application, title="Fatih Kalem")
        self.settings = settings
        self.book = Book()                               # çok sayfalı tahta

        # ---- durum (orijinal alan adları yorumda) ----
        self.pen_style = settings["PenStyle"]            # PenStyle
        self.active_tab = self.pen_style                 # ActiveTab
        self.color_no = settings["ColorNo"]              # ColorNo
        self.custom_rgb = hex_to_rgb(settings["InkColor"])
        self.ink_size = settings["InkSize"]              # InkSize
        self.draw_state = st.PEN                         # DrawState
        self.state_before = st.PEN                       # DrawStateBeforeAction
        self.eraser = Eraser(*st.ERASER_SIZES[2])
        self.eraser_no = 2
        self.minimized = True                            # isMinimized
        self.shown_submenu = None                        # ShownSubMenu
        self.submenu_left = False                        # isLeftSubMenu
        self.favourites = list(settings["Favourites"])
        self.gesture_enabled = settings["isGestureEnabled"]
        self.menu_scale = settings["MenuScale"]
        self.submenu_side = settings["SubmenuSide"]
        st.set_colorblind(settings["ColorBlindPalette"])
        self.W, self.H = 1, 1

        self.toolbar = Toolbar(self)
        self.mover = None
        self._mover_source = 0

        # Etkileşim durumu
        self.live = None            # çizilmekte olan Stroke
        self.live_drawn = 0
        self.preview = []           # şekil önizleme çizgileri
        self.p_start = None
        self.triangle = None        # [p1, p2] ikinci adımda
        self.curtain_sel = None     # perde seçimi (x0, y0, x1, y1)
        self.erasing = False
        self.erase_before = None
        self.last_eraser_pt = None
        self.eraser_cursor = None
        self.lib_item = None        # düzenlenen kütüphane görseli
        self.lib_mode = None        # 'move' | 'resize' | 'rotate'
        self.lib_grab = None
        self.gesture = None         # {'start':(x,y), 'pos':(x,y), 'dir':..}
        self.gesture_anim = None    # (görsel, x, y, başlangıç zamanı)
        self.drag = None            # menü sürükleme
        self.press_target = None
        self.long_press_id = 0
        self.long_press_fired = False
        self.touches = []           # [(sequence, rol)]
        self.side_hidden_until = {"left": 0.0, "right": 0.0}
        self._eraser_tip_restore = False
        self.last_side_click = 0.0
        self.settings_window = None
        self.first_run = True
        self.fading = None

        # ---- pencere ----
        self.composited = False
        self.backdrop = None        # bileşikleştirici yoksa ekran görüntüsü
        self._grab_pending = False
        self.base = None            # sahne önbelleği
        self.live_surface = None
        self._image_cache = {}
        self._init_features()
        self._setup_window(monitor_index)

    @property
    def history(self):
        """Geçerli sayfanın geri al / yinele geçmişi."""
        return self.book.current

    # ================================================================ kurulum
    def _setup_window(self, monitor_index):
        self.set_decorated(False)
        self.set_skip_taskbar_hint(True)
        self.set_skip_pager_hint(True)
        self.set_keep_above(True)
        self.set_app_paintable(True)
        self.set_resizable(False)
        self.stick()
        self.set_icon_name("fatih-kalem")

        screen = self.get_screen()
        self.composited = screen.is_composited()
        visual = screen.get_rgba_visual()
        if visual is not None and self.composited:
            self.set_visual(visual)
        screen.connect("composited-changed", self._on_composited_changed)
        screen.connect("monitors-changed", lambda *_: self._fit_to_monitor())

        self.monitor_index = monitor_index
        self._fit_to_monitor()

        self.add_events(Gdk.EventMask.BUTTON_PRESS_MASK
                        | Gdk.EventMask.BUTTON_RELEASE_MASK
                        | Gdk.EventMask.POINTER_MOTION_MASK
                        | Gdk.EventMask.TOUCH_MASK
                        | Gdk.EventMask.LEAVE_NOTIFY_MASK)
        self.connect("draw", self._on_draw)
        self.connect("button-press-event", self._on_button_press)
        self.connect("button-release-event", self._on_button_release)
        self.connect("motion-notify-event", self._on_motion)
        self.connect("touch-event", self._on_touch)
        self.connect("realize", lambda *_: self._after_realize())
        self.connect("delete-event", self._on_delete)

    def _monitor_area(self):
        """Seçilen ekranın çalışma alanı; -2 ise tüm ekranların birleşimi."""
        display = Gdk.Display.get_default()
        idx = self.monitor_index if self.monitor_index is not None else self.settings["Monitor"]
        count = display.get_n_monitors()
        if idx == -2 and count > 1:
            rects = [display.get_monitor(i).get_workarea() for i in range(count)]
            area = Gdk.Rectangle()
            area.x = min(r.x for r in rects)
            area.y = min(r.y for r in rects)
            area.width = max(r.x + r.width for r in rects) - area.x
            area.height = max(r.y + r.height for r in rects) - area.y
            return area
        mon = display.get_monitor(idx) if 0 <= idx < count else None
        if mon is None:
            mon = display.get_primary_monitor() or display.get_monitor(0)
        return mon.get_workarea()

    def _fit_to_monitor(self):
        """SetWindowSize: pencere çalışma alanını (panel hariç) kaplar."""
        area = self._monitor_area()
        self.area = area
        self.W, self.H = area.width, area.height
        self.move(area.x, area.y)
        self.set_size_request(self.W, self.H)
        self.resize(self.W, self.H)
        self.base = None
        self.live_surface = None
        self.toolbar.clamp(self.W, self.H)
        self.queue_draw()

    def _after_realize(self):
        self.get_window().set_events(self.get_window().get_events()
                                     | Gdk.EventMask.TOUCH_MASK)
        self._place_toolbar_at_start()
        try:
            papers.ensure_papers(self.W, self.H)
        except Exception:
            pass
        self._update_input_shape()

    def _place_toolbar_at_start(self):
        s = self.settings
        if s["isStartupPositionDefault"]:
            h = self.menu_scale * expanded_height(len(self.favourites))
            self.toolbar.set_border_pos(8, (self.H - h) / 3.0)
        else:
            self.toolbar.set_border_pos(s["Left"], s["Top"])
        self.toolbar.clamp(self.W, self.H)

    def _on_composited_changed(self, screen):
        self.composited = screen.is_composited()
        self.backdrop = None
        self._update_input_shape()
        self.queue_draw()

    def _on_delete(self, *_):
        self.quit_app()
        return True

    # ============================================================ yardımcılar
    @property
    def ink_rgb(self):
        return st.color_for(self.pen_style, self.color_no, self.custom_rgb)

    def pen_button_image(self):
        return st.pen_image_name(self.pen_style, self.color_no,
                                 self.draw_state == st.NOPEN)

    def tick_target(self):
        if self.draw_state in (st.PEN,):
            return "pen"
        if self.draw_state == st.ERASER:
            return "eraser"
        if self.draw_state in st.SHAPES:
            return "shape"
        if self.draw_state in st.TOOL_STATES:
            return "tools"
        return None

    def fav_image(self, tag):
        if tag == "Undo" and not self.history.can_undo():
            return "favgrayedundo"
        if tag == "Redo" and not self.history.can_redo():
            return "favgrayedredo"
        name = "fav" + tag
        return name if res.has_image(name) else "favline"

    def wants_full_input(self):
        """Ekranın tamamı dokunma almalı mı? (yoksa tıklamalar geçer)"""
        if self.minimized:
            return False
        if self.draw_state == st.NOPEN:
            return (self.curtain_sel is not None or self.lib_item is not None
                    or self.gesture is not None
                    or any(o.takes_input_everywhere for o in self.overlays))
        return True

    def _side_arrow_rects(self):
        if not self.settings["isSideArrowsEnabled"]:
            return []
        now = time.monotonic()
        out = []
        if now >= self.side_hidden_until["left"]:
            out.append(("left", 0, self.H - 43, 40, 43))
        if now >= self.side_hidden_until["right"]:
            out.append(("right", self.W - 40, self.H - 43, 40, 43))
        return out

    def _update_input_shape(self):
        gdk_win = self.get_window()
        if gdk_win is None:
            return
        if self.wants_full_input():
            self.input_shape_combine_region(None)
            if not self.composited:
                if self.backdrop is not None:
                    self.shape_combine_region(None)
                elif not self._grab_pending:
                    # Ekran görüntüsüne kendi menümüz girmesin: pencereyi
                    # kısa süre tamamen gizle, alttakiler yeniden çizilsin,
                    # sonra görüntüyü al ve pencereyi tüm ekrana aç.
                    self._grab_pending = True
                    self.shape_combine_region(cairo.Region())
                    GLib.timeout_add(120, self._finish_backdrop)
            return
        rects = list(self.toolbar.bounds())
        rects += [(x, y, w, h) for _, x, y, w, h in self._side_arrow_rects()]
        rects += self.overlay_input_rects()
        region = cairo.Region()
        for x, y, w, h in rects:
            region.union(cairo.RectangleInt(int(x), int(y), int(math.ceil(w)), int(math.ceil(h))))
        self.input_shape_combine_region(region)
        if not self.composited:
            # Saydamlık yok: pencereyi (X11 SHAPE ile) görünen içeriğin,
            # yani çizimlerin ve menünün piksel şekline kırp.
            self.backdrop = None
            self.shape_combine_region(
                Gdk.cairo_region_create_from_surface(self._content_mask()))

    def _content_mask(self):
        # 1 bitlik maske: yarıdan saydam pikseller (gölge, kenar yumuşatma)
        # şekle girmez; aksi halde beyaz zeminle karışıp soluk görünürler.
        surf = cairo.ImageSurface(cairo.FORMAT_A1, self.W, self.H)
        cr = cairo.Context(surf)
        cr.set_antialias(cairo.ANTIALIAS_NONE)
        if not self.history.scene.is_empty():
            self._render_scene(cr)
        if self.overlays:
            self.draw_overlays(cr)
        self.toolbar.draw(cr)
        for side, x, y, w, h in self._side_arrow_rects():
            res.paint(cr, "sidearrow" + side, x, y)
        return surf

    def _finish_backdrop(self):
        self._grab_pending = False
        if self.composited:
            return False
        if self.wants_full_input():
            self._grab_backdrop()
            self.shape_combine_region(None)
            self.queue_draw()
        else:
            self._update_input_shape()
        return False

    def _grab_backdrop(self):
        """Bileşikleştirici yokken kalem modunda arkadaki ekranın görüntüsü."""
        if self.backdrop is not None:
            return
        root = Gdk.get_default_root_window()
        try:
            pb = Gdk.pixbuf_get_from_window(root, self.area.x, self.area.y,
                                            self.W, self.H)
        except Exception:
            pb = None
        if pb is not None:
            surf = cairo.ImageSurface(cairo.FORMAT_RGB24, self.W, self.H)
            cr = cairo.Context(surf)
            Gdk.cairo_set_source_pixbuf(cr, pb, 0, 0)
            cr.paint()
            self.backdrop = surf

    # ========================================================== sahne çizimi
    def _surface(self):
        # HiDPI: yüzey cihaz pikselinde, çizim mantıksal koordinatlarda.
        scale = self.get_scale_factor() or 1
        surf = cairo.ImageSurface(cairo.FORMAT_ARGB32, self.W * scale, self.H * scale)
        surf.set_device_scale(scale, scale)
        return surf

    def _load_picture(self, path):
        surf = self._image_cache.get(path)
        if surf is None:
            try:
                from gi.repository import GdkPixbuf
                pb = GdkPixbuf.Pixbuf.new_from_file(path)
                pb = pb.apply_embedded_orientation() or pb
                surf = cairo.ImageSurface(cairo.FORMAT_ARGB32, pb.get_width(), pb.get_height())
                cr = cairo.Context(surf)
                Gdk.cairo_set_source_pixbuf(cr, pb, 0, 0)
                cr.paint()
            except Exception:
                surf = None
            self._image_cache[path] = surf
        return surf

    def _paint_library_item(self, cr, item, alpha=1.0):
        surf = self._load_picture(item.path)
        if surf is None:
            return
        cx, cy = item.center
        cr.save()
        cr.translate(cx, cy)
        cr.rotate(math.radians(item.angle))
        cr.translate(-item.width / 2.0, -item.height / 2.0)
        cr.scale(item.width / surf.get_width(), item.height / surf.get_height())
        cr.set_source_surface(surf, 0, 0)
        cr.get_source().set_filter(cairo.FILTER_GOOD)
        cr.paint_with_alpha(alpha)
        cr.restore()

    def _render_scene(self, cr, clip=None):
        render.render_scene(cr, self.history.scene, self.W, self.H,
                            self._load_picture, clip)

    def _draw_curtain(self, cr, rect, placed):
        cr.save()
        cr.set_fill_rule(cairo.FILL_RULE_EVEN_ODD)
        cr.rectangle(0, 0, self.W, self.H)
        x, y, w, h = rect
        if w > 0 and h > 0:
            cr.rectangle(x, y, w, h)
        if placed:
            cr.set_source_rgb(1, 1, 1)
        else:
            cr.set_source_rgba(0, 0, 0, 0.2)
        cr.fill_preserve()
        cr.set_source_rgba(0, 0, 1, 1)
        cr.set_line_width(0.5)
        cr.stroke()
        cr.restore()

    def _rebuild_base(self, dirty=None):
        if self.base is None:
            self.base = self._surface()
            dirty = None
        cr = cairo.Context(self.base)
        if dirty is not None:
            x0, y0, x1, y1 = dirty
            clip = (math.floor(x0) - 2, math.floor(y0) - 2,
                    math.ceil(x1 - x0) + 4, math.ceil(y1 - y0) + 4)
            cr.rectangle(*clip)
            cr.clip()
        else:
            clip = None
        cr.set_operator(cairo.OPERATOR_CLEAR)
        cr.paint()
        cr.set_operator(cairo.OPERATOR_OVER)
        self._render_scene(cr, clip)

    def scene_changed(self, dirty=None):
        self._dirty = True
        self._rebuild_base(dirty)
        if not self.composited and not self.wants_full_input():
            self._update_input_shape()
        self.queue_draw()

    # ================================================================ draw
    def _on_draw(self, _widget, cr):
        if self.base is None:
            self._rebuild_base()
        cr.set_operator(cairo.OPERATOR_SOURCE)
        if self.backdrop is not None and self.wants_full_input():
            cr.set_source_surface(self.backdrop, 0, 0)
            cr.paint()
        elif self.composited:
            cr.set_source_rgba(0, 0, 0, 0)
            cr.paint()
        else:
            cr.set_source_rgb(1, 1, 1)
            cr.paint()
        cr.set_operator(cairo.OPERATOR_OVER)

        if self.fading is not None:
            cr.push_group()

        cr.set_source_surface(self.base, 0, 0)
        cr.paint()

        if self.curtain_sel is not None:
            x0, y0, x1, y1 = self.curtain_sel
            self._draw_curtain(cr, (min(x0, x1), min(y0, y1), abs(x1 - x0),
                                    abs(y1 - y0)), placed=False)

        if self.live is not None and self.live_surface is not None:
            cr.set_source_surface(self.live_surface, 0, 0)
            cr.paint_with_alpha(0.5 if self.live.highlighter else 1.0)
        if self.preview:
            render_strokes(cr, self.preview)

        if self.vanishing:
            self.draw_vanishing(cr)

        if self.lib_item is not None:
            self._draw_library_editor(cr)

        if self.overlays:
            self.draw_overlays(cr)
        self.draw_selection(cr)
        if self.laser.points:
            self.laser.draw(cr)

        if self.gesture is not None:
            self._draw_gesture(cr)
        if self.gesture_anim is not None:
            self._draw_gesture_anim(cr)

        if self.erasing and self.eraser_cursor is not None:
            pts = self.eraser.outline(*self.eraser_cursor)
            cr.move_to(*pts[0])
            for p in pts[1:]:
                cr.line_to(*p)
            cr.close_path()
            cr.set_source_rgba(1, 1, 1, 0.55)
            cr.fill_preserve()
            cr.set_source_rgba(0.3, 0.3, 0.3, 0.9)
            cr.set_line_width(1)
            cr.stroke()

        if self.overlays:
            self.draw_overlays(cr, top=True)

        if not self.minimized and self.draw_state != st.NOPEN:
            cr.set_source_rgb(*FRAME_BLUE)       # rectFrame
            cr.set_line_width(6)
            cr.rectangle(3, 3, self.W - 6, self.H - 6)
            cr.stroke()

        self.toolbar.draw(cr)
        for side, x, y, w, h in self._side_arrow_rects():
            res.paint(cr, "sidearrow" + side, x, y)

        if self.fading is not None:
            cr.pop_group_to_source()
            cr.paint_with_alpha(max(0.0, self.fading))
        return True

    def _draw_library_editor(self, cr):
        it = self.lib_item
        self._paint_library_item(cr, it)
        cx, cy = it.center
        cr.save()
        cr.translate(cx, cy)
        cr.rotate(math.radians(it.angle))
        cr.translate(-it.width / 2.0, -it.height / 2.0)
        cr.set_source_rgb(0xDC / 255.0, 0x14 / 255.0, 0x3C / 255.0)
        cr.set_line_width(3)
        cr.rectangle(-25, -25, it.width + 50, it.height + 50)
        cr.stroke()
        res.paint(cr, "rotateicon", -50, -50)
        res.paint(cr, "resizeicon", it.width, it.height)
        cr.restore()

    def _gesture_targets(self):
        """Sol / yukarı / sağ hareketlerinin renk ve görselleri."""
        if self.pen_style == st.HIGHLIGHTER:
            return {"left": ((1, 0, 1), "gesturehighlighterred", 1),
                    "up": ((1, 1, 1 / 255.0), "gesturehighlighteryellow", 4),
                    "right": ((1 / 255.0, 1, 1), "gesturehighlighterblue", 2)}
        if self.pen_style == st.STYLOGRAPH:
            return {"left": ((230 / 255.0, 27 / 255.0, 27 / 255.0), "gesturestylographred", 1),
                    "up": ((20 / 255.0,) * 3, "gesturestylographblack", 5),
                    "right": ((0, 0, 157 / 255.0), "gesturestylographblue", 2)}
        return {"left": ((1, 0, 0), "gesturemarkerred", 1),
                "up": ((20 / 255.0,) * 3, "gesturemarkerblack", 5),
                "right": ((0, 0, 1), "gesturemarkerblue", 2)}

    @staticmethod
    def _gesture_eval(g):
        """GestureMove: baskın yön, uzaklık (en çok 125) ve saydamlık."""
        sx, sy = g["start"]
        dx, dy = g["pos"][0] - sx, g["pos"][1] - sy
        if abs(dx) > abs(dy):
            direction, dist = ("right", dx) if dx > 0 else ("left", -dx)
        else:
            direction, dist = ("down", dy) if dy > 0 else ("up", -dy)
        dist = min(GESTURE_RADIUS, max(0.0, dist))
        g["shown"] = direction
        g["dist"] = dist
        g["dir"] = direction if dist >= GESTURE_RADIUS else None
        g["opacity"] = 0.85 * (dist / GESTURE_RADIUS) ** 2

    def _draw_gesture(self, cr):
        g = self.gesture
        sx, sy = g["start"]
        direction, dist, opacity = g["shown"], g["dist"], g["opacity"]
        if opacity <= 0.0:
            return
        targets = self._gesture_targets()
        cr.push_group()
        angle = {"right": 0, "down": math.pi / 2, "left": math.pi, "up": -math.pi / 2}[direction]
        if direction == "down":
            color = FRAME_BLUE
        else:
            color = targets[direction][0]
        cr.move_to(sx, sy)
        cr.arc(sx, sy, dist, angle - math.pi / 4, angle + math.pi / 4)
        cr.close_path()
        cr.set_source_rgb(*color)
        cr.fill()
        cr.set_source_rgb(*ORANGE)
        cr.set_line_width(2)
        cr.arc(sx, sy, GESTURE_RADIUS - 1, 0, 2 * math.pi)
        cr.stroke()
        ox, oy = sx - 125, sy - 125
        res.paint(cr, targets["left"][1], ox + 10, oy + 93)
        res.paint(cr, targets["up"][1], ox + 93, oy + 10)
        res.paint(cr, targets["right"][1], ox + 177, oy + 93)
        res.paint(cr, "gestureeraser", ox + 93, oy + 177)
        cr.arc(sx, sy, 25, 0, 2 * math.pi)
        cr.set_source_rgb(*ORANGE)
        cr.fill_preserve()
        cr.set_source_rgb(1, 1, 1)
        cr.set_line_width(2)
        cr.stroke()
        cr.pop_group_to_source()
        cr.paint_with_alpha(opacity)

    def _draw_gesture_anim(self, cr):
        name, x, y, t0 = self.gesture_anim
        a = 1.0 - (time.monotonic() - t0) / 1.25
        if a <= 0:
            self.gesture_anim = None
            return
        res.paint(cr, name, x, y, alpha=a)

    # ============================================================== girdiler
    def _pressure(self, event):
        try:
            ok, p = event.get_axis(Gdk.AxisUse.PRESSURE)
        except Exception:
            ok = False
        if ok and p is not None and p > 0:
            return max(0.05, min(1.0, p))
        return 0.5

    @staticmethod
    def _source(event):
        dev = event.get_source_device()
        return dev.get_source() if dev is not None else None

    def _on_button_press(self, _w, event):
        if event.type != Gdk.EventType.BUTTON_PRESS:
            return True
        if self._source(event) == Gdk.InputSource.TOUCHSCREEN:
            return True     # dokunmalar touch-event ile işlenir
        eraser_tip = self._source(event) == Gdk.InputSource.ERASER
        if event.button == 1:
            self.pointer_down(event.x, event.y, self._pressure(event), eraser_tip)
        elif event.button == 3:
            self.secondary_down(event.x, event.y)
        return True

    def _on_button_release(self, _w, event):
        if self._source(event) == Gdk.InputSource.TOUCHSCREEN:
            return True
        if event.button == 1:
            self.pointer_up(event.x, event.y)
        elif event.button == 3:
            self.secondary_up(event.x, event.y)
        return True

    def _on_motion(self, _w, event):
        if self._source(event) == Gdk.InputSource.TOUCHSCREEN:
            return True
        if event.state & Gdk.ModifierType.BUTTON3_MASK and self.gesture is not None:
            self.gesture_move(event.x, event.y)
        elif event.state & Gdk.ModifierType.BUTTON1_MASK:
            self.pointer_move(event.x, event.y, self._pressure(event))
        return True

    def _touch_role(self, seq):
        for s, role in self.touches:
            if s == seq:
                return role
        return None

    def _on_touch(self, _w, event):
        t = event.type
        seq = event.sequence
        x, y = event.x, event.y
        if t == Gdk.EventType.TOUCH_BEGIN:
            if not self.touches:
                self.touches.append((seq, "primary"))
                self.pointer_down(x, y, 0.5, False)
            elif self._touch_role(seq) is None:
                if (self.gesture is None and self.gesture_enabled
                        and not self.minimized and self.draw_state != st.NOPEN):
                    self.touches.append((seq, "gesture"))
                    self.cancel_live()     # ilk parmağın çizgisi iptal
                    self.gesture_down(x, y)
                else:
                    self.touches.append((seq, "ignored"))
        elif t == Gdk.EventType.TOUCH_UPDATE:
            role = self._touch_role(seq)
            if role == "primary" and self.gesture is None:
                self.pointer_move(x, y, 0.5)
            elif role == "gesture":
                self.gesture_move(x, y)
        elif t in (Gdk.EventType.TOUCH_END, Gdk.EventType.TOUCH_CANCEL):
            role = self._touch_role(seq)
            self.touches = [(s, r) for s, r in self.touches if s != seq]
            if role == "primary":
                if self.gesture is None:
                    self.pointer_up(x, y)
                else:
                    self._reset_press()
            elif role == "gesture":
                self.gesture_up()
        return True

    # ------------------------------------------------------- birincil basış
    def pointer_down(self, x, y, pressure, eraser_tip):
        self._cancel_mover()
        hit = self.toolbar.hit_button(x, y)
        sub = self.toolbar.hit_submenu(x, y)
        side = self._side_arrow_at(x, y)
        if side:
            self.press_target = ("side", side)
            return
        if hit is not None:
            self.press_target = ("button", hit)
            self.toolbar.pressed = hit
            if hit == "titlebar":
                self._start_drag(x, y)
            elif hit.startswith("fav:"):
                self._start_long_press(("fav", int(hit[4:])))
            self.queue_draw()
            return
        if sub is not None:
            self.press_target = ("submenu",) + sub
            self._start_long_press(("submenu",) + sub)
            return
        if self.overlays and self.overlay_press(x, y):
            if self.shown_submenu:
                self.collapse_submenus()
            self.press_target = ("overlay",)
            return
        if self.minimized:
            self.press_target = None
            return
        self.press_target = ("canvas",)

        if self.shown_submenu:
            self.collapse_submenus()
        if self.lib_item is not None:
            if self._library_press(x, y):
                return
            self.place_library_item()
            self.press_target = None
            return
        state = self.draw_state
        if eraser_tip and state in (st.PEN,) + st.SHAPES:
            self.state_before = state
            state = st.ERASER
            self._eraser_tip_restore = True
        if state in (st.PEN, st.VANISH):
            self._begin_stroke(x, y, pressure)
        elif state == st.LASER:
            self.laser_point(x, y)
        elif state == st.SELECT:
            self.select_press(x, y)
        elif state == st.TEXT:
            self.press_target = None
            self.text_press(x, y)
        elif state == st.FILL:
            self.press_target = None
            self.fill_at(x, y)
        elif state == st.ERASER:
            self._begin_erase(x, y)
        elif state in st.SHAPES:
            self.p_start = (x, y)
        elif state == st.CURTAIN:
            self.curtain_sel = (x, y, x, y)
            self.queue_draw()

    def pointer_move(self, x, y, pressure):
        if self.drag is not None:
            self._drag_move(x, y)
            return
        if self.press_target and self.press_target[0] in ("button", "submenu", "side"):
            return
        if self.lib_mode is not None:
            self._library_move(x, y)
            return
        if self.press_target and self.press_target[0] == "overlay":
            self.overlay_move(x, y)
            return
        if self.draw_state == st.LASER and self.press_target:
            self.laser_point(x, y)
            return
        if self.draw_state == st.SELECT and self.sel_mode:
            self.select_move(x, y)
            return
        if self.live is not None:
            self._extend_stroke(x, y, pressure)
        elif self.erasing:
            self._erase_to(x, y)
        elif self.curtain_sel is not None:
            x0, y0, _, _ = self.curtain_sel
            self.curtain_sel = (x0, y0, x, y)
            self.queue_draw()
        elif self.p_start is not None or self.triangle is not None:
            self._update_shape_preview(x, y)

    def pointer_up(self, x, y):
        target = self.press_target
        self._reset_press()
        if self.drag is not None:
            self._end_drag()
            return
        if target is None:
            return
        if target[0] == "side":
            if self._side_arrow_at(x, y) == target[1]:
                self.side_arrow_clicked(target[1])
            return
        if target[0] == "button":
            if self.long_press_fired:
                return
            if self.toolbar.hit_button(x, y) == target[1]:
                self.button_clicked(target[1])
            return
        if target[0] == "submenu":
            if self.long_press_fired:
                return
            sub = self.toolbar.hit_submenu(x, y)
            if sub is not None and sub[0] == target[1]:
                self.submenu_clicked(*sub)
            return
        if target[0] == "overlay":
            self.overlay_release(x, y)
            return
        # tuval
        if self.draw_state == st.SELECT and self.sel_mode:
            self.select_release(x, y)
            return
        if self.lib_mode is not None:
            self.lib_mode = None
            self.lib_grab = None
            self.queue_draw()
            return
        if self.live is not None:
            self._end_stroke()
        elif self.erasing:
            self._end_erase()
        elif self.curtain_sel is not None:
            self._place_curtain()
        elif self.draw_state in st.SHAPES:
            self._shape_released(x, y)
        if getattr(self, "_eraser_tip_restore", False):
            self._eraser_tip_restore = False
            self.draw_state = self.state_before

    def _reset_press(self):
        if self.long_press_id:
            GLib.source_remove(self.long_press_id)
            self.long_press_id = 0
        if self.toolbar.pressed is not None:
            self.toolbar.pressed = None
            self.queue_draw()

    def _start_long_press(self, what):
        self.long_press_fired = False
        if self.long_press_id:
            GLib.source_remove(self.long_press_id)

        def fire():
            self.long_press_id = 0
            self.long_press_fired = True
            self.long_pressed(what)
            return False
        self.long_press_id = GLib.timeout_add(LONG_PRESS_MS, fire)

    # ---------------------------------------------------- ikincil (sağ tık)
    def secondary_down(self, x, y):
        if self.minimized:
            return
        sub = self.toolbar.hit_submenu(x, y)
        if sub is not None:
            self.long_pressed(("submenu",) + sub)
            return
        hit = self.toolbar.hit_button(x, y)
        if hit and hit.startswith("fav:"):
            self.long_pressed(("fav", int(hit[4:])))
            return
        if self.toolbar.contains(x, y):
            return
        if self.gesture_enabled and self.draw_state != st.NOPEN:
            self.gesture_down(x, y)

    def secondary_up(self, x, y):
        """inkCanvas_PreviewMouseRightButtonUp: önce jesti bitir, sonra
        (çekmeden bırakıldıysa) tıklanan kütüphane görselini düzenlemeye al."""
        moved = False
        if self.gesture is not None:
            moved = self.gesture.get("dist", 0.0) > 10.0
            self.gesture_up()
        if self.minimized or moved:
            return
        hit = self.toolbar.hit_button(x, y)
        if hit is None and self.toolbar.hit_submenu(x, y) is None:
            self.reselect_library(x, y)

    # ================================================================ çizim
    def _begin_stroke(self, x, y, pressure):
        x, y = self._snap_start(x, y)
        w, h, tip, rot, hl = st.pen_attributes(self.pen_style, self.ink_size)
        self.live = Stroke([(x, y, pressure)], self.ink_rgb, w, h, tip, rot, hl)
        self.live_drawn = 0
        if self.live_surface is None:
            self.live_surface = self._surface()
        cr = cairo.Context(self.live_surface)
        cr.set_operator(cairo.OPERATOR_CLEAR)
        cr.paint()
        self._draw_live_tail()

    def _extend_stroke(self, x, y, pressure):
        x, y = self._snapped(x, y)
        last = self.live.points[-1]
        if abs(last[0] - x) < 0.5 and abs(last[1] - y) < 0.5:
            return
        new = densify([last, (x, y, pressure)])[1:]
        self.live.points.extend(new)
        self.live._bbox = None
        self._draw_live_tail()

    def _draw_live_tail(self):
        s = self.live
        cr = cairo.Context(self.live_surface)
        cr.set_source_rgb(*s.color)
        start = self.live_drawn
        draw_segments(cr, s, start)
        pts = s.points[max(0, start - 1):]
        r = s.radius * 2 + 3
        x0 = min(p[0] for p in pts) - r
        y0 = min(p[1] for p in pts) - r
        x1 = max(p[0] for p in pts) + r
        y1 = max(p[1] for p in pts) + r
        self.live_drawn = len(s.points)
        self.queue_draw_area(int(x0), int(y0), int(x1 - x0) + 1, int(y1 - y0) + 1)

    def _end_stroke(self):
        s = self.live
        self.live = None
        if self.draw_state == st.VANISH:
            self.snap_edge = None
            self.add_vanishing(s)
            self._queue_bbox(s.bbox())
            return
        strokes = self._finish_stroke(s)
        sc = self.history.scene
        self.history.commit(sc.replace(strokes=sc.strokes + tuple(strokes)))
        dirty = s.bbox()
        for part in strokes:
            dirty = _union(dirty, part.bbox())
        self.scene_changed(dirty)

    def cancel_live(self):
        if self.live is not None:
            bbox = self.live.bbox()
            self.live = None
            self.queue_draw_area(int(bbox[0]), int(bbox[1]),
                                 int(bbox[2] - bbox[0]) + 1, int(bbox[3] - bbox[1]) + 1)
        self.preview = []
        self.p_start = None
        if self.erasing:
            self._end_erase()

    # --------------------------------------------------------------- silgi
    def _begin_erase(self, x, y):
        self.erasing = True
        self.erase_before = self.history.scene
        self.last_eraser_pt = None
        self._erase_to(x, y)

    def _erase_to(self, x, y):
        centers = interpolate_centers(self.last_eraser_pt, (x, y))
        self.last_eraser_pt = (x, y)
        old_cursor = self.eraser_cursor
        self.eraser_cursor = (x, y)
        sc = self.history.scene
        strokes, changed, dirty = self.eraser.erase(sc.strokes, centers)
        texts = list(sc.texts)
        if texts:
            keep = []
            for t_ in texts:
                x0, y0, x1, y1 = render.text_bbox(t_)
                if any(x0 <= cx <= x1 and y0 <= cy <= y1 for cx, cy in centers):
                    changed = True
                    dirty = _union(dirty, (x0, y0, x1, y1))
                else:
                    keep.append(t_)
            texts = keep
        if changed:
            self.history.set_without_history(sc.replace(strokes=strokes, texts=texts))
            self._rebuild_base(dirty)
        r = self.eraser.reach() + 4
        for c in filter(None, (old_cursor, (x, y))):
            self.queue_draw_area(int(c[0] - r), int(c[1] - r), int(2 * r) + 1, int(2 * r) + 1)
        if dirty:
            self.queue_draw_area(int(dirty[0]), int(dirty[1]),
                                 int(dirty[2] - dirty[0]) + 1, int(dirty[3] - dirty[1]) + 1)

    def _end_erase(self):
        self.erasing = False
        after = self.history.scene
        if after is not self.erase_before:
            self.history.set_without_history(self.erase_before)
            self.history.commit(after)
            self._dirty = True
        self.erase_before = None
        self.eraser_cursor = None
        self.last_eraser_pt = None
        if not self.composited and not self.wants_full_input():
            self._update_input_shape()
        self.queue_draw()

    # ------------------------------------------------------------- şekiller
    def _shape_size(self):
        return st.SHAPE_SIZES[self.ink_size]

    def _update_shape_preview(self, x, y):
        color, size = self.ink_rgb, self._shape_size()
        old = self._preview_bbox()
        if self.draw_state == st.TRIANGLE:
            if self.triangle is not None:
                p1, p2 = self.triangle
                self.preview = st.triangle(p1, p2, (x, y), color, size)
            elif self.p_start is not None:
                self.preview = st.triangle(self.p_start, (x, y), None, color, size)
        elif self.p_start is not None:
            self.preview = st.SHAPE_BUILDERS[self.draw_state](self.p_start, (x, y), color, size)
        self._queue_bbox(old)
        self._queue_bbox(self._preview_bbox())

    def _preview_bbox(self):
        if not self.preview:
            return None
        bb = [s.bbox() for s in self.preview]
        return (min(b[0] for b in bb) - 80, min(b[1] for b in bb) - 80,
                max(b[2] for b in bb) + 80, max(b[3] for b in bb) + 80)

    def _queue_bbox(self, bb):
        if bb:
            self.queue_draw_area(int(bb[0]), int(bb[1]), int(bb[2] - bb[0]) + 1,
                                 int(bb[3] - bb[1]) + 1)

    def _shape_released(self, x, y):
        if self.draw_state == st.TRIANGLE:
            if self.triangle is None and self.p_start is not None:
                # İlk kenar çizildi; üçüncü köşe için ikinci adım.
                if math.hypot(x - self.p_start[0], y - self.p_start[1]) > 3:
                    self.triangle = (self.p_start, (x, y))
                    self.p_start = None
                    return
                self.p_start = None
                self.preview = []
                self.queue_draw()
                return
            if self.triangle is not None:
                p1, p2 = self.triangle
                strokes = st.triangle(p1, p2, (x, y), self.ink_rgb, self._shape_size())
                self.triangle = None
                self._commit_shapes(strokes)
            return
        if self.p_start is None:
            return
        strokes = []
        if math.hypot(x - self.p_start[0], y - self.p_start[1]) > 2:
            strokes = st.SHAPE_BUILDERS[self.draw_state](self.p_start, (x, y),
                                                         self.ink_rgb, self._shape_size())
        self.p_start = None
        self._commit_shapes(strokes)

    def _commit_shapes(self, strokes):
        self.preview = []
        if strokes:
            sc = self.history.scene
            self.history.commit(sc.replace(strokes=sc.strokes + tuple(strokes)))
            bb = [s.bbox() for s in strokes]
            self.scene_changed((min(b[0] for b in bb), min(b[1] for b in bb),
                                max(b[2] for b in bb), max(b[3] for b in bb)))
        else:
            self.queue_draw()

    def cancel_triangle(self):
        if self.triangle is not None or self.preview:
            self.triangle = None
            self.preview = []
            self.p_start = None
            self.queue_draw()

    # ----------------------------------------------------------------- perde
    def show_curtain_selection(self):
        """ShowCurtainSelection: perde varsa kaldırır, yoksa seçim başlatır."""
        self.collapse_submenus()
        if self.history.scene.curtain is not None:
            self.remove_curtain()
            return
        if self.draw_state != st.CURTAIN:
            self.state_before = self.draw_state
        self.draw_state = st.CURTAIN
        self._update_input_shape()
        self.queue_draw()

    def _place_curtain(self):
        x0, y0, x1, y1 = self.curtain_sel
        self.curtain_sel = None
        rect = (min(x0, x1), min(y0, y1), abs(x1 - x0), abs(y1 - y0))
        self.draw_state = self.state_before if self.state_before != st.CURTAIN else st.PEN
        sc = self.history.scene
        self.history.commit(sc.replace(curtain=rect))
        self._update_input_shape()
        self.scene_changed()

    def remove_curtain(self):
        sc = self.history.scene
        if sc.curtain is not None:
            self.history.commit(sc.replace(curtain=None))
            self.scene_changed()

    # ------------------------------------------------ kütüphane / arka plan
    def _choose_image(self, title, folder):
        test_pick = os.environ.get("FATIHKALEM_TEST_PICK")
        if test_pick:          # otomatik testler için
            return test_pick
        dlg = Gtk.FileChooserDialog(title=title, transient_for=self,
                                    action=Gtk.FileChooserAction.OPEN)
        dlg.add_buttons(_("Vazgeç"), Gtk.ResponseType.CANCEL,
                        _("Aç"), Gtk.ResponseType.ACCEPT)
        dlg.set_keep_above(True)
        dlg.set_modal(True)
        flt = Gtk.FileFilter()
        flt.set_name(_("Resim Dosyaları"))
        for pat in ("*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.tif",
                    "*.tiff", "*.svg", "*.webp"):
            flt.add_pattern(pat)
            flt.add_pattern(pat.upper())
        dlg.add_filter(flt)
        last = self.settings["LastBrowsedPath"]
        start = folder
        if last and os.path.isdir(os.path.dirname(last)) and folder in last:
            start = os.path.dirname(last)
        if start and os.path.isdir(start):
            dlg.set_current_folder(start)
        for extra in [papers.papers_dir()] + papers.library_dirs():
            if os.path.isdir(extra):
                try:
                    dlg.add_shortcut_folder(extra)
                except GLib.Error:
                    pass
        preview = Gtk.Image()
        dlg.set_preview_widget(preview)

        def update_preview(chooser):
            path = chooser.get_preview_filename()
            ok = False
            if path and os.path.isfile(path):
                try:
                    from gi.repository import GdkPixbuf
                    pb = GdkPixbuf.Pixbuf.new_from_file_at_size(path, 220, 220)
                    preview.set_from_pixbuf(pb)
                    ok = True
                except GLib.Error:
                    pass
            chooser.set_preview_widget_active(ok)
        dlg.connect("update-preview", update_preview)
        resp = dlg.run()
        path = dlg.get_filename() if resp == Gtk.ResponseType.ACCEPT else None
        dlg.destroy()
        if path:
            self.settings["LastBrowsedPath"] = path
            self.settings.save()
        return path

    def activate_background(self):
        self.collapse_submenus()
        self.place_library_item()
        folder = papers.ensure_papers(self.W, self.H)
        path = self._choose_image(_("Arka Plan Sayfası Seç"), folder)
        if not path or self._load_picture(path) is None:
            return
        sc = self.history.scene
        self.history.commit(sc.replace(backgrounds=sc.backgrounds + (path,), curtain=None))
        self.scene_changed()

    def activate_library(self):
        self.collapse_submenus()
        self.place_library_item()
        dirs = papers.library_dirs()
        from .system import default_pictures_dir
        path = self._choose_image(_("Görsel Seç"), dirs[0] if dirs else default_pictures_dir())
        if not path:
            return
        surf = self._load_picture(path)
        if surf is None:
            return
        iw, ih = surf.get_width(), surf.get_height()
        if iw >= ih:
            w, h = 200.0, 200.0 * ih / iw
        else:
            w, h = 200.0 * iw / ih, 200.0
        self.lib_item = LibraryItem(path, (self.W - w) / 2.0, (self.H - h) / 2.0, w, h)
        self._update_input_shape()
        self.queue_draw()

    def _to_item_local(self, x, y):
        it = self.lib_item
        cx, cy = it.center
        a = -math.radians(it.angle)
        dx, dy = x - cx, y - cy
        lx = dx * math.cos(a) - dy * math.sin(a) + it.width / 2.0
        ly = dx * math.sin(a) + dy * math.cos(a) + it.height / 2.0
        return lx, ly

    def _library_press(self, x, y):
        it = self.lib_item
        lx, ly = self._to_item_local(x, y)
        cx, cy = it.center
        if -50 <= lx <= 0 and -50 <= ly <= 0:
            self.lib_mode = "rotate"
            self.lib_grab = (math.atan2(y - cy, x - cx), it.angle)
            return True
        if it.width <= lx <= it.width + 50 and it.height <= ly <= it.height + 50:
            self.lib_mode = "resize"
            self.lib_grab = (math.hypot(x - cx, y - cy), it.width, it.height)
            return True
        if -25 <= lx <= it.width + 25 and -25 <= ly <= it.height + 25:
            self.lib_mode = "move"
            self.lib_grab = (x - it.x, y - it.y)
            return True
        return False

    def _library_move(self, x, y):
        it = self.lib_item
        cx, cy = it.center
        if self.lib_mode == "move":
            it.x, it.y = x - self.lib_grab[0], y - self.lib_grab[1]
        elif self.lib_mode == "rotate":
            a0, ang0 = self.lib_grab
            it.angle = ang0 + math.degrees(math.atan2(y - cy, x - cx) - a0)
        elif self.lib_mode == "resize":
            d0, w0, h0 = self.lib_grab
            s = max(0.1, math.hypot(x - cx, y - cy) / max(1.0, d0))
            nw, nh = max(30.0, w0 * s), max(30.0, h0 * s)
            it.x, it.y = cx - nw / 2.0, cy - nh / 2.0
            it.width, it.height = nw, nh
        self.queue_draw()

    def place_library_item(self):
        """PlaceLibraryImageToBack: görseli mürekkebin altına sabitler."""
        if self.lib_item is None:
            return
        item = self.lib_item
        self.lib_item = None
        self.lib_mode = None
        sc = self.history.scene
        self.history.commit(sc.replace(library=sc.library + (item,)))
        self._update_input_shape()
        self.scene_changed()

    def reselect_library(self, x, y):
        sc = self.history.scene
        for i in range(len(sc.library) - 1, -1, -1):
            if sc.library[i].contains(x, y):
                self.place_library_item()
                sc = self.history.scene
                item = sc.library[i].clone()
                lib = sc.library[:i] + sc.library[i + 1:]
                self.history.commit(sc.replace(library=lib))
                self.lib_item = item
                self._update_input_shape()
                self.scene_changed()
                return True
        return False

    # =============================================================== jestler
    def gesture_down(self, x, y):
        if self.shown_submenu:
            self.collapse_submenus()
        self.gesture = {"start": (x, y), "pos": (x, y)}
        self._gesture_eval(self.gesture)
        self._update_input_shape()
        self.queue_draw()

    def gesture_move(self, x, y):
        if self.gesture is None:
            return
        self.gesture["pos"] = (x, y)
        self._gesture_eval(self.gesture)
        sx, sy = self.gesture["start"]
        self.queue_draw_area(int(sx - 130), int(sy - 130), 260, 260)

    def gesture_up(self):
        g = self.gesture
        self.gesture = None
        self.queue_draw()
        if g is None:
            return
        sx, sy = g["start"]
        direction = g.get("dir")
        if direction is None:
            # Kısa çekme: menüyü parmağın yanına getir.
            if self.settings["isShortGestureEnabled"] and g.get("opacity", 0) > 0.05:
                bh = self.toolbar.border_height()
                tx = sx + self.toolbar.border_width() / 2.0
                ty = sy - bh / 2.0
                bx, by = self.toolbar.border_pos()
                self._start_mover(4.9 * (tx - bx) / 1920.0, 4.9 * (ty - by) / 1920.0, 0.9976)
            return
        targets = self._gesture_targets()
        anim_pos = {"left": (sx - 115, sy - 32), "up": (sx - 32, sy - 115),
                    "right": (sx + 52, sy - 32), "down": (sx - 32, sy + 52)}[direction]
        if direction == "down":
            if self.draw_state != st.ERASER:
                self.state_before = self.draw_state
                self.activate_eraser(*st.GESTURE_ERASER, number=None)
            else:
                self._return_from_eraser()
            name = "gestureeraser"
        else:
            color = targets[direction][2]
            self.change_color(color)
            if self.draw_state == st.ERASER:
                self._return_from_eraser()
            elif self.draw_state not in st.SHAPES:
                self.activate_pen()
            name = targets[direction][1]
        self.gesture_anim = (name, anim_pos[0], anim_pos[1], time.monotonic())

        def fade():
            self.queue_draw()
            return self.gesture_anim is not None
        GLib.timeout_add(40, fade)

    def _return_from_eraser(self):
        if self.state_before in st.SHAPES:
            self.activate_shape(self.state_before)
        else:
            self.activate_pen()

    # =========================================================== menü işleri
    def _start_drag(self, x, y):
        self.drag = {"dx": x - self.toolbar.x, "dy": y - self.toolbar.y,
                     "hist": [(time.monotonic(), self.toolbar.x, self.toolbar.y)]}
        self.collapse_submenus()

    def _drag_move(self, x, y):
        self.toolbar.x = x - self.drag["dx"]
        self.toolbar.y = y - self.drag["dy"]
        self.toolbar.clamp(self.W, self.H)
        hist = self.drag["hist"]
        hist.append((time.monotonic(), self.toolbar.x, self.toolbar.y))
        if len(hist) > 12:
            del hist[0]
        self._update_input_shape()
        self.queue_draw()

    def _end_drag(self):
        hist = self.drag["hist"]
        self.drag = None
        now = time.monotonic()
        recent = [h for h in hist if now - h[0] <= 0.1]
        if len(recent) >= 2:
            t0, x0, y0 = recent[0]
            t1, x1, y1 = recent[-1]
            dt = max(1.0, (t1 - t0) * 1000.0)
            vx, vy = (x1 - x0) / dt, (y1 - y0) / dt
            if abs(vx) >= 0.1 or abs(vy) >= 0.1:
                self._start_mover(vx, vy, 0.998)
        self._update_input_shape()

    def _start_mover(self, vx, vy, decay):
        self._cancel_mover()
        self.mover = Mover(vx, vy, decay)

        def tick():
            alive = self.mover is not None and self.mover.step(self.toolbar, self.W, self.H)
            self.queue_draw()
            if not alive:
                self.mover = None
                self._mover_source = 0
                self._update_input_shape()
                return False
            return True
        self._mover_source = GLib.timeout_add(TICK_MS, tick)

    def _cancel_mover(self):
        if self._mover_source:
            GLib.source_remove(self._mover_source)
            self._mover_source = 0
        self.mover = None

    def _side_arrow_at(self, x, y):
        for side, ax, ay, w, h in self._side_arrow_rects():
            if ax <= x < ax + w and ay <= y < ay + h:
                return side
        return None

    def side_arrow_clicked(self, side):
        now = time.monotonic()
        if now - self.last_side_click < 1.0:
            # Çift dokunma: oku bir süre gizle.
            self.side_hidden_until[side] = now + 5.0
            GLib.timeout_add(5100, lambda: (self._update_input_shape(), self.queue_draw(), False)[-1])
            self.last_side_click = 0.0
            self._update_input_shape()
            self.queue_draw()
            return
        self.last_side_click = now
        bx, _by = self.toolbar.border_pos()
        if side == "left":
            target = 0.0
        else:
            target = self.W - self.toolbar.border_width()
        self._start_mover(4.9 * (target - bx) / 1920.0, 0.0, 0.9976)

    def collapse_submenus(self):
        if self.shown_submenu:
            self.shown_submenu = None
            self._update_input_shape()
            self.queue_draw()

    def _show_submenu(self, name):
        self.submenu_left = self.toolbar.choose_side(self.W)
        self.shown_submenu = name
        self._update_input_shape()
        self.queue_draw()

    def toggle_minimized(self):
        """imgHandAndPen: el modu <-> kalem modu."""
        if self.minimized:
            self.minimized = False
            if self.draw_state in (st.NOPEN, st.CURTAIN):
                self.draw_state = st.PEN
            self.activate_pen()
            self.toolbar.clamp(self.W, self.H)
            bx, by = self.toolbar.border_pos()
            bh = self.toolbar.border_height()
            if by + bh + 10 > self.H:
                self.toolbar.set_border_pos(bx, max(0, self.H - bh - 10))
            self.backdrop = None
            self.present()
        else:
            self.cancel_triangle()
            self.collapse_submenus()
            self.place_library_item()
            self.gesture = None
            self.curtain_sel = None
            self.clear_selection()
            self.overlays = [o for o in self.overlays if isinstance(o, tl.TimerWidget)]
            if not self.first_run and not self.book.is_empty():
                # ClearCanvas: önce otomatik kaydet ("Ders aç" ile geri gelir).
                self.autosave_now()
                self.book.reset()
                self.base = None
            self.minimized = True
            self.scene_changed()
        self.first_run = False
        self._update_input_shape()
        self.queue_draw()

    def button_clicked(self, name):
        if name == "handpen":
            self.toggle_minimized()
        elif name == "titlebar":
            pass
        elif name == "pen":
            self.cancel_triangle()
            if self.shown_submenu == "pen":
                self.collapse_submenus()
                return
            self.collapse_submenus()
            if self.draw_state != st.NOPEN:
                self.activate_pen()
            self.active_tab = self.pen_style
            self._show_submenu("pen")
        elif name == "eraser":
            self.cancel_triangle()
            if self.shown_submenu == "eraser":
                self.collapse_submenus()
                return
            self._show_submenu("eraser")
        elif name == "shape":
            self.cancel_triangle()
            if self.shown_submenu == "shape":
                self.collapse_submenus()
                return
            self._show_submenu("shape")
        elif name == "tools":
            self.cancel_triangle()
            if self.shown_submenu == "tools":
                self.collapse_submenus()
                return
            self.collapse_submenus()
            self._show_submenu("tools")
        elif name == "settings":
            self.collapse_submenus()
            self.open_settings()
        elif name == "close":
            self.cancel_triangle()
            if not self.settings["isCloseConfirmationEnabled"]:
                self.quit_app()
                return
            if self.shown_submenu == "close":
                self.collapse_submenus()
            else:
                self._show_submenu("close")
        elif name.startswith("fav:"):
            idx = int(name[4:])
            if idx < len(self.favourites):
                self.run_favourite(self.favourites[idx])

    def submenu_clicked(self, name, x, y):
        if name == "tools":
            action = tools_submenu_action(self.toolbar, x, y)
            if action is not None:
                self.perform_tool(action[0])
            return
        action = SUBMENU_ACTIONS[name](x, y)
        if action is None:
            return
        self.perform(name, action)

    def perform(self, submenu, action, from_fav=False):
        kind, val = action
        if submenu == "pen":
            if kind == "tab":
                self.active_tab = val
                if self.pen_style != val or self.draw_state != st.PEN:
                    self.pen_style = val
                    self.change_color(self.color_no)
                    self.activate_pen()
                self.queue_draw()
                return
            self.pen_style = self.active_tab
            if kind == "color":
                self.change_color(val)
                self.activate_pen()
            elif kind == "colorful":
                self.collapse_submenus()
                self._show_submenu("color")
                return
            elif kind == "nopen":
                self.set_nopen()
            elif kind == "size":
                self.ink_size = val
                if self.draw_state not in st.SHAPES and self.draw_state != st.NOPEN:
                    self.activate_pen()
                if from_fav:
                    self.queue_draw()
                    return
                if self.active_tab == self.pen_style and self.shown_submenu:
                    self.queue_draw()
                    return
            self.collapse_submenus()
        elif submenu == "eraser":
            if kind == "undo":
                self.undo()
            elif kind == "redo":
                self.redo()
            elif kind == "curtain":
                self.show_curtain_selection()
            elif kind == "eraser":
                self.activate_eraser(*st.ERASER_SIZES[val], number=val)
                self.collapse_submenus()
        elif submenu == "shape":
            if self.draw_state == st.NOPEN:
                self.activate_pen()
            if kind == "shape":
                self.activate_shape(val)
            elif kind == "library":
                self.activate_library()
            elif kind == "background":
                self.activate_background()
        elif submenu == "color":
            if kind == "cartela":
                self.custom_rgb = tuple(c / 255.0 for c in val)
                self.remember_color(self.custom_rgb)
                self.pen_style = self.active_tab
                self.color_no = 7
                self.activate_pen()
                self.collapse_submenus()
        elif submenu == "close":
            if kind == "quit":
                self.quit_app()

    # ------------------------------------------------------ sık kullanılanlar
    def long_pressed(self, what):
        if what[0] == "fav":
            idx = what[1]
            if idx < len(self.favourites):
                del self.favourites[idx]
                self.settings["Favourites"] = list(self.favourites)
                self.settings.save()
                self.toolbar.pressed = None
                self._update_input_shape()
                self.queue_draw()
            return
        _, name, x, y = what
        if name == "tools":
            action = tools_submenu_action(self.toolbar, x, y)
            if action is not None and action[0].split(":")[0] in ("recent", "palette") \
                    and action[0] != "palette:add":
                self.remove_palette_color(action[0])
                return
        else:
            action = SUBMENU_ACTIONS[name](x, y)
        if action is None:
            return
        tag = favourite_tag_for(name, action, self.active_tab)
        if not tag or tag in self.favourites or len(self.favourites) >= MAX_FAVOURITES:
            return
        bh = self.menu_scale * expanded_height(len(self.favourites) + 1)
        if bh > self.H:
            return
        self.favourites.append(tag)
        self.settings["Favourites"] = list(self.favourites)
        self.settings.save()
        self.toolbar.clamp(self.W, self.H)
        self._update_input_shape()
        self.queue_draw()

    def run_favourite(self, tag):
        self.collapse_submenus()
        if tag.startswith("Tool:"):
            self.perform_tool(tag[5:])
            return
        if tag in ("Undo", "Redo", "Curtain"):
            self.perform("eraser", (tag.lower(), None), from_fav=True)
            return
        if tag.startswith("Eraser"):
            self.perform("eraser", ("eraser", int(tag[6:])), from_fav=True)
            return
        if tag.startswith("Size"):
            self.active_tab = self.pen_style
            self.perform("pen", ("size", int(tag[4:])), from_fav=True)
            return
        if tag in ("Library", "Background"):
            self.perform("shape", (tag.lower(), None), from_fav=True)
            return
        shapes = {"Line": st.LINE, "DashLine": st.DASHLINE, "Arrow": st.ARROW,
                  "Rectangle": st.RECTANGLE, "Ellipse": st.ELLIPSE,
                  "Triangle": st.TRIANGLE}
        if tag in shapes:
            self.perform("shape", ("shape", shapes[tag]), from_fav=True)
            return
        for style, sname in st.STYLE_NAMES.items():
            if tag.startswith(sname):
                rest = tag[len(sname):]
                self.active_tab = style
                self.pen_style = style
                if rest == "NoPen":
                    self.perform("pen", ("nopen", None), from_fav=True)
                elif rest == "ColorFul":
                    self._show_submenu("color")
                else:
                    if rest == "Yellow":
                        rest = "Orange"
                    nums = {v: k for k, v in st.COLOR_NAMES.items()}
                    self.perform("pen", ("color", nums.get(rest, 2)), from_fav=True)
                return

    # ------------------------------------------------------- durum değişimi
    def change_color(self, no):
        self.color_no = no
        self.queue_draw()

    def activate_pen(self):
        """ActivatePen"""
        self.place_library_item()
        self.clear_selection()
        self.draw_state = st.PEN
        self.state_before = st.PEN
        self._update_input_shape()
        self.queue_draw()

    def set_nopen(self):
        self.cancel_triangle()
        self.draw_state = st.NOPEN
        self.collapse_submenus()
        self._update_input_shape()
        self.queue_draw()

    def activate_eraser(self, w, h, number=None):
        if self.draw_state == st.NOPEN:
            self.activate_pen()
        self.cancel_triangle()
        self.eraser = Eraser(w, h)
        if number is not None:
            self.eraser_no = number
        self.draw_state = st.ERASER
        self._update_input_shape()
        self.queue_draw()

    def activate_shape(self, shape):
        self.cancel_triangle()
        self.draw_state = shape
        self.triangle = None
        self.collapse_submenus()
        self._update_input_shape()
        self.queue_draw()

    def undo(self):
        self.cancel_triangle()
        self.clear_selection()
        self.place_library_item()
        if self.history.undo():
            self.scene_changed()
        self.queue_draw()

    def redo(self):
        self.cancel_triangle()
        self.clear_selection()
        if self.history.redo():
            self.scene_changed()
        self.queue_draw()

    # -------------------------------------------------------------- ayarlar
    def open_settings(self):
        from .settings_window import SettingsWindow
        if self.settings_window is None:
            self.settings_window = SettingsWindow(self)
            self.settings_window.connect("destroy", self._settings_closed)
        self.settings_window.present()

    def _settings_closed(self, *_):
        self.settings_window = None

    def save_current_pen(self):
        s = self.settings
        s["PenStyle"] = self.pen_style
        s["ColorNo"] = self.color_no
        s["InkSize"] = self.ink_size
        s["InkColor"] = rgb_to_hex(self.custom_rgb)
        s.save()

    def save_current_position(self):
        s = self.settings
        s["isStartupPositionDefault"] = False
        bx, by = self.toolbar.border_pos()
        s["Left"] = int(round(bx))
        s["Top"] = int(round(by))
        s.save()

    def favourites_changed(self):
        self.favourites = list(self.settings["Favourites"])
        self.toolbar.clamp(self.W, self.H)
        self._update_input_shape()
        self.queue_draw()

    def settings_changed(self):
        s = self.settings
        self.gesture_enabled = s["isGestureEnabled"]
        self.menu_scale = s["MenuScale"]
        self.submenu_side = s["SubmenuSide"]
        st.set_colorblind(s["ColorBlindPalette"])
        i18n.set_language(s["Language"])
        self.toolbar.clamp(self.W, self.H)
        self._update_input_shape()
        self.queue_draw()

    # ---------------------------------------------------------------- çıkış
    def quit_app(self):
        """CloseMe: 200 ms'de solarak kapanır."""
        if self.fading is not None:
            return
        if self._dirty and self.settings["Autosave"]:
            self.autosave_now()
        if self.share_server is not None:
            self.share_server.stop()
            self.share_server = None
        self.fading = 1.0
        t0 = time.monotonic()

        def step():
            self.fading = 1.0 - (time.monotonic() - t0) / 0.2
            self.queue_draw()
            if self.fading <= 0:
                app = self.get_application()
                self.destroy()
                if app is not None:
                    app.quit()
                return False
            return True
        GLib.timeout_add(TICK_MS, step)


__all__ = ["KalemWindow", "MINIMIZED_HEIGHT"]
