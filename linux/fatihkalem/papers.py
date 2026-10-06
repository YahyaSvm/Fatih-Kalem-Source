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


PAGES = [
    ("01 Beyaz Sayfa.png", _white),
    ("02 Çizgili Sayfa.png", _lined),
    ("03 Kareli Sayfa.png", _grid),
    ("04 Milimetrik Sayfa.png", _millimetric),
    ("05 Noktalı Sayfa.png", _dotted),
    ("06 Yeşil Tahta.png", _board),
    ("07 Siyah Tahta.png", _black),
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
