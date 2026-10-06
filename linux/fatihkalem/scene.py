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

    __slots__ = ("strokes", "backgrounds", "library", "curtain", "texts", "fills")

    def __init__(self, strokes=(), backgrounds=(), library=(), curtain=None,
                 texts=(), fills=()):
        self.strokes = tuple(strokes)
        self.backgrounds = tuple(backgrounds)   # dosya yolları (en sonuncusu üstte)
        self.library = tuple(library)           # LibraryItem
        self.curtain = curtain                  # (x, y, w, h) açık alan ya da None
        self.texts = tuple(texts)               # TextItem
        self.fills = tuple(fills)               # FillItem

    def replace(self, **kw):
        d = {"strokes": self.strokes, "backgrounds": self.backgrounds,
             "library": self.library, "curtain": self.curtain,
             "texts": self.texts, "fills": self.fills}
        d.update(kw)
        return Scene(**d)

    def is_empty(self):
        return not (self.strokes or self.backgrounds or self.library
                    or self.curtain is not None or self.texts or self.fills)

    def cleared(self):
        return Scene()


class TextItem:
    """Ekrana yazılmış metin (değiştirilemez)."""

    __slots__ = ("text", "x", "y", "size", "color")

    def __init__(self, text, x, y, size, color):
        self.text = text
        self.x = float(x)        # sol üst köşe
        self.y = float(y)
        self.size = float(size)  # punto (px)
        self.color = tuple(color)

    def moved(self, dx, dy, scale=1.0, origin=None):
        if origin is None:
            return TextItem(self.text, self.x + dx, self.y + dy, self.size, self.color)
        ox, oy = origin
        return TextItem(self.text, ox + (self.x - ox) * scale + dx,
                        oy + (self.y - oy) * scale + dy, self.size * scale, self.color)

    def bbox(self):
        # Yaklaşık kutu: Pango ölçüsü pencere tarafında hesaplanır, burada
        # seçim/silgi için kaba bir sınır yeterli.
        lines = self.text.split("\n") or [""]
        w = max(len(l) for l in lines) * self.size * 0.6 + 4
        h = len(lines) * self.size * 1.3 + 4
        return (self.x, self.y, self.x + w, self.y + h)


class FillItem:
    """Kapalı bir bölgenin boya kovasıyla doldurulmuş hali."""

    __slots__ = ("points", "color", "alpha")

    def __init__(self, points, color, alpha=0.45):
        self.points = tuple(points)
        self.color = tuple(color)
        self.alpha = float(alpha)

    def moved(self, dx, dy, scale=1.0, origin=None):
        if origin is None:
            return FillItem([(x + dx, y + dy) for x, y in self.points], self.color, self.alpha)
        ox, oy = origin
        return FillItem([(ox + (x - ox) * scale + dx, oy + (y - oy) * scale + dy)
                         for x, y in self.points], self.color, self.alpha)

    def bbox(self):
        xs = [p[0] for p in self.points]
        ys = [p[1] for p in self.points]
        return (min(xs), min(ys), max(xs), max(ys))


def point_in_polygon(x, y, poly):
    inside = False
    n = len(poly)
    j = n - 1
    for i in range(n):
        xi, yi = poly[i][0], poly[i][1]
        xj, yj = poly[j][0], poly[j][1]
        if (yi > y) != (yj > y) and x < (xj - xi) * (y - yi) / ((yj - yi) or 1e-9) + xi:
            inside = not inside
        j = i
    return inside


def polygon_area(poly):
    a = 0.0
    for i in range(len(poly)):
        x1, y1 = poly[i][0], poly[i][1]
        x2, y2 = poly[(i + 1) % len(poly)][0], poly[(i + 1) % len(poly)][1]
        a += x1 * y2 - x2 * y1
    return abs(a) / 2.0


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


class Book:
    """Çok sayfalı tahta: her sayfanın kendi geri al / yinele geçmişi vardır."""

    MAX_PAGES = 99

    def __init__(self):
        self.pages = [History()]
        self.index = 0

    @property
    def current(self):
        return self.pages[self.index]

    def __len__(self):
        return len(self.pages)

    def new_page(self):
        if len(self.pages) >= self.MAX_PAGES:
            return False
        self.pages.insert(self.index + 1, History())
        self.index += 1
        return True

    def go(self, index):
        if 0 <= index < len(self.pages) and index != self.index:
            self.index = index
            return True
        return False

    def delete_page(self):
        if len(self.pages) == 1:
            self.pages[0].reset()
            return
        del self.pages[self.index]
        self.index = min(self.index, len(self.pages) - 1)

    def is_empty(self):
        return all(h.scene.is_empty() for h in self.pages)

    def reset(self):
        self.pages = [History()]
        self.index = 0
