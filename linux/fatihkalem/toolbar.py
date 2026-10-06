"""Ana menü (araç çubuğu) ve alt menülerin yerleşimi, çizimi ve tıklama
bölgeleri.

Kalem, silgi ve şekiller menülerinin koordinatları ve görselleri ilk Fatih
Kalem'deki değerlerle birebir aynıdır. 2.1 ile şekillerin altına "Araçlar"
düğmesi ve vektörle çizilen Araçlar alt menüsü eklendi.

Tüm yerleşim "yerel" koordinatlarda tanımlıdır; menü boyutu ayarı
(MenuScale) çizimi ve dokunma bölgelerini birlikte ölçekler.
"""

import math

import cairo

from . import resources as res
from . import icons
from .i18n import _
from .styles import HIGHLIGHTER, MARKER, STYLOGRAPH

BORDER_X, BORDER_Y, BORDER_W = 177, 10, 46
BTN_X = 179
MINIMIZED_HEIGHT = 76
BYLINE_HEIGHT = 16
BYLINE_TEXT = "By YhySvm"
ORANGE = (0xF0 / 255.0, 0x5A / 255.0, 0x28 / 255.0)
FRAME_BLUE = (0x1E / 255.0, 0x90 / 255.0, 0xFF / 255.0)

TOOLS_Y = 210          # Araçlar düğmesi (şekillerin altında)
FIRST_FAV_Y = 252      # ilk sık kullanılan öğe

# Alt menü adı -> (sağ x, sol x, y, genişlik, yükseklik)
SUBMENUS = {
    "pen": (223, 8, 34, 169, 147),
    "eraser": (223, 8, 100, 169, 98),
    "shape": (223, 8, 124, 169, 129),
    "close": (223, 77, 222, 124, 78),
    "color": (223, 15, 19, 165, 222),
}
SUBMENU_ROOM = 470   # sağda bu kadar yer yoksa alt menü sola açılır

SIZE_SELECTION = {1: (16, 113), 2: (32, 112), 3: (50, 110), 4: (70, 108),
                  5: (94, 106), 6: (121, 105)}

# ------------------------------------------------- Araçlar alt menüsü düzeni
TOOLS_COLS = 5
TOOLS_CELL_W, TOOLS_CELL_H = 64, 62
TOOLS_PAD = 12
TOOLS_ARROW = 12
TOOLS_ROWS = [
    ["stroke:normal", "stroke:dash", "stroke:dot", "stroke:arrow", "recognize"],
    ["select", "text", "laser", "vanish", "fill"],
    ["ruler", "setsquare", "protractor", "compass", "spotlight"],
    ["magnifier", "timer", "lots", "screenshot", "clear"],
    ["png", "pdf", "save", "open", "share"],
    ["page:prev", "page:label", "page:next", "page:new", "page:delete"],
]
SWATCH_ROW_H = 44
TOOLS_W = TOOLS_PAD * 2 + TOOLS_COLS * TOOLS_CELL_W + TOOLS_ARROW
SWATCH_TOP = 22       # 'Son renkler' başlığı için boşluk
TOOLS_H = TOOLS_PAD * 2 + len(TOOLS_ROWS) * TOOLS_CELL_H + SWATCH_TOP + 2 * SWATCH_ROW_H

TOOL_LABELS = {
    "stroke:normal": "Normal", "stroke:dash": "Kesikli", "stroke:dot": "Noktalı",
    "stroke:arrow": "Oklu", "recognize": "Tanıma", "select": "Seç", "text": "Metin",
    "laser": "Lazer", "vanish": "Kaybolan", "fill": "Dolgu", "ruler": "Cetvel",
    "setsquare": "Gönye", "protractor": "İletki", "compass": "Pergel",
    "spotlight": "Spot", "magnifier": "Büyüteç", "timer": "Sayaç", "lots": "Kura",
    "screenshot": "Ekran", "clear": "Temizle", "png": "PNG", "pdf": "PDF",
    "save": "Ders kaydet", "open": "Ders aç", "share": "Paylaş",
    "page:prev": "Önceki", "page:next": "Sonraki", "page:new": "Yeni sayfa",
    "page:delete": "Sayfa sil",
}

SUBMENUS["tools"] = (223, 8 - (TOOLS_W - 169), 20, TOOLS_W, TOOLS_H)


def expanded_height(n_favs):
    # İlk Fatih Kalem: 223 + 42 * iFav; 2.1: + Araçlar düğmesi + imza satırı
    return 223 + 42 + 42 * n_favs + BYLINE_HEIGHT


class Toolbar:
    def __init__(self, app):
        self.app = app          # KalemWindow (durum buradan okunur)
        self.x = 28.0           # cnvMainMenu sol üst köşesi (pencereye göre)
        self.y = 24.0
        self.pressed = None     # basılı tutulan öğe (görsel geri bildirim)

    # ------------------------------------------------------- dönüşümler --
    @property
    def s(self):
        return getattr(self.app, "menu_scale", 1.0)

    def to_window(self, lx, ly):
        return self.x + self.s * lx, self.y + self.s * ly

    def to_local(self, px, py):
        return (px - self.x) / self.s, (py - self.y) / self.s

    def border_pos(self):
        return self.to_window(BORDER_X, BORDER_Y)

    def set_border_pos(self, bx, by):
        self.x = bx - self.s * BORDER_X
        self.y = by - self.s * BORDER_Y

    # ------------------------------------------------------------ ölçüler --
    def n_favs(self):
        return len(self.app.favourites)

    def local_border_height(self):
        if self.app.minimized:
            return MINIMIZED_HEIGHT
        return expanded_height(self.n_favs())

    def border_height(self):
        """Pencere koordinatlarında kenarlık yüksekliği."""
        return self.s * self.local_border_height()

    def border_width(self):
        return self.s * BORDER_W

    def border_rect(self):
        bx, by = self.border_pos()
        return (bx, by, self.border_width(), self.border_height())

    def local_submenu_origin(self, name):
        right_x, left_x, y, w, h = SUBMENUS[name]
        if name == "close":
            y += 42 * self.n_favs()
        x = left_x if self.app.submenu_left else right_x
        if name == "tools":
            # Alt menü ekrandan taşmasın
            top_limit = -self.y / self.s
            bottom_limit = (self.app.H - self.y) / self.s - h
            y = min(max(y, top_limit), max(top_limit, bottom_limit))
        return x, y, w, h

    def submenu_origin(self, name):
        x, y, w, h = self.local_submenu_origin(name)
        wx, wy = self.to_window(x, y)
        return wx, wy, w * self.s, h * self.s

    def choose_side(self, window_width):
        """İlk sürüm: cnvMainMenu.Margin.Left + 470 > Width ise sola aç."""
        side = getattr(self.app, "submenu_side", "auto")
        if side == "left":
            return True
        if side == "right":
            return False
        room = SUBMENU_ROOM if self.app.shown_submenu != "tools" else BORDER_X + BORDER_W + TOOLS_W + 10
        return self.x + self.s * room > window_width

    def local_items(self):
        """Görünen düğmeler: (ad, x, y, w, h) yerel koordinatlarda."""
        out = [("titlebar", BTN_X, 12, 42, 30),
               ("handpen", BTN_X, 42, 42, 42)]
        if self.app.minimized:
            return out
        n = self.n_favs()
        out += [("pen", BTN_X, 84, 42, 42),
                ("eraser", BTN_X, 126, 42, 42),
                ("shape", BTN_X, 168, 42, 42),
                ("tools", BTN_X, TOOLS_Y, 42, 42)]
        for i in range(n):
            out.append(("fav:%d" % i, BTN_X, FIRST_FAV_Y + 42 * i, 42, 42))
        sy = FIRST_FAV_Y + 42 * n
        out += [("settings", BTN_X, sy, 21, 21),
                ("close", 200, sy, 21, 21)]
        return out

    def items(self):
        """Pencere koordinatlarında düğmeler."""
        s = self.s
        return [(name, self.x + s * x, self.y + s * y, s * w, s * h)
                for name, x, y, w, h in self.local_items()]

    def bounds(self):
        """Menünün (alt menü dahil) kapladığı alan; tıklama-geçirgenliği için."""
        bx, by, bw, bh = self.border_rect()
        m = 12 * self.s
        rects = [(bx - m, by - m, bw + 2 * m, bh + 2 * m)]
        if self.app.shown_submenu:
            sx, sy, sw, sh = self.submenu_origin(self.app.shown_submenu)
            rects.append((sx - 4, sy - 4, sw + 8, sh + 8))
        return rects

    def clamp(self, width, height):
        bx, by = self.border_pos()
        bw, bh = self.border_width(), self.border_height()
        bx = min(max(bx, 0), max(0, width - bw))
        by = min(max(by, 0), max(0, height - bh))
        self.set_border_pos(bx, by)

    # -------------------------------------------------------------- çizim --
    def draw(self, cr):
        cr.save()
        cr.translate(self.x, self.y)
        cr.scale(self.s, self.s)
        self._draw_local(cr)
        cr.restore()

    def _draw_local(self, cr):
        app = self.app
        bx, by, bw, bh = BORDER_X, BORDER_Y, BORDER_W, self.local_border_height()

        # Gölge (DropShadowEffect: blur 10, opaklık 0.8). Saydamlık yoksa
        # gölge düz siyah leke olarak görüneceğinden çizilmez.
        if app.composited:
            for i in range(8, 0, -1):
                cr.set_source_rgba(0, 0, 0, 0.035 * (9 - i) / 2.0)
                _rounded(cr, bx - i, by - i, bw + 2 * i, bh + 2 * i, 2 + i)
                cr.fill()
        cr.set_source_rgb(1, 1, 1)
        cr.rectangle(bx, by, bw, bh)
        cr.fill()
        cr.set_source_rgb(*ORANGE)
        cr.set_line_width(2)
        cr.rectangle(bx + 1, by + 1, bw - 2, bh - 2)
        cr.stroke()

        for name, x, y, w, h in self.local_items():
            alpha = 0.6 if self.pressed == name else 1.0
            if name == "titlebar":
                res.paint(cr, "titlebar", x, y, alpha)
            elif name == "handpen":
                res.paint(cr, "pen" if app.minimized else "hand", x, y, alpha)
            elif name == "pen":
                res.paint(cr, app.pen_button_image(), x, y, alpha)
            elif name == "eraser":
                res.paint(cr, "eraser", x, y, alpha)
            elif name == "shape":
                res.paint(cr, "shapes", x, y, alpha)
            elif name == "tools":
                icons.draw_button(cr, "toolbox", x, y, 42, alpha)
            elif name.startswith("fav:"):
                tag = app.favourites[int(name[4:])]
                if tag.startswith("Tool:"):
                    icons.draw_button(cr, tag[5:], x, y, 42, alpha)
                else:
                    res.paint(cr, app.fav_image(tag), x, y, alpha)
            elif name == "settings":
                res.paint(cr, "settings", x, y, alpha)
            elif name == "close":
                res.paint(cr, "close", x, y, alpha)

        if not app.minimized:
            tick = app.tick_target()
            ticks = {"pen": (204, 113), "eraser": (204, 154), "shape": (204, 193.7),
                     "tools": (204, TOOLS_Y + 25.7)}
            if tick in ticks:
                tx, ty = ticks[tick]
                res.paint(cr, "tick", tx, ty, w=16, h=20)
            self._draw_byline(cr, bx, by, bw, bh)

        if app.shown_submenu:
            self._draw_submenu(cr, app.shown_submenu)

    def _draw_byline(self, cr, bx, by, bw, bh):
        cr.save()
        cr.select_font_face("Sans", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_BOLD)
        cr.set_font_size(8.0)
        ext = cr.text_extents(BYLINE_TEXT)
        room = bw - 6.0          # 2 px kenarlık + 1 px boşluk (iki yanda)
        if ext.width > room:     # yazı tipi geniş ise sığacak kadar küçült
            cr.set_font_size(8.0 * room / ext.width)
            ext = cr.text_extents(BYLINE_TEXT)
        tx = bx + (bw - ext.width) / 2.0 - ext.x_bearing
        ty = by + bh - BYLINE_HEIGHT / 2.0 - 1 - (ext.height / 2.0 + ext.y_bearing)
        cr.set_source_rgb(110 / 255.0, 110 / 255.0, 110 / 255.0)
        cr.move_to(tx, ty)
        cr.show_text(BYLINE_TEXT)
        cr.restore()

    def _draw_submenu(self, cr, name):
        app = self.app
        x, y, w, h = self.local_submenu_origin(name)
        side = "left" if app.submenu_left else "right"
        if name == "pen":
            tab = {MARKER: "marker", STYLOGRAPH: "stylograph",
                   HIGHLIGHTER: "highlighter"}[app.active_tab]
            res.paint(cr, "submenu%s%s" % (tab, side), x, y)
            if app.active_tab == app.pen_style:
                sx, sy = SIZE_SELECTION[app.ink_size]
                res.paint(cr, "size%dselection" % app.ink_size, x + sx, y + sy)
        elif name == "eraser":
            res.paint(cr, "submenueraser" + side, x, y)
            if not app.history.can_undo():
                res.paint(cr, "grayedundo", x + 20, y + 14)
            if not app.history.can_redo():
                res.paint(cr, "grayedredo", x + 65, y + 14)
        elif name == "shape":
            res.paint(cr, "submenushape" + side, x, y)
        elif name == "close":
            res.paint(cr, "submenuclose" + side, x, y)
            cr.save()
            cr.select_font_face("Sans", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_NORMAL)
            cr.set_font_size(15)
            label = _("Kapat")
            ext = cr.text_extents(label)
            lx, ly, lw, lh = x + 30, y + 27, 63, 24
            cr.set_source_rgb(0x52 / 255.0, 0x52 / 255.0, 0x52 / 255.0)
            cr.move_to(lx + (lw - ext.width) / 2.0 - ext.x_bearing,
                       ly + (lh - ext.height) / 2.0 - ext.y_bearing)
            cr.show_text(label)
            cr.restore()
        elif name == "color":
            res.paint(cr, "colorselector", x, y)
        elif name == "tools":
            self._draw_tools(cr, x, y, w, h, side)

    # ------------------------------------------------- Araçlar alt menüsü --
    def tools_cells(self):
        """(ad, x, y, w, h) alt menünün sol üstüne göre yerel koordinatlar."""
        left = TOOLS_PAD + (0 if self.app.submenu_left else TOOLS_ARROW)
        out = []
        for r, row in enumerate(TOOLS_ROWS):
            for c, name in enumerate(row):
                out.append((name, left + c * TOOLS_CELL_W, TOOLS_PAD + r * TOOLS_CELL_H,
                            TOOLS_CELL_W, TOOLS_CELL_H))
        top = TOOLS_PAD + len(TOOLS_ROWS) * TOOLS_CELL_H + SWATCH_TOP
        sw = TOOLS_CELL_W
        recent = self.app.settings["RecentColors"]
        for i in range(5):
            out.append(("recent:%d" % i, left + i * sw, top, sw, SWATCH_ROW_H))
        top += SWATCH_ROW_H
        out.append(("palette:add", left, top, sw, SWATCH_ROW_H))
        for i in range(4):
            out.append(("palette:%d" % i, left + (i + 1) * sw, top, sw, SWATCH_ROW_H))
        del recent
        return out

    def tools_hit(self, lx, ly):
        for name, x, y, w, h in self.tools_cells():
            if x <= lx < x + w and y <= ly < y + h:
                return name
        return None

    def _draw_tools(self, cr, x, y, w, h, side):
        app = self.app
        cr.save()
        cr.translate(x, y)
        # Baloncuk (ilk sürümdeki alt menülerle aynı görünüm)
        left = 0 if app.submenu_left else TOOLS_ARROW
        bw = w - TOOLS_ARROW
        _rounded(cr, left + 1, 1, bw - 2, h - 2, 12)
        cr.set_source_rgb(1, 1, 1)
        cr.fill_preserve()
        cr.set_source_rgb(*ORANGE)
        cr.set_line_width(2)
        cr.stroke()
        # ok ucu ana menüye bakar
        ay = (TOOLS_Y + 21) - self.local_submenu_origin("tools")[1]
        ay = min(max(ay, 24), h - 24)
        cr.set_source_rgb(1, 1, 1)
        if app.submenu_left:
            cr.move_to(w - TOOLS_ARROW - 2, ay - 9)
            cr.line_to(w - 1, ay)
            cr.line_to(w - TOOLS_ARROW - 2, ay + 9)
        else:
            cr.move_to(TOOLS_ARROW + 2, ay - 9)
            cr.line_to(1, ay)
            cr.line_to(TOOLS_ARROW + 2, ay + 9)
        cr.fill_preserve()
        cr.set_source_rgb(*ORANGE)
        cr.stroke()

        active = app.active_tools()
        cr.select_font_face("Sans", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_NORMAL)
        for name, cx, cy, cw, ch in self.tools_cells():
            if name.startswith("recent:") or name.startswith("palette:"):
                self._draw_swatch(cr, name, cx, cy, cw, ch)
                continue
            if name == "page:label":
                cr.set_font_size(13)
                label = _("Sayfa %d / %d") % (app.book.index + 1, len(app.book))
                cr.set_source_rgb(0.25, 0.25, 0.3)
                ext = cr.text_extents(label)
                if ext.width > cw - 4:
                    label = "%d / %d" % (app.book.index + 1, len(app.book))
                    ext = cr.text_extents(label)
                cr.move_to(cx + (cw - ext.width) / 2.0 - ext.x_bearing, cy + ch / 2.0 + 5)
                cr.show_text(label)
                continue
            if name in active:
                _rounded(cr, cx + 3, cy + 2, cw - 6, ch - 4, 8)
                cr.set_source_rgba(0.12, 0.56, 1.0, 0.16)
                cr.fill()
            if self.pressed == "tools:" + name:
                _rounded(cr, cx + 3, cy + 2, cw - 6, ch - 4, 8)
                cr.set_source_rgba(0.94, 0.35, 0.16, 0.2)
                cr.fill()
            icons.draw_icon(cr, name, cx + cw / 2.0, cy + 22, 34)
            cr.set_font_size(10.5)
            label = _(TOOL_LABELS.get(name, name))
            ext = cr.text_extents(label)
            if ext.width > cw - 4:
                cr.set_font_size(10.5 * (cw - 4) / ext.width)
                ext = cr.text_extents(label)
            cr.set_source_rgb(0.32, 0.32, 0.32)
            cr.move_to(cx + (cw - ext.width) / 2.0 - ext.x_bearing, cy + ch - 8)
            cr.show_text(label)
            cr.new_path()
        # bölüm başlıkları (renk satırları)
        cr.set_font_size(10)
        cr.set_source_rgb(0.45, 0.45, 0.5)
        top = TOOLS_PAD + len(TOOLS_ROWS) * TOOLS_CELL_H + SWATCH_TOP - 4
        lx = TOOLS_PAD + (0 if app.submenu_left else TOOLS_ARROW)
        cr.move_to(lx + 6, top)
        cr.show_text(_("Son renkler"))
        cr.new_path()
        cr.restore()

    def _draw_swatch(self, cr, name, x, y, w, h):
        app = self.app
        from .config import hex_to_rgb
        cx, cy, r = x + w / 2.0, y + h / 2.0 + 4, 14
        if name == "palette:add":
            cr.arc(cx, cy, r, 0, 2 * math.pi)
            cr.set_source_rgb(*app.ink_rgb)
            cr.fill_preserve()
            cr.set_source_rgb(0.4, 0.4, 0.45)
            cr.set_line_width(1.2)
            cr.stroke()
            cr.set_source_rgb(1, 1, 1)
            cr.set_line_width(3)
            cr.move_to(cx - 6, cy)
            cr.line_to(cx + 6, cy)
            cr.move_to(cx, cy - 6)
            cr.line_to(cx, cy + 6)
            cr.stroke()
            return
        kind, idx = name.split(":")
        lst = app.settings["RecentColors"] if kind == "recent" else app.settings["CustomPalette"]
        idx = int(idx)
        if idx >= len(lst):
            cr.arc(cx, cy, r, 0, 2 * math.pi)
            cr.set_source_rgba(0.6, 0.6, 0.65, 0.5)
            cr.set_line_width(1)
            cr.set_dash([3, 3])
            cr.stroke()
            cr.set_dash([])
            return
        cr.arc(cx, cy, r, 0, 2 * math.pi)
        cr.set_source_rgb(*hex_to_rgb(lst[idx]))
        cr.fill_preserve()
        cr.set_source_rgb(0.4, 0.4, 0.45)
        cr.set_line_width(1.2)
        cr.stroke()

    # ------------------------------------------------------- tıklama testi --
    def hit_button(self, px, py):
        lx, ly = self.to_local(px, py)
        for name, x, y, w, h in self.local_items():
            if x <= lx < x + w and y <= ly < y + h:
                return name
        if BORDER_X <= lx < BORDER_X + BORDER_W and BORDER_Y <= ly < BORDER_Y + self.local_border_height():
            return "border"
        return None

    def hit_submenu(self, px, py):
        """(alt menü adı, alt menü içindeki yerel x, y) ya da None."""
        name = self.app.shown_submenu
        if not name:
            return None
        lx, ly = self.to_local(px, py)
        x, y, w, h = self.local_submenu_origin(name)
        if x <= lx < x + w and y <= ly < y + h:
            return name, lx - x, ly - y
        return None

    def contains(self, px, py):
        for x, y, w, h in self.bounds():
            if x <= px < x + w and y <= py < y + h:
                return True
        return False


def _rounded(cr, x, y, w, h, r):
    r = min(r, w / 2.0, h / 2.0)
    cr.new_sub_path()
    cr.arc(x + w - r, y + r, r, -math.pi / 2, 0)
    cr.arc(x + w - r, y + h - r, r, 0, math.pi / 2)
    cr.arc(x + r, y + h - r, r, math.pi / 2, math.pi)
    cr.arc(x + r, y + r, r, math.pi, 1.5 * math.pi)
    cr.close_path()


# --------------------------------------------- alt menü bölge eşlemeleri --

def pen_submenu_action(x, y):
    """SubMenuPen_MouseLeftButtonUp eşiklerinin aynısı."""
    if y <= 10:
        return None
    if y <= 32:
        if x <= 16:
            return None
        if x <= 65:
            return ("tab", MARKER)
        if x <= 106:
            return ("tab", STYLOGRAPH)
        if x <= 152:
            return ("tab", HIGHLIGHTER)
        return None
    if y <= 69:
        for limit, c in ((16, None), (50, 1), (84, 2), (118, 3), (152, 4)):
            if x <= limit:
                return ("color", c) if c else None
        return None
    if y <= 105:
        if x <= 16:
            return None
        if x <= 50:
            return ("color", 5)
        if x <= 84:
            return ("color", 6)
        if x <= 118:
            return ("colorful", None)
        if x <= 152:
            return ("nopen", None)
        return None
    if y <= 137:
        for limit, s in ((16, None), (31, 1), (50, 2), (70, 3), (94, 4), (121, 5), (151, 6)):
            if x <= limit:
                return ("size", s) if s else None
    return None


def eraser_submenu_action(x, y):
    if y <= 12:
        return None
    if y <= 48:
        if x <= 16:
            return None
        if x <= 65:
            return ("undo", None)
        if x <= 110:
            return ("redo", None)
        if x <= 153:
            return ("curtain", None)
        return None
    if y <= 86:
        for limit, e in ((16, None), (47, 1), (76, 2), (111, 3), (153, 4)):
            if x <= limit:
                return ("eraser", e) if e else None
    return None


def shape_submenu_action(x, y):
    from .styles import ARROW, DASHLINE, ELLIPSE, LINE, RECTANGLE, TRIANGLE
    if y <= 12:
        return None
    if y <= 43:
        row = (LINE, DASHLINE, ARROW)
    elif y <= 79:
        row = (RECTANGLE, ELLIPSE, TRIANGLE)
    elif y <= 117:
        if x <= 16:
            return None
        if x <= 84:
            return ("library", None)
        if x <= 153:
            return ("background", None)
        return None
    else:
        return None
    for limit, s in ((16, None), (65, row[0]), (106, row[1]), (153, row[2])):
        if x <= limit:
            return ("shape", s) if s is not None else None
    return None


def color_selector_action(x, y):
    from .styles import CARTELA
    if y <= 16 or x <= 23 or x > 143:
        return None
    rows = (51, 81, 111, 141, 171, 205)
    for r, limit in enumerate(rows):
        if y <= limit:
            c = min(3, int((x - 23 - 1e-6) // 30))
            return ("cartela", CARTELA[r][c])
    return None


def close_submenu_action(x, y):
    if 16 <= x <= 120 and 10 <= y <= 68:
        return ("quit", None)
    return None


SUBMENU_ACTIONS = {
    "pen": pen_submenu_action,
    "eraser": eraser_submenu_action,
    "shape": shape_submenu_action,
    "color": color_selector_action,
    "close": close_submenu_action,
}


def tools_submenu_action(toolbar, x, y):
    name = toolbar.tools_hit(x, y)
    if name is None or name == "page:label":
        return None
    return (name, None)


# Sık kullanılan etiketi (orijinal Fav etiketleri) -> alt menü eylemi
def favourite_tag_for(submenu, action, pen_style):
    """Alt menüde uzun basılan öğenin sık kullanılan etiketi."""
    from .styles import (ARROW, COLOR_NAMES, DASHLINE, ELLIPSE, LINE,
                         RECTANGLE, STYLE_NAMES, TRIANGLE)
    kind, val = action
    if submenu == "tools":
        if kind in ("recent", "palette", "page:label") or kind.startswith("recent:")                 or kind.startswith("palette:") or kind == "page:label":
            return None
        return "Tool:" + kind
    style = STYLE_NAMES[pen_style]
    if submenu == "pen":
        if kind == "color":
            name = COLOR_NAMES[val]
            if pen_style == HIGHLIGHTER and val == 4:
                name = "Yellow"
            return style + name
        if kind == "colorful":
            return style + "ColorFul"
        if kind == "nopen":
            return style + "NoPen"
        if kind == "size":
            return "Size%d" % val
        return None
    if submenu == "eraser":
        if kind == "eraser":
            return "Eraser%d" % val
        return {"undo": "Undo", "redo": "Redo", "curtain": "Curtain"}.get(kind)
    if submenu == "shape":
        if kind == "shape":
            return {LINE: "Line", DASHLINE: "DashLine", ARROW: "Arrow",
                    RECTANGLE: "Rectangle", ELLIPSE: "Ellipse",
                    TRIANGLE: "Triangle"}[val]
        return {"library": "Library", "background": "Background"}.get(kind)
    return None
