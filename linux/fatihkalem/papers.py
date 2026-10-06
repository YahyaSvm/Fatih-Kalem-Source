"""Arka plan sayfaları (çizgili, kareli, milimetrik ...).

Orijinal program kurulum klasöründeki "Arka Plan Sayfaları" klasöründen
resim seçtirir. Pardus sürümü bu sayfaları ilk açılışta ekran
çözünürlüğünde kendisi üretir; kullanıcı aynı klasöre kendi resimlerini de
koyabilir.
"""

import os

import cairo

from .config import data_home

PAPERS_DIRNAME = "Arka Plan Sayfaları"
LIBRARY_DIRNAME = "Görseller"


def papers_dir():
    return os.path.join(data_home(), PAPERS_DIRNAME)


def library_dirs():
    """Kütüphane görsellerinin aranacağı klasörler (var olanlar)."""
    dirs = [os.path.join(data_home(), LIBRARY_DIRNAME),
            "/usr/share/fatih-kalem/gorseller"]
    return [d for d in dirs if os.path.isdir(d)]


def _page(path, w, h, draw):
    surf = cairo.ImageSurface(cairo.FORMAT_RGB24, w, h)
    cr = cairo.Context(surf)
    draw(cr, w, h)
    surf.write_to_png(path)


def _white(cr, w, h):
    cr.set_source_rgb(1, 1, 1)
    cr.paint()


def _lined(cr, w, h):
    _white(cr, w, h)
    step = max(32, h // 24)
    cr.set_source_rgb(0.55, 0.72, 0.9)
    cr.set_line_width(1.5)
    y = step * 2
    while y < h:
        cr.move_to(0, y + 0.5)
        cr.line_to(w, y + 0.5)
        y += step
    cr.stroke()
    cr.set_source_rgb(0.92, 0.45, 0.45)
    cr.move_to(step * 3 + 0.5, 0)
    cr.line_to(step * 3 + 0.5, h)
    cr.stroke()


def _grid(cr, w, h):
    _white(cr, w, h)
    step = max(30, h // 27)
    cr.set_source_rgb(0.6, 0.75, 0.9)
    cr.set_line_width(1)
    for x in range(0, w, step):
        cr.move_to(x + 0.5, 0)
        cr.line_to(x + 0.5, h)
    for y in range(0, h, step):
        cr.move_to(0, y + 0.5)
        cr.line_to(w, y + 0.5)
    cr.stroke()


def _millimetric(cr, w, h):
    _white(cr, w, h)
    mm = max(4, h // 216)
    for i, (width, alpha) in enumerate(((0.6, 0.35), (1.0, 0.6), (1.6, 0.9))):
        every = (1, 5, 10)[i]
        cr.set_source_rgba(0.95, 0.55, 0.25, alpha)
        cr.set_line_width(width)
        for x in range(0, w, mm * every):
            cr.move_to(x + 0.5, 0)
            cr.line_to(x + 0.5, h)
        for y in range(0, h, mm * every):
            cr.move_to(0, y + 0.5)
            cr.line_to(w, y + 0.5)
        cr.stroke()


def _dotted(cr, w, h):
    _white(cr, w, h)
    step = max(30, h // 27)
    cr.set_source_rgb(0.45, 0.55, 0.7)
    for x in range(step, w, step):
        for y in range(step, h, step):
            cr.arc(x, y, 1.6, 0, 6.2832)
            cr.fill()


def _board(cr, w, h):
    cr.set_source_rgb(0.12, 0.30, 0.22)
    cr.paint()


def _black(cr, w, h):
    cr.set_source_rgb(0.08, 0.08, 0.1)
    cr.paint()


def _four_lines(cr, w, h):
    """İlkokul dört çizgili defter (yazı satırları)."""
    _white(cr, w, h)
    band = max(80, h // 11)
    gap = band // 3
    y = band
    while y + band < h:
        for i, (rgb, width) in enumerate((((0.55, 0.72, 0.9), 1.4), ((0.85, 0.5, 0.5), 1.0),
                                          ((0.85, 0.5, 0.5), 1.0), ((0.55, 0.72, 0.9), 1.4))):
            cr.set_source_rgb(*rgb)
            cr.set_line_width(width)
            yy = y + i * gap
            if i in (1, 2):
                cr.set_dash([6, 5])
            cr.move_to(0, yy + 0.5)
            cr.line_to(w, yy + 0.5)
            cr.stroke()
            cr.set_dash([])
        y += band + gap * 2


def _music(cr, w, h):
    """Müzik portesi (beşli çizgi grupları)."""
    _white(cr, w, h)
    step = max(14, h // 75)
    group = step * 4
    y = step * 6
    cr.set_source_rgb(0.25, 0.25, 0.3)
    cr.set_line_width(1.3)
    while y + group < h - step * 2:
        for i in range(5):
            cr.move_to(w * 0.04, y + i * step + 0.5)
            cr.line_to(w * 0.96, y + i * step + 0.5)
        cr.stroke()
        y += group + step * 5


def _coordinate(cr, w, h):
    """Koordinat düzlemi: kareli zemin, x / y eksenleri ve sayılar."""
    _grid(cr, w, h)
    step = max(30, h // 27)
    cx = (w // 2) // step * step
    cy = (h // 2) // step * step
    cr.set_source_rgb(0.15, 0.15, 0.2)
    cr.set_line_width(2.2)
    cr.move_to(0, cy + 0.5)
    cr.line_to(w, cy + 0.5)
    cr.move_to(cx + 0.5, 0)
    cr.line_to(cx + 0.5, h)
    cr.stroke()
    for (x0, y0, x1, y1) in ((w - 16, cy - 8, w, cy), (w - 16, cy + 8, w, cy),
                             (cx - 8, 16, cx, 0), (cx + 8, 16, cx, 0)):
        cr.move_to(x0, y0)
        cr.line_to(x1, y1)
    cr.stroke()
    cr.select_font_face("Sans")
    cr.set_font_size(max(12, step * 0.42))
    for k in range(-(cx // step) + 1, (w - cx) // step):
        if k:
            cr.move_to(cx + k * step - 5, cy + step * 0.6)
            cr.show_text(str(k))
    for k in range(-(cy // step) + 1, (h - cy) // step):
        if k:
            cr.move_to(cx + 6, cy + k * step + 5)
            cr.show_text(str(-k))
    cr.move_to(w - 22, cy - 12)
    cr.show_text("x")
    cr.move_to(cx + 12, 22)
    cr.show_text("y")


def _number_line(cr, w, h):
    """Sayı doğrusu (-10 ... 10)."""
    _white(cr, w, h)
    y = h // 2
    left, right = w * 0.05, w * 0.95
    unit = (right - left) / 20.0
    cr.set_source_rgb(0.15, 0.15, 0.2)
    cr.set_line_width(3)
    cr.move_to(left - 20, y)
    cr.line_to(right + 20, y)
    cr.stroke()
    for sx in (-1, 1):
        x = right + 20 if sx > 0 else left - 20
        cr.move_to(x - sx * 16, y - 10)
        cr.line_to(x, y)
        cr.line_to(x - sx * 16, y + 10)
        cr.stroke()
    cr.select_font_face("Sans")
    cr.set_font_size(max(18, h // 45))
    for k in range(-10, 11):
        x = left + (k + 10) * unit
        cr.set_line_width(2.5 if k == 0 else 1.6)
        cr.move_to(x, y - (18 if k % 5 == 0 else 11))
        cr.line_to(x, y + (18 if k % 5 == 0 else 11))
        cr.stroke()
        label = str(k)
        ext = cr.text_extents(label)
        cr.move_to(x - ext.width / 2 - ext.x_bearing, y + 48)
        cr.show_text(label)


def _isometric(cr, w, h):
    """İzometrik (üçgen) nokta kâğıdı: geometri ve hacim çizimleri için."""
    _white(cr, w, h)
    step = max(36, h // 24)
    dy = step * 0.866
    cr.set_source_rgb(0.4, 0.5, 0.65)
    row = 0
    y = dy / 2
    while y < h:
        x = step / 2 if row % 2 else 0
        while x < w:
            cr.arc(x, y, 2.0, 0, 6.2832)
            cr.fill()
            x += step
        y += dy
        row += 1


PAGES = [
    ("01 Beyaz Sayfa.png", _white),
    ("02 Çizgili Sayfa.png", _lined),
    ("03 Kareli Sayfa.png", _grid),
    ("04 Milimetrik Sayfa.png", _millimetric),
    ("05 Noktalı Sayfa.png", _dotted),
    ("06 Yeşil Tahta.png", _board),
    ("07 Siyah Tahta.png", _black),
    ("08 Dört Çizgili Defter.png", _four_lines),
    ("09 Müzik Portesi.png", _music),
    ("10 Koordinat Düzlemi.png", _coordinate),
    ("11 Sayı Doğrusu.png", _number_line),
    ("12 İzometrik Kâğıt.png", _isometric),
]


def ensure_papers(width, height):
    """Eksik sayfaları üretir, klasör yolunu döndürür."""
    d = papers_dir()
    os.makedirs(d, exist_ok=True)
    os.makedirs(os.path.join(data_home(), LIBRARY_DIRNAME), exist_ok=True)
    stamp = os.path.join(d, ".boyut")
    size = "%dx%d" % (width, height)
    try:
        with open(stamp, encoding="utf-8") as f:
            same = f.read().strip() == size
    except OSError:
        same = False
    for name, fn in PAGES:
        path = os.path.join(d, name)
        if not same or not os.path.exists(path):
            try:
                _page(path, width, height, fn)
            except (OSError, cairo.Error):
                pass
    try:
        with open(stamp, "w", encoding="utf-8") as f:
            f.write(size)
    except OSError:
        pass
    return d
