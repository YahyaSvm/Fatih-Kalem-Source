"""Görsellerin (orijinal programın PNG kaynakları) bulunması ve önbelleği."""

import os

import cairo


def data_dir():
    env = os.environ.get("FATIHKALEM_DATA")
    candidates = [env] if env else []
    here = os.path.dirname(os.path.abspath(__file__))
    candidates += [os.path.join(here, "..", "data"),   # kaynak ağacı
                   os.path.join(here, ".."),           # kurulu: <önek>/share/fatih-kalem
                   "/usr/share/fatih-kalem",
                   "/usr/local/share/fatih-kalem"]
    for c in candidates:
        if c and os.path.isdir(os.path.join(c, "images")):
            return os.path.abspath(c)
    raise RuntimeError("Fatih Kalem görsel klasörü bulunamadı")


_cache = {}


def image(name):
    """Küçük harfli dosya adıyla (uzantısız) cairo yüzeyi döndürür."""
    key = name.lower()
    surf = _cache.get(key)
    if surf is None:
        path = os.path.join(data_dir(), "images", key + ".png")
        surf = cairo.ImageSurface.create_from_png(path)
        _cache[key] = surf
    return surf


def has_image(name):
    return os.path.exists(os.path.join(data_dir(), "images", name.lower() + ".png"))


def paint(cr, name, x, y, alpha=1.0, w=None, h=None):
    surf = image(name)
    cr.save()
    cr.translate(x, y)
    sw, sh = surf.get_width(), surf.get_height()
    if w is not None and h is not None and (w != sw or h != sh):
        cr.scale(w / sw, h / sh)
    cr.set_source_surface(surf, 0, 0)
    cr.get_source().set_filter(cairo.FILTER_GOOD)
    if alpha >= 1.0:
        cr.paint()
    else:
        cr.paint_with_alpha(alpha)
    cr.restore()
