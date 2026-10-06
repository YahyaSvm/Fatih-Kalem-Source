"""Araçlar menüsünün vektör simgeleri (ek görsel dosyası gerektirmez)."""

import math

import cairo

DARK = (0.22, 0.24, 0.28)
ORANGE = (0.94, 0.35, 0.16)
BLUE = (0.12, 0.47, 0.85)
GREEN = (0.1, 0.6, 0.35)
RED = (0.85, 0.15, 0.15)
YELLOW = (1.0, 0.85, 0.1)


def _line(cr, pts, width=2.2, color=DARK, dash=None):
    cr.set_source_rgb(*color)
    cr.set_line_width(width)
    cr.set_line_cap(cairo.LINE_CAP_ROUND)
    cr.set_line_join(cairo.LINE_JOIN_ROUND)
    if dash:
        cr.set_dash(dash)
    cr.move_to(*pts[0])
    for p in pts[1:]:
        cr.line_to(*p)
    cr.stroke()
    cr.set_dash([])


def _wave(cr, color=DARK, dash=None, width=2.6):
    pts = [(-13 + i, 4 * math.sin(i / 4.0)) for i in range(0, 27)]
    _line(cr, pts, width, color, dash)


def _arrowhead(cr, x, y, ang, size=6, color=DARK):
    _line(cr, [(x + size * math.cos(ang + 2.6), y + size * math.sin(ang + 2.6)), (x, y),
               (x + size * math.cos(ang - 2.6), y + size * math.sin(ang - 2.6))], 2.2, color)


def _ruler_shape(cr, w=28, h=9):
    cr.rectangle(-w / 2, -h / 2, w, h)
    cr.set_source_rgb(0.85, 0.93, 1.0)
    cr.fill_preserve()
    cr.set_source_rgb(*BLUE)
    cr.set_line_width(1.5)
    cr.stroke()
    for i in range(7):
        x = -w / 2 + 3 + i * 3.7
        cr.move_to(x, -h / 2)
        cr.line_to(x, -h / 2 + (4 if i % 2 == 0 else 2.5))
    cr.set_line_width(1)
    cr.stroke()


def _draw(cr, name):
    """(0, 0) merkezli, yaklaşık 30x30 alanda çizer."""
    if name == "stroke:normal":
        _wave(cr)
    elif name == "stroke:dash":
        _wave(cr, dash=[5, 4])
    elif name == "stroke:dot":
        _wave(cr, dash=[0.1, 5], width=3.2)
    elif name == "stroke:arrow":
        _wave(cr)
        _arrowhead(cr, 13, 4 * math.sin(26 / 4.0), 0.35, 7)
    elif name == "recognize":
        _line(cr, [(-13, 6), (-9, -5), (-4, 3), (0, -6)], 1.6, (0.6, 0.6, 0.65))
        cr.arc(7, 0, 7, 0, 2 * math.pi)
        cr.set_source_rgb(*BLUE)
        cr.set_line_width(2.2)
        cr.stroke()
        _arrowhead(cr, -2, -1, 0, 4, ORANGE)
    elif name == "select":
        cr.set_dash([3, 2.5])
        cr.rectangle(-12, -10, 24, 20)
        cr.set_source_rgb(*BLUE)
        cr.set_line_width(1.6)
        cr.stroke()
        cr.set_dash([])
        cr.move_to(0, -3)
        cr.line_to(0, 11)
        cr.line_to(3.5, 7.5)
        cr.line_to(7, 13)
        cr.line_to(9, 12)
        cr.line_to(6, 6.5)
        cr.line_to(10.5, 6)
        cr.close_path()
        cr.set_source_rgb(1, 1, 1)
        cr.fill_preserve()
        cr.set_source_rgb(*DARK)
        cr.set_line_width(1.4)
        cr.stroke()
    elif name == "text":
        cr.select_font_face("Serif", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_BOLD)
        cr.set_font_size(24)
        ext = cr.text_extents("A")
        cr.move_to(-ext.width / 2 - ext.x_bearing - 3, ext.height / 2)
        cr.set_source_rgb(*DARK)
        cr.show_text("A")
        _line(cr, [(9, -10), (9, 11)], 1.6, BLUE)
    elif name == "laser":
        for r, a in ((11, 0.15), (7, 0.35)):
            cr.arc(0, 0, r, 0, 2 * math.pi)
            cr.set_source_rgba(*RED, a)
            cr.fill()
        cr.arc(0, 0, 4, 0, 2 * math.pi)
        cr.set_source_rgb(*RED)
        cr.fill()
        for i in range(8):
            a = i * math.pi / 4
            _line(cr, [(13 * math.cos(a), 13 * math.sin(a)), (15.5 * math.cos(a), 15.5 * math.sin(a))],
                  1.6, RED)
    elif name == "vanish":
        pts = [(-13 + i, 4 * math.sin(i / 4.0)) for i in range(0, 27)]
        for a, b in zip(pts, pts[1:]):
            t = (a[0] + 13) / 26.0
            cr.set_source_rgba(*BLUE, 1.0 - 0.85 * t)
            cr.set_line_width(2.8)
            cr.set_line_cap(cairo.LINE_CAP_ROUND)
            cr.move_to(*a)
            cr.line_to(*b)
            cr.stroke()
    elif name == "fill":
        cr.move_to(-6, -12)
        cr.line_to(7, 1)
        cr.line_to(-2, 10)
        cr.line_to(-15, -3)
        cr.close_path()
        cr.set_source_rgb(0.95, 0.95, 0.97)
        cr.fill_preserve()
        cr.set_source_rgb(*DARK)
        cr.set_line_width(1.8)
        cr.stroke()
        cr.move_to(-15, -3)
        cr.line_to(7, 1)
        cr.line_to(-2, 10)
        cr.close_path()
        cr.set_source_rgb(*BLUE)
        cr.fill()
        cr.move_to(10, 3)
        cr.curve_to(14, 9, 14, 12, 10.5, 12.5)
        cr.curve_to(7, 12, 7, 8, 10, 3)
        cr.fill()
    elif name == "ruler":
        cr.save()
        cr.rotate(-0.5)
        _ruler_shape(cr, 30, 10)
        cr.restore()
    elif name == "setsquare":
        cr.move_to(-12, 11)
        cr.line_to(-12, -12)
        cr.line_to(12, 11)
        cr.close_path()
        cr.set_source_rgb(0.85, 0.93, 1.0)
        cr.fill_preserve()
        cr.set_source_rgb(*BLUE)
        cr.set_line_width(1.6)
        cr.stroke()
        cr.move_to(-7, 6.5)
        cr.line_to(-7, -2)
        cr.line_to(1.5, 6.5)
        cr.close_path()
        cr.stroke()
    elif name == "protractor":
        cr.arc(0, 7, 14, math.pi, 2 * math.pi)
        cr.close_path()
        cr.set_source_rgb(0.85, 0.93, 1.0)
        cr.fill_preserve()
        cr.set_source_rgb(*BLUE)
        cr.set_line_width(1.6)
        cr.stroke()
        for d in range(0, 181, 30):
            a = math.pi + math.radians(d)
            cr.move_to(14 * math.cos(a), 7 + 14 * math.sin(a))
            cr.line_to(10 * math.cos(a), 7 + 10 * math.sin(a))
        cr.set_line_width(1)
        cr.stroke()
        _line(cr, [(0, 7), (9, -2)], 1.6, ORANGE)
    elif name == "compass":
        _line(cr, [(-7, 12), (0, -9), (7, 12)], 2.6, DARK)
        cr.arc(0, -10, 3, 0, 2 * math.pi)
        cr.set_source_rgb(*ORANGE)
        cr.fill()
        cr.arc(0, 12, 9, math.radians(200), math.radians(340))
        cr.set_source_rgb(*BLUE)
        cr.set_line_width(1.6)
        cr.set_dash([2.5, 2])
        cr.stroke()
        cr.set_dash([])
    elif name == "spotlight":
        cr.rectangle(-15, -12, 30, 24)
        cr.set_source_rgb(0.15, 0.15, 0.2)
        cr.fill()
        cr.arc(2, 0, 8, 0, 2 * math.pi)
        cr.set_source_rgb(1, 0.95, 0.6)
        cr.fill()
    elif name == "magnifier":
        cr.arc(-3, -3, 9, 0, 2 * math.pi)
        cr.set_source_rgb(0.88, 0.95, 1.0)
        cr.fill_preserve()
        cr.set_source_rgb(*DARK)
        cr.set_line_width(2.6)
        cr.stroke()
        _line(cr, [(4, 4), (12, 12)], 4.5, DARK)
        _line(cr, [(-3, -7), (-3, 1)], 1.8, BLUE)
        _line(cr, [(-7, -3), (1, -3)], 1.8, BLUE)
    elif name == "timer":
        cr.arc(0, 2, 11, 0, 2 * math.pi)
        cr.set_source_rgb(1, 1, 1)
        cr.fill_preserve()
        cr.set_source_rgb(*DARK)
        cr.set_line_width(2.2)
        cr.stroke()
        _line(cr, [(0, 2), (0, -5)], 2.2, ORANGE)
        _line(cr, [(0, 2), (5, 5)], 2.0, DARK)
        _line(cr, [(-3, -12), (3, -12)], 2.6, DARK)
    elif name == "lots":
        for i, (dx, dy) in enumerate(((-6, -4), (2, 2))):
            cr.save()
            cr.translate(dx, dy)
            cr.rotate(-0.3 + i * 0.5)
            cr.rectangle(-7, -7, 14, 14)
            cr.set_source_rgb(1, 1, 1)
            cr.fill_preserve()
            cr.set_source_rgb(*DARK)
            cr.set_line_width(1.6)
            cr.stroke()
            for px, py in ((-3, -3), (3, 3), (0, 0)) if i else ((-3, -3), (3, 3)):
                cr.arc(px, py, 1.5, 0, 2 * math.pi)
                cr.set_source_rgb(*RED)
                cr.fill()
            cr.restore()
    elif name == "screenshot":
        cr.rectangle(-14, -9, 28, 20)
        cr.set_source_rgb(*DARK)
        cr.set_line_width(2)
        cr.stroke()
        cr.arc(0, 1, 5.5, 0, 2 * math.pi)
        cr.stroke()
        cr.rectangle(-5, -13, 10, 4)
        cr.fill()
    elif name == "clear":
        cr.move_to(-9, -6)
        cr.line_to(-7, 12)
        cr.line_to(7, 12)
        cr.line_to(9, -6)
        cr.close_path()
        cr.set_source_rgb(*DARK)
        cr.set_line_width(2)
        cr.stroke()
        _line(cr, [(-12, -9), (12, -9)], 2.2)
        _line(cr, [(-3, -12), (3, -12)], 2.2)
        _line(cr, [(-3, -2), (-2.5, 8)], 1.6)
        _line(cr, [(3, -2), (2.5, 8)], 1.6)
    elif name in ("png", "pdf"):
        cr.move_to(-10, -13)
        cr.line_to(5, -13)
        cr.line_to(11, -7)
        cr.line_to(11, 13)
        cr.line_to(-10, 13)
        cr.close_path()
        cr.set_source_rgb(1, 1, 1)
        cr.fill_preserve()
        cr.set_source_rgb(*DARK)
        cr.set_line_width(1.6)
        cr.stroke()
        cr.rectangle(-13, -2, 22, 10)
        cr.set_source_rgb(*(RED if name == "pdf" else GREEN))
        cr.fill()
        cr.select_font_face("Sans", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_BOLD)
        cr.set_font_size(8)
        cr.set_source_rgb(1, 1, 1)
        cr.move_to(-11.5, 6)
        cr.show_text(name.upper())
    elif name == "save":
        cr.rectangle(-12, -12, 24, 24)
        cr.set_source_rgb(*BLUE)
        cr.fill()
        cr.rectangle(-7, -12, 14, 9)
        cr.set_source_rgb(1, 1, 1)
        cr.fill()
        cr.rectangle(-8, 3, 16, 9)
        cr.fill()
        cr.rectangle(2, -10, 3, 5)
        cr.set_source_rgb(*BLUE)
        cr.fill()
    elif name == "open":
        cr.move_to(-14, -8)
        cr.line_to(-5, -8)
        cr.line_to(-2, -5)
        cr.line_to(12, -5)
        cr.line_to(12, 11)
        cr.line_to(-14, 11)
        cr.close_path()
        cr.set_source_rgb(1.0, 0.8, 0.3)
        cr.fill_preserve()
        cr.set_source_rgb(0.7, 0.5, 0.1)
        cr.set_line_width(1.4)
        cr.stroke()
        cr.move_to(-11, -1)
        cr.line_to(15, -1)
        cr.line_to(12, 11)
        cr.line_to(-14, 11)
        cr.close_path()
        cr.set_source_rgb(1.0, 0.88, 0.45)
        cr.fill()
    elif name == "share":
        pts = [(-9, 0), (8, -9), (8, 9)]
        _line(cr, [pts[1], pts[0], pts[2]], 2, DARK)
        for x, y in pts:
            cr.arc(x, y, 4.5, 0, 2 * math.pi)
            cr.set_source_rgb(*BLUE)
            cr.fill()
    elif name == "page:prev":
        _line(cr, [(5, -10), (-5, 0), (5, 10)], 3, DARK)
    elif name == "page:next":
        _line(cr, [(-5, -10), (5, 0), (-5, 10)], 3, DARK)
    elif name in ("page:new", "page:delete"):
        cr.rectangle(-9, -12, 18, 24)
        cr.set_source_rgb(1, 1, 1)
        cr.fill_preserve()
        cr.set_source_rgb(*DARK)
        cr.set_line_width(1.6)
        cr.stroke()
        if name == "page:new":
            _line(cr, [(0, -5), (0, 5)], 2.6, GREEN)
            _line(cr, [(-5, 0), (5, 0)], 2.6, GREEN)
        else:
            _line(cr, [(-5, 0), (5, 0)], 2.6, RED)
    elif name == "toolbox":
        cr.save()
        cr.rotate(0.6)
        _ruler_shape(cr, 30, 9)
        cr.restore()
        cr.save()
        cr.rotate(-0.75)
        cr.rectangle(-13, -3, 22, 6)
        cr.set_source_rgb(*ORANGE)
        cr.fill()
        cr.move_to(9, -3)
        cr.line_to(14, 0)
        cr.line_to(9, 3)
        cr.close_path()
        cr.set_source_rgb(0.95, 0.85, 0.65)
        cr.fill()
        cr.arc(13.2, 0, 1.2, 0, 2 * math.pi)
        cr.set_source_rgb(*DARK)
        cr.fill()
        cr.restore()
    else:
        cr.arc(0, 0, 10, 0, 2 * math.pi)
        cr.set_source_rgb(*DARK)
        cr.set_line_width(2)
        cr.stroke()


def draw_icon(cr, name, cx, cy, size):
    cr.save()
    cr.new_path()          # önceki yazının "geçerli noktası" çizgi bırakmasın
    cr.translate(cx, cy)
    k = size / 34.0
    cr.scale(k, k)
    _draw(cr, name)
    cr.new_path()
    cr.restore()


def draw_button(cr, name, x, y, size, alpha=1.0):
    """Ana menüdeki 42x42 düğme alanına simge çizer."""
    cr.save()
    if alpha < 1.0:
        cr.push_group()
    draw_icon(cr, name, x + size / 2.0, y + size / 2.0, size * 0.85)
    if alpha < 1.0:
        cr.pop_group_to_source()
        cr.paint_with_alpha(alpha)
    cr.restore()
