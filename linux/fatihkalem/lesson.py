"""Ders dosyaları (.fkalem), PNG / PDF dışa aktarma ve otomatik kayıt.

.fkalem dosyası bir ZIP arşividir:
    ders.json          sayfalar, çizgiler, metinler, dolgular, görseller
    gorseller/<özet>   kullanılan arka plan ve kütüphane görselleri
Böylece ders başka bir tahtada (USB bellekle) aynen açılabilir.
"""

import hashlib
import json
import os
import shutil
import tempfile
import time
import zipfile

import cairo

from .config import data_home
from .ink import Stroke
from .render import render_scene
from .scene import Book, FillItem, History, LibraryItem, Scene, TextItem

FORMAT_VERSION = 1
EXTENSION = ".fkalem"


def lessons_dir():
    d = os.path.join(data_home(), "Dersler")
    os.makedirs(d, exist_ok=True)
    return d


def autosave_dir():
    d = os.path.join(data_home(), "Otomatik Kayıt")
    os.makedirs(d, exist_ok=True)
    return d


def _images_cache():
    d = os.path.join(data_home(), "ders-gorselleri")
    os.makedirs(d, exist_ok=True)
    return d


# ------------------------------------------------------------ serileştirme --

def _stroke_dict(s):
    return {"p": [[round(x, 1), round(y, 1), round(p, 2)] for x, y, p in s.points],
            "c": [round(v, 4) for v in s.color], "w": s.width, "h": s.height,
            "t": s.tip, "r": s.rotation, "hl": s.highlighter}


def _stroke_from(d):
    return Stroke([tuple(p) for p in d["p"]], tuple(d["c"]), d["w"], d["h"],
                  d.get("t", "ellipse"), d.get("r", 0.0), d.get("hl", False))


def _scene_dict(scene, image_ref):
    return {
        "strokes": [_stroke_dict(s) for s in scene.strokes],
        "texts": [{"text": t.text, "x": t.x, "y": t.y, "size": t.size,
                   "color": list(t.color)} for t in scene.texts],
        "fills": [{"points": [list(p) for p in f.points], "color": list(f.color),
                   "alpha": f.alpha} for f in scene.fills],
        "backgrounds": [image_ref(p) for p in scene.backgrounds],
        "library": [{"path": image_ref(i.path), "x": i.x, "y": i.y, "w": i.width,
                     "h": i.height, "a": i.angle} for i in scene.library],
        "curtain": list(scene.curtain) if scene.curtain is not None else None,
    }


def _scene_from(d, image_path):
    return Scene(
        strokes=[_stroke_from(s) for s in d.get("strokes", [])],
        texts=[TextItem(t["text"], t["x"], t["y"], t["size"], t["color"])
               for t in d.get("texts", [])],
        fills=[FillItem([tuple(p) for p in f["points"]], f["color"], f.get("alpha", 0.45))
               for f in d.get("fills", [])],
        backgrounds=[p for p in (image_path(b) for b in d.get("backgrounds", [])) if p],
        library=[LibraryItem(image_path(i["path"]), i["x"], i["y"], i["w"], i["h"], i.get("a", 0))
                 for i in d.get("library", []) if image_path(i["path"])],
        curtain=tuple(d["curtain"]) if d.get("curtain") else None,
    )


def save_lesson(path, book, width, height):
    if not path.endswith(EXTENSION):
        path += EXTENSION
    images = {}

    def image_ref(src):
        if src in images:
            return images[src]
        try:
            with open(src, "rb") as f:
                digest = hashlib.sha1(f.read()).hexdigest()[:16]
        except OSError:
            images[src] = None
            return None
        name = "gorseller/%s%s" % (digest, os.path.splitext(src)[1].lower() or ".png")
        images[src] = name
        return name

    data = {"format": FORMAT_VERSION, "program": "Fatih Kalem", "width": width,
            "height": height, "current": book.index,
            "pages": [_scene_dict(h.scene, image_ref) for h in book.pages]}
    fd, tmp = tempfile.mkstemp(suffix=EXTENSION, dir=os.path.dirname(os.path.abspath(path)))
    os.close(fd)
    try:
        with zipfile.ZipFile(tmp, "w", zipfile.ZIP_DEFLATED) as z:
            z.writestr("ders.json", json.dumps(data, ensure_ascii=False))
            for src, name in images.items():
                if name:
                    z.write(src, name)
        umask = os.umask(0)
        os.umask(umask)
        os.chmod(tmp, 0o666 & ~umask)     # mkstemp 0600 açar; normal izin ver
        os.replace(tmp, path)
    finally:
        if os.path.exists(tmp):
            os.unlink(tmp)
    return path


def load_lesson(path):
    """(Book, (genişlik, yükseklik)) döndürür."""
    cache = _images_cache()
    with zipfile.ZipFile(path) as z:
        data = json.loads(z.read("ders.json").decode("utf-8"))
        extracted = {}
        for name in z.namelist():
            if name.startswith("gorseller/") and not name.endswith("/"):
                target = os.path.join(cache, os.path.basename(name))
                if not os.path.exists(target):
                    with z.open(name) as src, open(target, "wb") as dst:
                        shutil.copyfileobj(src, dst)
                extracted[name] = target

    def image_path(ref):
        return extracted.get(ref) if ref else None

    book = Book()
    book.pages = []
    for page in data.get("pages", []):
        h = History()
        h.scene = _scene_from(page, image_path)
        book.pages.append(h)
    if not book.pages:
        book.pages = [History()]
    book.index = min(max(0, data.get("current", 0)), len(book.pages) - 1)
    return book, (data.get("width", 0), data.get("height", 0))


# ----------------------------------------------------------- dışa aktarma --

def export_png(path, scene, width, height, pictures, background=(1, 1, 1)):
    if not path.lower().endswith(".png"):
        path += ".png"
    surf = cairo.ImageSurface(cairo.FORMAT_ARGB32, width, height)
    cr = cairo.Context(surf)
    if background is not None:
        cr.set_source_rgb(*background)
        cr.paint()
    render_scene(cr, scene, width, height, pictures)
    surf.write_to_png(path)
    return path


def export_pdf(path, scenes, width, height, pictures, background=(1, 1, 1)):
    """Her sahne bir PDF sayfası olur (piksel = punto)."""
    if not path.lower().endswith(".pdf"):
        path += ".pdf"
    surf = cairo.PDFSurface(path, width, height)
    surf.set_metadata(cairo.PDF_METADATA_CREATOR, "Fatih Kalem")
    surf.set_metadata(cairo.PDF_METADATA_TITLE, "Fatih Kalem dersi")
    cr = cairo.Context(surf)
    for scene in scenes:
        if background is not None:
            cr.set_source_rgb(*background)
            cr.paint()
        cr.save()
        render_scene(cr, scene, width, height, pictures)
        cr.restore()
        cr.show_page()
    surf.finish()
    return path


def default_name(ext):
    return time.strftime("Ders %Y-%m-%d %H.%M") + ext


def autosave(book, width, height, keep=10):
    """Otomatik kayıt klasörüne yazar; en yeni `keep` dosya tutulur."""
    if book.is_empty():
        return None
    d = autosave_dir()
    path = os.path.join(d, time.strftime("Otomatik %Y-%m-%d %H.%M") + EXTENSION)
    save_lesson(path, book, width, height)
    files = sorted((f for f in os.listdir(d) if f.endswith(EXTENSION)),
                   key=lambda f: os.path.getmtime(os.path.join(d, f)))
    for old in files[:-keep]:
        try:
            os.unlink(os.path.join(d, old))
        except OSError:
            pass
    return path
