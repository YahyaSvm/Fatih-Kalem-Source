"""Çizgi işlemleri: yumuşatma, şekil tanıma ve kesikli / noktalı / oklu kalem."""

import math

from .ink import Stroke, densify


def path_length(pts):
    return sum(math.hypot(b[0] - a[0], b[1] - a[1]) for a, b in zip(pts, pts[1:]))


# -------------------------------------------------------------- yumuşatma --

def smooth(points, passes=2):
    """Chaikin köşe kesme; uçlar korunur, titrek el yazısı düzelir."""
    pts = list(points)
    if len(pts) < 4:
        return pts
    # Önce seyrelt (2 px aralıklı noktalar fazla titrer)
    thin = [pts[0]]
    for p in pts[1:-1]:
        if math.hypot(p[0] - thin[-1][0], p[1] - thin[-1][1]) >= 4.0:
            thin.append(p)
    thin.append(pts[-1])
    pts = thin
    for _ in range(passes):
        if len(pts) < 3:
            break
        out = [pts[0]]
        for a, b in zip(pts, pts[1:]):
            out.append((0.75 * a[0] + 0.25 * b[0], 0.75 * a[1] + 0.25 * b[1],
                        0.75 * a[2] + 0.25 * b[2]))
            out.append((0.25 * a[0] + 0.75 * b[0], 0.25 * a[1] + 0.75 * b[1],
                        0.25 * a[2] + 0.75 * b[2]))
        out.append(pts[-1])
        pts = out
    return densify(pts)


# ----------------------------------------------------------- şekil tanıma --

def _perp_dist(p, a, b):
    dx, dy = b[0] - a[0], b[1] - a[1]
    n = math.hypot(dx, dy)
    if n < 1e-9:
        return math.hypot(p[0] - a[0], p[1] - a[1])
    return abs(dy * p[0] - dx * p[1] + b[0] * a[1] - b[1] * a[0]) / n


def _rdp(pts, eps):
    if len(pts) < 3:
        return list(pts)
    a, b = pts[0], pts[-1]
    idx, dmax = 0, 0.0
    for i in range(1, len(pts) - 1):
        d = _perp_dist(pts[i], a, b)
        if d > dmax:
            idx, dmax = i, d
    if dmax > eps:
        left = _rdp(pts[:idx + 1], eps)
        right = _rdp(pts[idx:], eps)
        return left[:-1] + right
    return [a, b]


def _merge_close_corners(corners, min_dist):
    out = []
    for c in corners:
        if not out or math.hypot(c[0] - out[-1][0], c[1] - out[-1][1]) >= min_dist:
            out.append(c)
    if len(out) > 2 and math.hypot(out[0][0] - out[-1][0], out[0][1] - out[-1][1]) < min_dist:
        out.pop()
    return out


def recognize(points):
    """El çizimini tanır.

    Dönüş: ("line", [p0, p1]) | ("ellipse", (cx, cy, rx, ry)) |
           ("polygon", [köşeler]) | None
    """
    pts = [(p[0], p[1]) for p in points]
    if len(pts) < 5:
        return None
    length = path_length(pts)
    if length < 40:
        return None
    start, end = pts[0], pts[-1]
    gap = math.hypot(end[0] - start[0], end[1] - start[1])

    # Çizgi: tüm noktalar uç noktaları birleştiren doğruya yakın
    if gap > 0.8 * length:
        dev = max(_perp_dist(p, start, end) for p in pts)
        if dev < max(6.0, 0.05 * length):
            return ("line", [start, end])
        return None

    if gap > 0.25 * length:
        return None          # açık eğri: tanınmaz

    xs = [p[0] for p in pts]
    ys = [p[1] for p in pts]
    x0, x1, y0, y1 = min(xs), max(xs), min(ys), max(ys)
    w, h = x1 - x0, y1 - y0
    if w < 20 or h < 20:
        return None

    # Çokgen: köşeleri bul
    eps = 0.045 * length
    simplified = _rdp(pts + [pts[0]], eps)
    corners = _merge_close_corners(simplified[:-1], 0.12 * min(w, h) + 8)

    # Elips: merkeze uzaklık normalize edilince ~1
    cx, cy, rx, ry = (x0 + x1) / 2.0, (y0 + y1) / 2.0, w / 2.0, h / 2.0
    ell_err = sum(abs(((x - cx) / rx) ** 2 + ((y - cy) / ry) ** 2 - 1.0) for x, y in pts) / len(pts)

    # Çokgen uyumu: noktaların köşeleri birleştiren kenarlara ortalama uzaklığı
    poly_err = None
    if len(corners) in (3, 4):
        edges = list(zip(corners, corners[1:] + corners[:1]))
        total = 0.0
        for p in pts:
            total += min(_seg_dist(p, a, b) for a, b in edges)
        poly_err = total / len(pts) / (0.5 * (w + h))

    if poly_err is not None and poly_err < 0.035:
        if len(corners) == 3:
            return ("polygon", corners)
        # Neredeyse eksenlere paralel dikdörtgen ise düzgün dikdörtgen yap
        axis = True
        for a, b in zip(corners, corners[1:] + corners[:1]):
            ang = abs(math.degrees(math.atan2(b[1] - a[1], b[0] - a[0]))) % 90
            if 12 < ang < 78:
                axis = False
        if axis:
            return ("polygon", [(x0, y0), (x1, y0), (x1, y1), (x0, y1)])
        return ("polygon", corners)
    if ell_err < 0.25:
        return ("ellipse", (cx, cy, rx, ry))
    return None


def _seg_dist(p, a, b):
    dx, dy = b[0] - a[0], b[1] - a[1]
    L2 = dx * dx + dy * dy or 1e-9
    t = max(0.0, min(1.0, ((p[0] - a[0]) * dx + (p[1] - a[1]) * dy) / L2))
    return math.hypot(p[0] - (a[0] + t * dx), p[1] - (a[1] + t * dy))


def recognized_strokes(stroke):
    """Tanınırsa çizginin yerini alacak düzgün çizgileri döndürür."""
    shape = recognize(stroke.points)
    if shape is None:
        return None
    kind, data = shape
    if kind == "line":
        pts = data
    elif kind == "polygon":
        pts = list(data) + [data[0]]
    else:
        cx, cy, rx, ry = data
        n = max(36, int((rx + ry) / 3))
        pts = [(cx + rx * math.cos(2 * math.pi * i / n), cy + ry * math.sin(2 * math.pi * i / n))
               for i in range(n + 1)]
    p = 0.5
    return [stroke.copy_with_points(densify([(x, y, p) for x, y in pts]))]


# ----------------------------------------------- kesikli / noktalı / oklu --

def split_pattern(stroke, on, off):
    """Çizgiyi uzunluk boyunca açık/kapalı aralıklara böler."""
    pts = densify(stroke.points, 1.0)
    out = []
    run = [pts[0]]
    pos, drawing = 0.0, True
    for a, b in zip(pts, pts[1:]):
        pos += math.hypot(b[0] - a[0], b[1] - a[1])
        limit = on if drawing else off
        if pos >= limit:
            if drawing:
                run.append(b)
                out.append(stroke.copy_with_points(run))
            run = [b]
            pos = 0.0
            drawing = not drawing
        elif drawing:
            run.append(b)
    if drawing and len(run) > 1:
        out.append(stroke.copy_with_points(run))
    return out or [stroke]


def dashed(stroke):
    w = max(stroke.width, stroke.height)
    on = max(12.0, 4.0 * w)
    return split_pattern(stroke, on, on * 0.8)


def dotted(stroke):
    w = max(stroke.width, stroke.height)
    return split_pattern(stroke, max(0.6, w * 0.15), max(6.0, 2.2 * w))


def with_arrow(stroke):
    """Serbest çizginin sonuna ok başı ekler."""
    pts = stroke.points
    if len(pts) < 2:
        return [stroke]
    end = pts[-1]
    # Son ~20 px'lik bölümden yönü al
    back = pts[0]
    acc = 0.0
    for a, b in zip(reversed(pts[:-1]), reversed(pts[1:])):
        acc += math.hypot(b[0] - a[0], b[1] - a[1])
        back = a
        if acc > 20:
            break
    theta = math.atan2(end[1] - back[1], end[0] - back[0])
    w = max(stroke.width, stroke.height)
    length = max(16.0, 4.5 * w)
    head = []
    for sign in (1, -1):
        a = theta + math.pi - sign * math.radians(28)
        head.append((end[0] + length * math.cos(a), end[1] + length * math.sin(a), end[2]))
    head_stroke = Stroke(densify([head[0], end, head[1]]), stroke.color, stroke.width,
                         stroke.height, stroke.tip, stroke.rotation, stroke.highlighter)
    return [stroke, head_stroke]


def chunk(stroke, max_points=1500):
    """Çok uzun çizgileri parçalar: silgi ve çizim hızlanır."""
    pts = stroke.points
    if len(pts) <= max_points:
        return [stroke]
    out = []
    for i in range(0, len(pts) - 1, max_points):
        part = pts[i:i + max_points + 1]
        if len(part) > 1:
            out.append(stroke.copy_with_points(part))
    return out
