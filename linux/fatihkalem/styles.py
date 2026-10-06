"""Kalem tipleri, renkler, kalınlıklar ve şekil geometrisi.

Değerler orijinal programdaki ChangeColor / ChangeInkSize / ChangePenImage /
DrawArrow / DrawDashLine metotlarından birebir alınmıştır.
"""

import math

from .ink import TIP_ELLIPSE, TIP_RECT, Stroke, densify

# PenStyleEnum
MARKER, STYLOGRAPH, HIGHLIGHTER = 0, 1, 2

# DrawStateEnum
PEN, ERASER, LINE, DASHLINE, ARROW, RECTANGLE, ELLIPSE, TRIANGLE, NOPEN, CURTAIN = (
    0, 1, 3, 4, 5, 6, 7, 8, 9, 10)
SHAPES = (LINE, DASHLINE, ARROW, RECTANGLE, ELLIPSE, TRIANGLE)
# 2.1 ile gelen araç kipleri
SELECT, TEXT, LASER, VANISH, FILL = 11, 12, 13, 14, 15
TOOL_STATES = (SELECT, TEXT, LASER, VANISH, FILL)

STYLE_NAMES = {MARKER: "Marker", STYLOGRAPH: "Stylograph", HIGHLIGHTER: "Highlighter"}
STYLE_TITLES = {MARKER: "Keçeli Kalem", STYLOGRAPH: "Dolma Kalem",
                HIGHLIGHTER: "Fosforlu Kalem"}

# ColorNo: 1 kırmızı, 2 mavi, 3 yeşil, 4 turuncu/sarı, 5 siyah, 6 beyaz,
# 7 renk kartelasından seçilen.
COLOR_NAMES = {1: "Red", 2: "Blue", 3: "Green", 4: "Orange", 5: "Black",
               6: "White", 7: "Colorful"}
COLOR_TITLES = {1: "Kırmızı", 2: "Mavi", 3: "Yeşil", 4: "Turuncu", 5: "Siyah",
                6: "Beyaz", 7: "Özel"}


def _rgb(r, g, b):
    return (r / 255.0, g / 255.0, b / 255.0)


PALETTE = {
    MARKER: {1: _rgb(255, 0, 0), 2: _rgb(0, 0, 255), 3: _rgb(0, 110, 0),
             4: _rgb(255, 115, 0), 5: _rgb(0, 0, 0), 6: _rgb(255, 255, 255)},
    STYLOGRAPH: {1: _rgb(230, 27, 27), 2: _rgb(0, 0, 157), 3: _rgb(0, 110, 0),
                 4: _rgb(255, 152, 0), 5: _rgb(20, 20, 20), 6: _rgb(255, 255, 255)},
    HIGHLIGHTER: {1: _rgb(255, 0, 255), 2: _rgb(1, 255, 255), 3: _rgb(0, 255, 1),
                  4: _rgb(255, 255, 1), 5: _rgb(0, 0, 0), 6: _rgb(255, 255, 255)},
}

# Renk körlüğüne uygun palet (Okabe-Ito); renk numaraları aynı kalır.
PALETTE_CB = {
    MARKER: {1: _rgb(213, 94, 0), 2: _rgb(0, 114, 178), 3: _rgb(0, 158, 115),
             4: _rgb(230, 159, 0), 5: _rgb(0, 0, 0), 6: _rgb(255, 255, 255)},
    STYLOGRAPH: {1: _rgb(213, 94, 0), 2: _rgb(0, 114, 178), 3: _rgb(0, 158, 115),
                 4: _rgb(230, 159, 0), 5: _rgb(20, 20, 20), 6: _rgb(255, 255, 255)},
    HIGHLIGHTER: {1: _rgb(204, 121, 167), 2: _rgb(86, 180, 233), 3: _rgb(0, 158, 115),
                  4: _rgb(240, 228, 66), 5: _rgb(0, 0, 0), 6: _rgb(255, 255, 255)},
}
_use_cb = False


def set_colorblind(enabled):
    global _use_cb
    _use_cb = bool(enabled)


# InkSize 1..6 -> (genişlik, yükseklik)
INK_SIZES = {
    MARKER: {1: (1, 1), 2: (2, 2), 3: (3, 3), 4: (5, 5), 5: (10, 10), 6: (22, 22)},
    HIGHLIGHTER: {1: (5, 10), 2: (8, 16), 3: (11, 22), 4: (14, 28), 5: (20, 40),
                  6: (30, 60)},
    STYLOGRAPH: {1: (0.75, 2.25), 2: (1.25, 3.75), 3: (1.75, 6), 4: (2.25, 9),
                 5: (5, 20), 6: (8, 35)},
}
SHAPE_SIZES = {1: 1, 2: 2, 3: 3, 4: 5, 5: 10, 6: 22}

# Renk kartelası (ColorSelector.png, 4 sütun x 6 satır)
CARTELA = [
    [(94, 124, 139), (136, 196, 64), (28, 230, 0), (246, 64, 44)],
    [(157, 157, 157), (204, 221, 30), (70, 175, 74), (235, 20, 96)],
    [(88, 89, 91), (255, 236, 22), (0, 150, 135), (156, 26, 177)],
    [(122, 85, 71), (255, 193, 0), (0, 187, 213), (96, 0, 128)],
    [(142, 86, 46), (255, 152, 0), (16, 147, 245), (102, 51, 185)],
    [(255, 85, 5), (230, 27, 27), (0, 77, 230), (61, 77, 183)],
]

# Silgi boyutları (SubMenuEraser): genişlik x yükseklik
ERASER_SIZES = {1: (20, 32), 2: (50, 83), 3: (100, 160), 4: (180, 288)}
GESTURE_ERASER = (62, 103)


def pen_attributes(style, ink_size):
    """(genişlik, yükseklik, uç, döndürme, fosforlu)"""
    w, h = INK_SIZES[style][ink_size]
    if style == HIGHLIGHTER:
        return w, h, TIP_RECT, 0.0, True
    if style == STYLOGRAPH:
        return w, h, TIP_ELLIPSE, 45.0, False
    return w, h, TIP_ELLIPSE, 0.0, False


def color_for(style, color_no, custom_rgb):
    if color_no == 7 and custom_rgb is not None:
        return custom_rgb
    pal = PALETTE_CB if _use_cb else PALETTE
    return pal[style].get(color_no, pal[style][2])


def pen_image_name(style, color_no, nopen=False):
    """Ana menüdeki kalem düğmesinin görseli (ChangePenImage)."""
    if nopen:
        return STYLE_NAMES[style] + "NoPen"
    if style == HIGHLIGHTER and color_no == 4:
        return "HighlighterOrange"
    return STYLE_NAMES[style] + COLOR_NAMES.get(color_no, "Blue")


# ------------------------------------------------------------- şekiller ----

def _shape_stroke(points, color, size, tip=TIP_ELLIPSE):
    pts = [(x, y, 0.5) for x, y in points]
    return Stroke(densify(pts), color, size, size, tip)


def line(p0, p1, color, size):
    return [_shape_stroke([p0, p1], color, size)]


def arrow(p0, p1, color, size):
    """DrawArrow: ok başı açısı ve uzunluğu kalınlığa göre değişir."""
    head = {1: (25, 14), 2: (23, 17), 3: (24, 20), 5: (25, 30), 10: (30, 40),
            22: (35, 70)}
    ang, length = head.get(size, (25, 30))
    x0, y0 = p0
    x1, y1 = p1
    theta = math.atan2(y1 - y0, x1 - x0)
    pts = [p0, p1]
    for sign in (1, -1):
        a = theta + math.pi - sign * math.radians(ang)
        pts.append((x1 + length * math.cos(a), y1 + length * math.sin(a)))
        pts.append(p1)
    return [_shape_stroke(pts, color, size)]


def dash_line(p0, p1, color, size):
    """DrawDashLine: çizgi kalınlığına göre değişen uzunlukta parçalar."""
    dash = {1: 5, 2: 10, 3: 15, 5: 20, 10: 25, 22: 35}.get(size, 15)
    x0, y0 = p0
    x1, y1 = p1
    length = math.hypot(x1 - x0, y1 - y0)
    if length < 1:
        return []
    ux, uy = (x1 - x0) / length, (y1 - y0) / length
    out = []
    n = int(round(length / dash / 2.0))
    for i in range(n):
        a = 2 * i * dash
        b = min(length, a + dash)
        out.append(_shape_stroke([(x0 + ux * a, y0 + uy * a),
                                  (x0 + ux * b, y0 + uy * b)], color, size))
    return out


def rectangle(p0, p1, color, size):
    x0, y0 = p0
    x1, y1 = p1
    pts = [(x0, y0), (x1, y0), (x1, y1), (x0, y1), (x0, y0)]
    return [_shape_stroke(pts, color, size, TIP_RECT)]


def ellipse(p0, p1, color, size):
    cx, cy = (p0[0] + p1[0]) / 2.0, (p0[1] + p1[1]) / 2.0
    rx, ry = abs(p1[0] - p0[0]) / 2.0, abs(p1[1] - p0[1]) / 2.0
    n = max(24, int((rx + ry) / 2))
    pts = [(cx + rx * math.cos(2 * math.pi * i / n),
            cy + ry * math.sin(2 * math.pi * i / n)) for i in range(n + 1)]
    return [_shape_stroke(pts, color, size)]


def triangle(p0, p1, p2, color, size):
    if p2 is None:
        return [_shape_stroke([p0, p1], color, size)]
    return [_shape_stroke([p0, p1, p2, p0], color, size)]


SHAPE_BUILDERS = {LINE: line, DASHLINE: dash_line, ARROW: arrow,
                  RECTANGLE: rectangle, ELLIPSE: ellipse}
