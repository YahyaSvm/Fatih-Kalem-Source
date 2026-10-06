"""Sahne durumu ve geri al / yinele geçmişi.

Orijinal programda her işlem bir "kare" (FrameNo) olarak saklanır ve geri
alma; çizgi ekleme, silme, perde ekleme/kaldırma, temizleme (CLS), kütüphane
görseli ekleme ve arka plan sayfası ekleme işlemlerini kapsar. Burada aynı
davranış, değiştirilemez sahne anlık görüntüleriyle sağlanır.
"""

import math


class LibraryItem:
    """Ekrana yerleştirilmiş görsel (kütüphane öğesi)."""

    __slots__ = ("path", "x", "y", "width", "height", "angle")

    def __init__(self, path, x, y, width, height, angle=0.0):
        self.path = path
        self.x = float(x)          # sol üst köşe (döndürmeden önce)
        self.y = float(y)
        self.width = float(width)
        self.height = float(height)
        self.angle = float(angle)  # derece, merkez etrafında

    def clone(self):
        return LibraryItem(self.path, self.x, self.y, self.width, self.height,
                           self.angle)

    @property
    def center(self):
        return (self.x + self.width / 2.0, self.y + self.height / 2.0)

    def contains(self, px, py):
        cx, cy = self.center
        a = -math.radians(self.angle)
        dx, dy = px - cx, py - cy
        lx = dx * math.cos(a) - dy * math.sin(a)
        ly = dx * math.sin(a) + dy * math.cos(a)
        return abs(lx) <= self.width / 2.0 and abs(ly) <= self.height / 2.0

    def bbox(self):
        cx, cy = self.center
        r = 0.5 * math.hypot(self.width, self.height) + 2
        return (cx - r, cy - r, cx + r, cy + r)


class Scene:
    """Değiştirilemez sahne anlık görüntüsü."""

    __slots__ = ("strokes", "backgrounds", "library", "curtain")

    def __init__(self, strokes=(), backgrounds=(), library=(), curtain=None):
        self.strokes = tuple(strokes)
        self.backgrounds = tuple(backgrounds)   # dosya yolları (en sonuncusu üstte)
        self.library = tuple(library)           # LibraryItem
        self.curtain = curtain                  # (x, y, w, h) açık alan ya da None

    def replace(self, **kw):
        d = {"strokes": self.strokes, "backgrounds": self.backgrounds,
             "library": self.library, "curtain": self.curtain}
        d.update(kw)
        return Scene(**d)

    def is_empty(self):
        return not (self.strokes or self.backgrounds or self.library
                    or self.curtain is not None)


class History:
    def __init__(self, limit=500):
        self.scene = Scene()
        self._undo = []
        self._redo = []
        self.limit = limit

    def commit(self, new_scene):
        if new_scene is self.scene:
            return
        self._undo.append(self.scene)
        if len(self._undo) > self.limit:
            del self._undo[0]
        self._redo.clear()
        self.scene = new_scene

    def set_without_history(self, new_scene):
        """Geçici durumlar (ör. canlı önizleme) için."""
        self.scene = new_scene

    def can_undo(self):
        return bool(self._undo)

    def can_redo(self):
        return bool(self._redo)

    def undo(self):
        if not self._undo:
            return False
        self._redo.append(self.scene)
        self.scene = self._undo.pop()
        return True

    def redo(self):
        if not self._redo:
            return False
        self._undo.append(self.scene)
        self.scene = self._redo.pop()
        return True

    def reset(self):
        self.scene = Scene()
        self._undo.clear()
        self._redo.clear()
