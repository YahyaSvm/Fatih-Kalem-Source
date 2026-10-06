"""Sahne çizimi: ekran, PNG ve PDF dışa aktarımı aynı kodu kullanır."""

import math

import cairo
import gi

gi.require_version("Pango", "1.0")
gi.require_version("PangoCairo", "1.0")
from gi.repository import Pango, PangoCairo  # noqa: E402

from .ink import render_strokes  # noqa: E402

_measure_surface = cairo.ImageSurface(cairo.FORMAT_A8, 1, 1)
_measure_cr = cairo.Context(_measure_surface)


def _layout(cr, item):
    layout = PangoCairo.create_layout(cr)
    font = Pango.FontDescription("Sans")
    font.set_absolute_size(item.size * Pango.SCALE)
    layout.set_font_description(font)
    layout.set_text(item.text, -1)
    return layout


def text_size(item):
    """(genişlik, yükseklik) piksel."""
    layout = _layout(_measure_cr, item)
    w, h = layout.get_pixel_size()
    return w, h


def text_bbox(item):
    w, h = text_size(item)
    return (item.x, item.y, item.x + w, item.y + h)


def draw_text(cr, item):
    cr.save()
    cr.move_to(item.x, item.y)
    cr.set_source_rgb(*item.color)
    PangoCairo.show_layout(cr, _layout(cr, item))
    cr.restore()


def draw_fill(cr, item):
    pts = item.points
    if len(pts) < 3:
        return
    cr.save()
    cr.move_to(pts[0][0], pts[0][1])
    for p in pts[1:]:
        cr.line_to(p[0], p[1])
    cr.close_path()
    r, g, b = item.color
    cr.set_source_rgba(r, g, b, item.alpha)
    cr.fill()
    cr.restore()


def draw_curtain(cr, rect, W, H, placed=True):
    cr.save()
    cr.set_fill_rule(cairo.FILL_RULE_EVEN_ODD)
    cr.rectangle(0, 0, W, H)
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


def paint_picture(cr, surf, x, y, w, h, angle=0.0, alpha=1.0):
    if surf is None:
        return
    cr.save()
    cx, cy = x + w / 2.0, y + h / 2.0
    cr.translate(cx, cy)
    cr.rotate(math.radians(angle))
    cr.translate(-w / 2.0, -h / 2.0)
    cr.scale(w / surf.get_width(), h / surf.get_height())
    cr.set_source_surface(surf, 0, 0)
    cr.get_source().set_filter(cairo.FILTER_GOOD)
    cr.paint_with_alpha(alpha)
    cr.restore()


def render_scene(cr, scene, W, H, pictures, clip=None):
    """Katmanlar (alttan üste): arka plan sayfası, perde, kütüphane
    görselleri, dolgular, mürekkep, metinler."""
    if clip is not None:
        cr.rectangle(*clip)
        cr.clip()
    for path in scene.backgrounds[-1:]:
        surf = pictures(path)
        if surf is not None:
            paint_picture(cr, surf, 0, 0, W, H)
    if scene.curtain is not None:
        draw_curtain(cr, scene.curtain, W, H, placed=True)
    for item in scene.library:
        paint_picture(cr, pictures(item.path), item.x, item.y, item.width,
                      item.height, item.angle)
    for f in scene.fills:
        draw_fill(cr, f)
    render_strokes(cr, scene.strokes, clip)
    for t in scene.texts:
        draw_text(cr, t)
