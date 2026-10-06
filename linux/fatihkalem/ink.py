"""Mürekkep modeli: çizgiler (stroke), çizimi ve noktasal silme.

Orijinal WPF InkCanvas davranışı örnek alınmıştır:
  * Keçeli kalem (Marker): yuvarlak uç, genişlik = yükseklik.
  * Dolma kalem (Stylograph): elips uç, 45 derece döndürülmüş (kaligrafi).
  * Fosforlu kalem (Highlighter): dikdörtgen uç, yarı saydam, diğer
    mürekkebin altında çizilir ve üst üste binince koyulaşmaz.
  * Silgi: döndürülmüş dikdörtgen; çizgilerin yalnızca değdiği kısmını siler
    (InkCanvasEditingMode.EraseByPoint).

Çizgiler değiştirilemez (immutable) kabul edilir; silme işlemi eski çizgiyi
kaldırıp parçalarını yeni çizgi olarak ekler. Bu sayede geri alma geçmişi
sadece liste kopyalarıyla tutulabilir.
"""

import math

TIP_ELLIPSE = "ellipse"
TIP_RECT = "rect"

# Yeni çizgilerde noktalar arası en büyük mesafe (px). Silgi noktasal
# çalıştığından sık örnekleme kesimlerin düzgün görünmesini sağlar.
MAX_POINT_SPACING = 2.0


class Stroke:
    __slots__ = ("points", "color", "width", "height", "tip", "rotation",
                 "highlighter", "_bbox", "_poly_cache")

    def __init__(self, points, color, width, height, tip=TIP_ELLIPSE,
                 rotation=0.0, highlighter=False):
        # points: [(x, y, basınç 0..1), ...]
        self.points = points
        self.color = color          # (r, g, b) 0..1
        self.width = float(width)
        self.height = float(height)
        self.tip = tip
        self.rotation = float(rotation)
        self.highlighter = bool(highlighter)
        self._bbox = None
        self._poly_cache = None

    def copy_with_points(self, points):
        return Stroke(points, self.color, self.width, self.height, self.tip,
                      self.rotation, self.highlighter)

    @property
    def radius(self):
        """Ucu çevreleyen dairenin yarıçapı (basınç en fazla 2 kat)."""
        return 0.5 * math.hypot(self.width, self.height)

    def bbox(self):
        if self._bbox is None:
            xs = [p[0] for p in self.points]
            ys = [p[1] for p in self.points]
            r = self.radius * 2.0 + 1.0
            self._bbox = (min(xs) - r, min(ys) - r, max(xs) + r, max(ys) + r)
        return self._bbox

    def tip_polygon(self):
        """Ucun merkez etrafındaki çokgen hali (basınç=0.5 için)."""
        if self._poly_cache is None:
            self._poly_cache = tip_polygon(self.tip, self.width, self.height,
                                           self.rotation)
        return self._poly_cache


def tip_polygon(tip, width, height, rotation):
    w2, h2 = width / 2.0, height / 2.0
    if tip == TIP_RECT:
        pts = [(-w2, -h2), (w2, -h2), (w2, h2), (-w2, h2)]
    else:
        n = 20
        pts = [(w2 * math.cos(2 * math.pi * i / n),
                h2 * math.sin(2 * math.pi * i / n)) for i in range(n)]
    if rotation:
        a = math.radians(rotation)
        ca, sa = math.cos(a), math.sin(a)
        pts = [(x * ca - y * sa, x * sa + y * ca) for x, y in pts]
    return pts


def pressure_factor(p):
    # WPF: basınç 0.5 iken nominal kalınlık. Fare her zaman 0.5 verir.
    return max(0.35, min(1.6, p / 0.5))


def densify(points, spacing=MAX_POINT_SPACING):
    """İki nokta arası `spacing`'den uzunsa araya nokta ekler."""
    if len(points) < 2:
        return list(points)
    out = [points[0]]
    for q in points[1:]:
        p = out[-1]
        d = math.hypot(q[0] - p[0], q[1] - p[1])
        if d > spacing:
            n = int(d // spacing)
            for i in range(1, n + 1):
                t = i / (n + 1)
                out.append((p[0] + (q[0] - p[0]) * t,
                            p[1] + (q[1] - p[1]) * t,
                            p[2] + (q[2] - p[2]) * t))
        out.append(q)
    return out


def _convex_hull(pts):
    pts = sorted(set(pts))
    if len(pts) <= 2:
        return pts

    def cross(o, a, b):
        return (a[0] - o[0]) * (b[1] - o[1]) - (a[1] - o[1]) * (b[0] - o[0])

    lower = []
    for p in pts:
        while len(lower) >= 2 and cross(lower[-2], lower[-1], p) <= 0:
            lower.pop()
        lower.append(p)
    upper = []
    for p in reversed(pts):
        while len(upper) >= 2 and cross(upper[-2], upper[-1], p) <= 0:
            upper.pop()
        upper.append(p)
    return lower[:-1] + upper[:-1]


def _is_round(stroke):
    return (stroke.tip == TIP_ELLIPSE
            and abs(stroke.width - stroke.height) < 0.01)


def _uniform_pressure(points):
    first = points[0][2]
    return all(abs(p[2] - first) < 0.02 for p in points)


def draw_segments(cr, stroke, start_index=0):
    """Çizginin `start_index`'ten itibaren olan kısmını opak olarak çizer.

    Renk/saydamlık çağıran tarafından ayarlanmış olmalıdır. Canlı çizimde
    yalnızca yeni eklenen parçalar için çağrılır.
    """
    pts = stroke.points
    if not pts:
        return
    i0 = max(0, start_index - 1)
    seg = pts[i0:]

    if _is_round(stroke):
        if _uniform_pressure(seg):
            cr.set_line_width(max(0.5, stroke.width * pressure_factor(seg[0][2])))
            cr.set_line_cap(1)   # ROUND
            cr.set_line_join(1)  # ROUND
            cr.move_to(seg[0][0], seg[0][1])
            if len(seg) == 1:
                cr.line_to(seg[0][0] + 0.01, seg[0][1])
            for p in seg[1:]:
                cr.line_to(p[0], p[1])
            cr.stroke()
            return
        cr.set_line_cap(1)
        if len(seg) == 1:
            r = stroke.width * pressure_factor(seg[0][2]) / 2.0
            cr.arc(seg[0][0], seg[0][1], max(0.25, r), 0, 2 * math.pi)
            cr.fill()
            return
        for a, b in zip(seg, seg[1:]):
            cr.set_line_width(max(0.5, stroke.width * pressure_factor((a[2] + b[2]) / 2)))
            cr.move_to(a[0], a[1])
            cr.line_to(b[0], b[1])
            cr.stroke()
        return

    # Elips/dikdörtgen uç: her parça için ucun iki ucundaki kopyalarının
    # dışbükey zarfı doldurulur (Minkowski toplamı).
    poly = stroke.tip_polygon()
    if len(seg) == 1:
        f = pressure_factor(seg[0][2])
        x, y = seg[0][0], seg[0][1]
        cr.move_to(x + poly[0][0] * f, y + poly[0][1] * f)
        for px, py in poly[1:]:
            cr.line_to(x + px * f, y + py * f)
        cr.close_path()
        cr.fill()
        return
    for a, b in zip(seg, seg[1:]):
        fa, fb = pressure_factor(a[2]), pressure_factor(b[2])
        hull = _convex_hull(
            [(a[0] + px * fa, a[1] + py * fa) for px, py in poly]
            + [(b[0] + px * fb, b[1] + py * fb) for px, py in poly])
        cr.move_to(*hull[0])
        for h in hull[1:]:
            cr.line_to(*h)
        cr.close_path()
        cr.fill()


def render_stroke(cr, stroke):
    r, g, b = stroke.color
    if stroke.highlighter:
        cr.push_group()
        cr.set_source_rgb(r, g, b)
        draw_segments(cr, stroke)
        cr.pop_group_to_source()
        cr.paint_with_alpha(0.5)
    else:
        cr.set_source_rgb(r, g, b)
        draw_segments(cr, stroke)


def render_strokes(cr, strokes, clip=None):
    """Fosforlu kalemler alta, diğerleri üste çizilir (WPF gibi)."""
    def visible(s):
        if clip is None:
            return True
        x0, y0, x1, y1 = s.bbox()
        cx, cy, cw, ch = clip
        return not (x1 < cx or y1 < cy or x0 > cx + cw or y0 > cy + ch)

    for s in strokes:
        if s.highlighter and visible(s):
            render_stroke(cr, s)
    for s in strokes:
        if not s.highlighter and visible(s):
            render_stroke(cr, s)


# ---------------------------------------------------------------- silgi ----

class Eraser:
    """Döndürülmüş dikdörtgen silgi (orijinalde açı 170 derece)."""

    def __init__(self, width, height, angle=170.0):
        self.width = float(width)
        self.height = float(height)
        self.angle = math.radians(angle)
        self._ca = math.cos(self.angle)
        self._sa = math.sin(self.angle)

    def outline(self, cx, cy):
        w2, h2 = self.width / 2.0, self.height / 2.0
        pts = []
        for x, y in ((-w2, -h2), (w2, -h2), (w2, h2), (-w2, h2)):
            pts.append((cx + x * self._ca - y * self._sa,
                        cy + x * self._sa + y * self._ca))
        return pts

    def contains(self, cx, cy, x, y, margin=0.0):
        dx, dy = x - cx, y - cy
        # Ters döndürüp eksen hizalı dikdörtgende test et.
        lx = dx * self._ca + dy * self._sa
        ly = -dx * self._sa + dy * self._ca
        return (abs(lx) <= self.width / 2.0 + margin
                and abs(ly) <= self.height / 2.0 + margin)

    def reach(self):
        return 0.5 * math.hypot(self.width, self.height)

    def erase(self, strokes, centers):
        """`centers` boyunca silgiyi geçirir.

        Dönüş: (yeni_liste, değişti_mi, kirli_bölge)
        """
        reach = self.reach()
        xs = [c[0] for c in centers]
        ys = [c[1] for c in centers]
        ex0, ey0 = min(xs) - reach, min(ys) - reach
        ex1, ey1 = max(xs) + reach, max(ys) + reach

        out = []
        changed = False
        dirty = None
        for s in strokes:
            x0, y0, x1, y1 = s.bbox()
            if x1 < ex0 or y1 < ey0 or x0 > ex1 or y0 > ey1:
                out.append(s)
                continue
            margin = min(s.width, s.height) / 2.0
            keep = []
            for p in s.points:
                hit = False
                for cx, cy in centers:
                    if (abs(p[0] - cx) <= reach + margin
                            and abs(p[1] - cy) <= reach + margin
                            and self.contains(cx, cy, p[0], p[1], margin)):
                        hit = True
                        break
                keep.append(not hit)
            if all(keep):
                out.append(s)
                continue
            changed = True
            dirty = _union(dirty, s.bbox())
            run = []
            for p, k in zip(s.points, keep):
                if k:
                    run.append(p)
                elif run:
                    out.append(s.copy_with_points(run))
                    run = []
            if run:
                out.append(s.copy_with_points(run))
        return out, changed, dirty


def _union(a, b):
    if a is None:
        return b
    return (min(a[0], b[0]), min(a[1], b[1]), max(a[2], b[2]), max(a[3], b[3]))


def interpolate_centers(p0, p1, step=4.0):
    """Hızlı silgi hareketinde boşluk kalmaması için ara noktalar."""
    if p0 is None:
        return [p1]
    d = math.hypot(p1[0] - p0[0], p1[1] - p0[1])
    n = max(1, int(d // step))
    return [(p0[0] + (p1[0] - p0[0]) * i / n, p0[1] + (p1[1] - p0[1]) * i / n)
            for i in range(1, n + 1)]
