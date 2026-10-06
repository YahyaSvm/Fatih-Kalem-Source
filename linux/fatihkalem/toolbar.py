"""Ana menü (araç çubuğu) ve alt menülerin yerleşimi, çizimi ve tıklama
bölgeleri.

Tüm koordinatlar orijinal XAML'deki (cnvMainMenu içindeki) değerlerdir ve
görseller orijinal programdan alınmıştır; bu sayede menü Windows sürümüyle
birebir aynı görünür ve aynı yerlere dokunulduğunda aynı işi yapar.
"""

import math

import cairo

from . import resources as res
from .styles import HIGHLIGHTER, MARKER, STYLOGRAPH

BORDER_X, BORDER_Y, BORDER_W = 177, 10, 46
BTN_X = 179
MINIMIZED_HEIGHT = 76
BYLINE_HEIGHT = 16
BYLINE_TEXT = "By YhySvm"
ORANGE = (0xF0 / 255.0, 0x5A / 255.0, 0x28 / 255.0)
FRAME_BLUE = (0x1E / 255.0, 0x90 / 255.0, 0xFF / 255.0)

# Alt menü adı -> (sağ x, sol x, y, görsel genişliği, yüksekliği)
SUBMENUS = {
    "pen": (223, 8, 34, 169, 147),
    "eraser": (223, 8, 100, 169, 98),
    "shape": (223, 8, 124, 169, 129),
    "close": (223, 77, 180, 124, 78),
    "color": (223, 15, 19, 165, 222),
}
SUBMENU_ROOM = 470   # sağda bu kadar yer yoksa alt menü sola açılır

SIZE_SELECTION = {1: (16, 113), 2: (32, 112), 3: (50, 110), 4: (70, 108),
                  5: (94, 106), 6: (121, 105)}


def expanded_height(n_favs):
    return 223 + 42 * n_favs + BYLINE_HEIGHT


class Toolbar:
    def __init__(self, app):
        self.app = app          # KalemWindow (durum buradan okunur)
        self.x = 28.0           # cnvMainMenu sol üst köşesi (pencereye göre)
        self.y = 24.0
        self.pressed = None     # basılı tutulan öğe (görsel geri bildirim)

    # ------------------------------------------------------------ ölçüler --
    def n_favs(self):
        return len(self.app.favourites)

    def border_height(self):
        if self.app.minimized:
            return MINIMIZED_HEIGHT
        return expanded_height(self.n_favs())

    def border_rect(self):
        """Pencere koordinatlarında (x, y, w, h)."""
        return (self.x + BORDER_X, self.y + BORDER_Y, BORDER_W, self.border_height())

    def submenu_origin(self, name):
        right_x, left_x, y, w, h = SUBMENUS[name]
        if name == "close":
            y += 42 * self.n_favs()
        x = left_x if self.app.submenu_left else right_x
        return self.x + x, self.y + y, w, h

    def choose_side(self, window_width):
        """Orijinal: cnvMainMenu.Margin.Left + 470 > Width ise sola aç."""
        return self.x + SUBMENU_ROOM > window_width

    def items(self):
        """Görünen düğmeler: (ad, x, y, w, h) pencere koordinatlarında."""
        ox, oy = self.x, self.y
        out = [("titlebar", ox + BTN_X, oy + 12, 42, 30),
               ("handpen", ox + BTN_X, oy + 42, 42, 42)]
        if self.app.minimized:
            return out
        n = self.n_favs()
        out += [("pen", ox + BTN_X, oy + 84, 42, 42),
                ("eraser", ox + BTN_X, oy + 126, 42, 42),
                ("shape", ox + BTN_X, oy + 168, 42, 42)]
        for i in range(n):
            out.append(("fav:%d" % i, ox + BTN_X, oy + 168 + 42 * (i + 1), 42, 42))
        out += [("settings", ox + BTN_X, oy + 210 + 42 * n, 21, 21),
                ("close", ox + 200, oy + 210 + 42 * n, 21, 21)]
        return out

    def bounds(self):
        """Menünün (alt menü dahil) kapladığı alan; tıklama-geçirgenliği için."""
        bx, by, bw, bh = self.border_rect()
        rects = [(bx - 12, by - 12, bw + 24, bh + 24)]
        if self.app.shown_submenu:
            sx, sy, sw, sh = self.submenu_origin(self.app.shown_submenu)
            rects.append((sx - 4, sy - 4, sw + 8, sh + 8))
        return rects

    def clamp(self, width, height):
        bx_min = -BORDER_X
        bx_max = width - BORDER_X - BORDER_W
        by_min = -BORDER_Y
        by_max = height - BORDER_Y - self.border_height()
        self.x = min(max(self.x, bx_min), max(bx_min, bx_max))
        self.y = min(max(self.y, by_min), max(by_min, by_max))

    # -------------------------------------------------------------- çizim --
    def draw(self, cr):
        app = self.app
        bx, by, bw, bh = self.border_rect()

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

        for name, x, y, w, h in self.items():
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
            elif name.startswith("fav:"):
                tag = app.favourites[int(name[4:])]
                res.paint(cr, app.fav_image(tag), x, y, alpha)
            elif name == "settings":
                res.paint(cr, "settings", x, y, alpha)
            elif name == "close":
                res.paint(cr, "close", x, y, alpha)

        if not app.minimized:
            tick = app.tick_target()
            ticks = {"pen": (204, 113), "eraser": (204, 154), "shape": (204, 193.7)}
            if tick in ticks:
                tx, ty = ticks[tick]
                res.paint(cr, "tick", self.x + tx, self.y + ty, w=16, h=20)
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
        x, y, w, h = self.submenu_origin(name)
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
            ext = cr.text_extents("Kapat")
            lx, ly, lw, lh = x + 30, y + 27, 63, 24
            cr.set_source_rgb(0x52 / 255.0, 0x52 / 255.0, 0x52 / 255.0)
            cr.move_to(lx + (lw - ext.width) / 2.0 - ext.x_bearing,
                       ly + (lh - ext.height) / 2.0 - ext.y_bearing)
            cr.show_text("Kapat")
            cr.restore()
        elif name == "color":
            res.paint(cr, "colorselector", x, y)

    # ------------------------------------------------------- tıklama testi --
    def hit_button(self, px, py):
        for name, x, y, w, h in self.items():
            if x <= px < x + w and y <= py < y + h:
                return name
        bx, by, bw, bh = self.border_rect()
        if bx <= px < bx + bw and by <= py < by + bh:
            return "border"
        return None

    def hit_submenu(self, px, py):
        """(alt menü adı, yerel x, yerel y) ya da None."""
        name = self.app.shown_submenu
        if not name:
            return None
        x, y, w, h = self.submenu_origin(name)
        if x <= px < x + w and y <= py < y + h:
            return name, px - x, py - y
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


# Sık kullanılan etiketi (orijinal Fav etiketleri) -> alt menü eylemi
def favourite_tag_for(submenu, action, pen_style):
    """Alt menüde uzun basılan öğenin sık kullanılan etiketi."""
    from .styles import (ARROW, COLOR_NAMES, DASHLINE, ELLIPSE, LINE,
                         RECTANGLE, STYLE_NAMES, TRIANGLE)
    kind, val = action
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
