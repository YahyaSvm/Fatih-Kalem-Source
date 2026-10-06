"""Ekran üstü ders araçları.

Her araç bir "katman"dır: çizilir, dokunmayı yakalayabilir, taşınır,
döndürülür ve sağ üstteki (x) ile kapatılır.

  * Cetvel, Gönye, İletki: kenarlarına yakın başlayan kalem çizgileri o
    kenara yapışır (düz çizgi).
  * Pergel: merkez, yarıçap tutamağı ve kalem tutamağı; kalem tutamağı
    çevrildikçe yay çizilir.
  * Spot ışığı, Büyüteç, Sayaç / Kronometre.
"""

import math
import time

import cairo

from .i18n import _

CM = 37.8          # 96 dpi'de 1 cm
HANDLE = 22        # tutamak yarıçapı (dokunmatik için büyük)
ORANGE = (0xF0 / 255.0, 0x5A / 255.0, 0x28 / 255.0)


def _rot(x, y, a):
    ca, sa = math.cos(a), math.sin(a)
    return x * ca - y * sa, x * sa + y * ca


def _close_button(cr, x, y, r=14):
    cr.save()
    cr.arc(x, y, r, 0, 2 * math.pi)
    cr.set_source_rgba(0.85, 0.15, 0.15, 0.92)
    cr.fill()
    cr.set_source_rgb(1, 1, 1)
    cr.set_line_width(2.5)
    d = r * 0.45
    cr.move_to(x - d, y - d)
    cr.line_to(x + d, y + d)
    cr.move_to(x + d, y - d)
    cr.line_to(x - d, y + d)
    cr.stroke()
    cr.restore()


def _handle(cr, x, y, icon="rotate"):
    cr.save()
    cr.arc(x, y, HANDLE * 0.8, 0, 2 * math.pi)
    cr.set_source_rgba(1, 1, 1, 0.95)
    cr.fill_preserve()
    cr.set_source_rgb(*ORANGE)
    cr.set_line_width(2.5)
    cr.stroke()
    cr.set_line_width(2.5)
    if icon == "rotate":
        cr.arc(x, y, HANDLE * 0.42, -0.4, 4.2)
        cr.stroke()
        ex, ey = x + HANDLE * 0.42 * math.cos(4.2), y + HANDLE * 0.42 * math.sin(4.2)
        cr.move_to(ex - 5, ey - 1)
        cr.line_to(ex, ey)
        cr.line_to(ex + 1, ey - 6)
        cr.stroke()
    elif icon == "pen":
        cr.move_to(x - 7, y + 7)
        cr.line_to(x + 7, y - 7)
        cr.stroke()
        cr.arc(x - 7, y + 7, 2.5, 0, 2 * math.pi)
        cr.fill()
    else:  # yarıçap
        cr.move_to(x - 8, y)
        cr.line_to(x + 8, y)
        cr.move_to(x - 4, y - 4)
        cr.line_to(x - 8, y)
        cr.line_to(x - 4, y + 4)
        cr.move_to(x + 4, y - 4)
        cr.line_to(x + 8, y)
        cr.line_to(x + 4, y + 4)
        cr.stroke()
    cr.restore()


def _project(px, py, ax, ay, bx, by):
    """P'nin AB doğru parçasına izdüşümü ve uzaklığı."""
    dx, dy = bx - ax, by - ay
    L2 = dx * dx + dy * dy or 1e-9
    t = max(0.0, min(1.0, ((px - ax) * dx + (py - ay) * dy) / L2))
    qx, qy = ax + t * dx, ay + t * dy
    return (qx, qy), math.hypot(px - qx, py - qy), t


class Overlay:
    kind = "overlay"
    takes_input_everywhere = False   # spot ışığı tüm ekranı kaplar

    def __init__(self):
        self.mode = None
        self.grab = None
        self.closed = False

    # alt sınıflar: draw, hit, press, move, release, bbox
    def bbox(self):
        return (0, 0, 0, 0)

    def snap_edges(self):
        """Kalemin yapışacağı kenarlar: [(ax, ay, bx, by)]."""
        return []

    EDGE_ZONE = 18.0

    def in_edge_zone(self, x, y):
        """Nokta bir kenara (içeriden ya da dışarıdan) çok yakın mı?"""
        for edge in self.snap_edges():
            (_q, d, t) = _project(x, y, *edge)
            if d <= self.EDGE_ZONE and 0.0 < t < 1.0:
                return True
        return False


class Instrument(Overlay):
    """Taşınabilir / döndürülebilir ölçme aracı (yerel koordinat sistemi)."""

    def __init__(self, cx, cy):
        super().__init__()
        self.cx, self.cy = float(cx), float(cy)
        self.angle = 0.0

    def to_local(self, x, y):
        return _rot(x - self.cx, y - self.cy, -self.angle)

    def to_world(self, lx, ly):
        x, y = _rot(lx, ly, self.angle)
        return x + self.cx, y + self.cy

    def local_shape(self):
        raise NotImplementedError

    def rotate_handle(self):
        raise NotImplementedError

    def close_pos(self):
        raise NotImplementedError

    def contains_local(self, lx, ly):
        raise NotImplementedError

    def hit(self, x, y):
        lx, ly = self.to_local(x, y)
        for (hx, hy) in (self.rotate_handle(), self.close_pos()):
            if math.hypot(lx - hx, ly - hy) <= HANDLE + 4:
                return True
        return self.contains_local(lx, ly)

    def press(self, x, y):
        lx, ly = self.to_local(x, y)
        cx_, cy_ = self.close_pos()
        if math.hypot(lx - cx_, ly - cy_) <= HANDLE:
            self.closed = True
            return
        hx, hy = self.rotate_handle()
        if math.hypot(lx - hx, ly - hy) <= HANDLE + 4:
            self.mode = "rotate"
            self.grab = (math.atan2(y - self.cy, x - self.cx), self.angle)
        else:
            self.mode = "move"
            self.grab = (x - self.cx, y - self.cy)

    def move(self, x, y):
        if self.mode == "move":
            self.cx, self.cy = x - self.grab[0], y - self.grab[1]
        elif self.mode == "rotate":
            a0, ang0 = self.grab
            a = ang0 + math.atan2(y - self.cy, x - self.cx) - a0
            # 15 dereceye yakınsa oraya otur (düz açılar kolay tutturulsun)
            deg = math.degrees(a)
            near = round(deg / 15.0) * 15.0
            if abs(deg - near) < 2.0:
                a = math.radians(near)
            self.angle = a

    def release(self, x, y):
        self.mode = None

    def bbox(self):
        pts = [self.to_world(*p) for p in self.local_shape()]
        for p in (self.rotate_handle(), self.close_pos()):
            pts.append(self.to_world(*p))
        xs = [p[0] for p in pts]
        ys = [p[1] for p in pts]
        m = HANDLE + 6
        return (min(xs) - m, min(ys) - m, max(xs) + m, max(ys) + m)

    def _path(self, cr):
        pts = self.local_shape()
        cr.move_to(*pts[0])
        for p in pts[1:]:
            cr.line_to(*p)
        cr.close_path()

    def _body(self, cr):
        self._path(cr)
        cr.set_source_rgba(0.93, 0.97, 1.0, 0.72)
        cr.fill_preserve()
        cr.set_source_rgba(0.1, 0.35, 0.65, 0.9)
        cr.set_line_width(1.5)
        cr.stroke()


class Ruler(Instrument):
    kind = "ruler"
    LENGTH = 20 * CM + 40
    WIDTH = 2.2 * CM

    def local_shape(self):
        L, W = self.LENGTH / 2.0, self.WIDTH / 2.0
        return [(-L, -W), (L, -W), (L, W), (-L, W)]

    def contains_local(self, lx, ly):
        return abs(lx) <= self.LENGTH / 2.0 and abs(ly) <= self.WIDTH / 2.0

    def rotate_handle(self):
        return (self.LENGTH / 2.0 + HANDLE + 4, 0)

    def close_pos(self):
        return (-self.LENGTH / 2.0 - HANDLE - 4, 0)

    def snap_edges(self):
        L, W = self.LENGTH / 2.0, self.WIDTH / 2.0
        a = self.to_world(-L, -W) + self.to_world(L, -W)
        b = self.to_world(-L, W) + self.to_world(L, W)
        return [a, b]

    def draw(self, cr):
        cr.save()
        cr.translate(self.cx, self.cy)
        cr.rotate(self.angle)
        self._body(cr)
        L, W = self.LENGTH / 2.0, self.WIDTH / 2.0
        x0 = -L + 20
        cr.set_source_rgba(0.1, 0.2, 0.35, 0.95)
        cr.set_line_width(1)
        cr.select_font_face("Sans")
        cr.set_font_size(11)
        for mm in range(0, 201):
            x = x0 + mm * CM / 10.0
            h = 14 if mm % 10 == 0 else (9 if mm % 5 == 0 else 5)
            cr.move_to(x, -W)
            cr.line_to(x, -W + h)
            if mm % 10 == 0:
                label = str(mm // 10)
                ext = cr.text_extents(label)
                cr.move_to(x - ext.width / 2.0 - ext.x_bearing, -W + 28)
                cr.show_text(label)
        cr.stroke()
        cr.restore()
        self._draw_handles(cr)

    def _draw_handles(self, cr):
        hx, hy = self.to_world(*self.rotate_handle())
        _handle(cr, hx, hy, "rotate")
        cx_, cy_ = self.to_world(*self.close_pos())
        _close_button(cr, cx_, cy_)
        # Açı göstergesi
        deg = math.degrees(self.angle) % 360
        cr.save()
        cr.select_font_face("Sans", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_BOLD)
        cr.set_font_size(13)
        cr.set_source_rgba(0.1, 0.2, 0.35, 0.9)
        cr.move_to(hx + HANDLE, hy + 5)
        cr.show_text("%d°" % round(deg))
        cr.restore()


class SetSquare(Instrument):
    """45-45-90 gönye."""
    kind = "setsquare"
    LEG = 12 * CM

    def local_shape(self):
        s = self.LEG
        return [(0, 0), (s, 0), (0, s)]

    def contains_local(self, lx, ly):
        return lx >= 0 and ly >= 0 and lx + ly <= self.LEG

    def rotate_handle(self):
        return (self.LEG + HANDLE + 4, 0)

    def close_pos(self):
        return (-HANDLE - 4, -HANDLE - 4)

    def snap_edges(self):
        s = self.LEG
        p0, p1, p2 = self.to_world(0, 0), self.to_world(s, 0), self.to_world(0, s)
        return [p0 + p1, p0 + p2, p1 + p2]

    def draw(self, cr):
        cr.save()
        cr.translate(self.cx, self.cy)
        cr.rotate(self.angle)
        self._body(cr)
        cr.set_source_rgba(0.1, 0.2, 0.35, 0.95)
        cr.set_line_width(1)
        for mm in range(0, int(self.LEG / CM * 10) + 1):
            d = mm * CM / 10.0
            h = 12 if mm % 10 == 0 else (8 if mm % 5 == 0 else 4)
            cr.move_to(d, 0)
            cr.line_to(d, h)
            cr.move_to(0, d)
            cr.line_to(h, d)
        cr.stroke()
        # iç boşluk
        s = self.LEG
        cr.move_to(0.25 * s, 0.2 * s)
        cr.line_to(0.55 * s, 0.2 * s)
        cr.line_to(0.25 * s, 0.5 * s)
        cr.close_path()
        cr.set_source_rgba(0.1, 0.35, 0.65, 0.6)
        cr.stroke()
        cr.restore()
        Ruler._draw_handles(self, cr)


class Protractor(Instrument):
    """180 derecelik iletki; merkezi (cx, cy), düz kenarı yerel x ekseni."""
    kind = "protractor"
    R = 7.5 * CM

    def local_shape(self):
        n = 60
        pts = [(self.R * math.cos(math.pi + math.pi * i / n),
                self.R * math.sin(math.pi + math.pi * i / n)) for i in range(n + 1)]
        return pts

    def contains_local(self, lx, ly):
        return ly <= 4 and math.hypot(lx, ly) <= self.R

    def rotate_handle(self):
        return (self.R + HANDLE + 4, 0)

    def close_pos(self):
        return (-self.R - HANDLE - 4, 0)

    def snap_edges(self):
        return [self.to_world(-self.R, 0) + self.to_world(self.R, 0)]

    def draw(self, cr):
        cr.save()
        cr.translate(self.cx, self.cy)
        cr.rotate(self.angle)
        self._body(cr)
        R = self.R
        cr.set_source_rgba(0.1, 0.2, 0.35, 0.95)
        cr.set_line_width(1)
        cr.select_font_face("Sans")
        cr.set_font_size(10)
        for d in range(0, 181):
            a = math.pi + math.radians(d)
            h = 16 if d % 10 == 0 else (10 if d % 5 == 0 else 5)
            cr.move_to(R * math.cos(a), R * math.sin(a))
            cr.line_to((R - h) * math.cos(a), (R - h) * math.sin(a))
            if d % 10 == 0:
                for label, rr in ((str(d), R - 28), (str(180 - d), R - 44)):
                    ext = cr.text_extents(label)
                    cr.move_to(rr * math.cos(a) - ext.width / 2.0,
                               rr * math.sin(a) + ext.height / 2.0)
                    cr.show_text(label)
        cr.stroke()
        cr.arc(0, 0, 4, 0, 2 * math.pi)
        cr.fill()
        cr.move_to(-R, 0)
        cr.line_to(R, 0)
        cr.stroke()
        cr.restore()
        Ruler._draw_handles(self, cr)


class Compass(Overlay):
    """Pergel: merkez + yarıçap; kalem tutamağı çevrildikçe yay çizer."""
    kind = "compass"

    def __init__(self, cx, cy, radius=150):
        super().__init__()
        self.cx, self.cy = float(cx), float(cy)
        self.r = float(radius)
        self.a = 0.0
        self.arc_points = None
        self.on_arc = None     # pencere atar: (noktalar, bitti_mi)

    def pen_pos(self):
        return (self.cx + self.r * math.cos(self.a), self.cy + self.r * math.sin(self.a))

    def radius_pos(self):
        b = self.a + math.radians(30)
        return (self.cx + self.r * 0.55 * math.cos(b), self.cy + self.r * 0.55 * math.sin(b))

    def close_pos(self):
        return (self.cx - 34, self.cy - 34)

    def hit(self, x, y):
        for p in (self.pen_pos(), self.radius_pos(), (self.cx, self.cy), self.close_pos()):
            if math.hypot(x - p[0], y - p[1]) <= HANDLE + 6:
                return True
        return False

    def press(self, x, y):
        if math.hypot(x - self.close_pos()[0], y - self.close_pos()[1]) <= HANDLE:
            self.closed = True
            return
        px, py = self.pen_pos()
        rx, ry = self.radius_pos()
        if math.hypot(x - px, y - py) <= HANDLE + 6:
            self.mode = "draw"
            self.arc_points = [(px, py)]
            self.grab = self.a
        elif math.hypot(x - rx, y - ry) <= HANDLE + 6:
            self.mode = "radius"
        else:
            self.mode = "move"
            self.grab = (x - self.cx, y - self.cy)

    def move(self, x, y):
        if self.mode == "move":
            self.cx, self.cy = x - self.grab[0], y - self.grab[1]
        elif self.mode == "radius":
            # Tutamak yarıçapın %55'inde durur
            self.r = max(20.0, math.hypot(x - self.cx, y - self.cy) / 0.55)
            self.a = math.atan2(y - self.cy, x - self.cx) - math.radians(30)
        elif self.mode == "draw":
            target = math.atan2(y - self.cy, x - self.cx)
            # Açıyı sürekli tut (±π sıçramasını önle)
            while target - self.a > math.pi:
                target -= 2 * math.pi
            while target - self.a < -math.pi:
                target += 2 * math.pi
            steps = max(1, int(abs(target - self.a) * self.r / 3.0))
            for i in range(1, steps + 1):
                ang = self.a + (target - self.a) * i / steps
                self.arc_points.append((self.cx + self.r * math.cos(ang),
                                        self.cy + self.r * math.sin(ang)))
            self.a = target
            if self.on_arc:
                self.on_arc(self.arc_points, False)

    def release(self, x, y):
        if self.mode == "draw" and self.on_arc and self.arc_points and len(self.arc_points) > 2:
            self.on_arc(self.arc_points, True)
        self.arc_points = None
        self.mode = None

    def bbox(self):
        m = HANDLE + 40
        return (self.cx - self.r - m, self.cy - self.r - m, self.cx + self.r + m, self.cy + self.r + m)

    def draw(self, cr):
        px, py = self.pen_pos()
        cr.save()
        cr.set_source_rgba(0.1, 0.35, 0.65, 0.35)
        cr.set_dash([6, 6])
        cr.set_line_width(1.5)
        cr.arc(self.cx, self.cy, self.r, 0, 2 * math.pi)
        cr.stroke()
        cr.set_dash([])
        # kollar
        top = (self.cx + (px - self.cx) / 2.0 - (py - self.cy) * 0.35,
               self.cy + (py - self.cy) / 2.0 + (px - self.cx) * 0.35)
        cr.set_source_rgba(0.3, 0.3, 0.35, 0.9)
        cr.set_line_width(6)
        cr.set_line_cap(cairo.LINE_CAP_ROUND)
        cr.move_to(self.cx, self.cy)
        cr.line_to(*top)
        cr.line_to(px, py)
        cr.stroke()
        cr.arc(top[0], top[1], 8, 0, 2 * math.pi)
        cr.set_source_rgb(*ORANGE)
        cr.fill()
        cr.arc(self.cx, self.cy, 6, 0, 2 * math.pi)
        cr.set_source_rgb(0.15, 0.15, 0.15)
        cr.fill()
        cr.restore()
        _handle(cr, px, py, "pen")
        rx, ry = self.radius_pos()
        _handle(cr, rx, ry, "radius")
        _close_button(cr, *self.close_pos())
        cr.save()
        cr.set_font_size(13)
        cr.set_source_rgba(0.1, 0.2, 0.35, 0.9)
        cr.move_to(rx + HANDLE, ry - HANDLE)
        cr.show_text("r = %.1f cm" % (self.r / CM))
        cr.restore()


class Spotlight(Overlay):
    """Ekranı karartıp yalnızca bir daireyi aydınlatır."""
    kind = "spotlight"
    takes_input_everywhere = True

    def __init__(self, W, H):
        super().__init__()
        self.W, self.H = W, H
        self.x, self.y = W / 2.0, H / 2.0
        self.r = min(W, H) / 5.0

    def close_pos(self):
        a = math.radians(-45)
        return (self.x + (self.r + 26) * math.cos(a), self.y + (self.r + 26) * math.sin(a))

    def resize_pos(self):
        a = math.radians(45)
        return (self.x + self.r * math.cos(a), self.y + self.r * math.sin(a))

    def hit(self, x, y):
        return True

    def press(self, x, y):
        cx_, cy_ = self.close_pos()
        if math.hypot(x - cx_, y - cy_) <= HANDLE:
            self.closed = True
            return
        rx, ry = self.resize_pos()
        if math.hypot(x - rx, y - ry) <= HANDLE + 4:
            self.mode = "resize"
        else:
            self.mode = "move"
            self.grab = (x - self.x, y - self.y)

    def move(self, x, y):
        if self.mode == "move":
            self.x, self.y = x - self.grab[0], y - self.grab[1]
        elif self.mode == "resize":
            self.r = max(50.0, math.hypot(x - self.x, y - self.y))

    def release(self, x, y):
        self.mode = None

    def bbox(self):
        return (0, 0, self.W, self.H)

    def draw(self, cr):
        cr.save()
        cr.set_fill_rule(cairo.FILL_RULE_EVEN_ODD)
        cr.rectangle(0, 0, self.W, self.H)
        cr.arc(self.x, self.y, self.r, 0, 2 * math.pi)
        cr.set_source_rgba(0, 0, 0, 0.82)
        cr.fill()
        cr.restore()
        cr.save()
        cr.arc(self.x, self.y, self.r, 0, 2 * math.pi)
        cr.set_source_rgba(1, 1, 0.8, 0.6)
        cr.set_line_width(3)
        cr.stroke()
        cr.restore()
        _handle(cr, *self.resize_pos(), icon="radius")
        _close_button(cr, *self.close_pos())


class Magnifier(Overlay):
    """Büyüteç: kaynak yüzeyden (ekran görüntüsü + çizimler) yakınlaştırır."""
    kind = "magnifier"
    ZOOM = 2.5

    def __init__(self, x, y, source):
        super().__init__()
        self.x, self.y = float(x), float(y)
        self.r = 170.0
        self.source = source      # çağrılabilir: [cairo yüzeyi, ...] döndürür

    def close_pos(self):
        a = math.radians(-45)
        return (self.x + (self.r + 22) * math.cos(a), self.y + (self.r + 22) * math.sin(a))

    def hit(self, x, y):
        cx_, cy_ = self.close_pos()
        return (math.hypot(x - self.x, y - self.y) <= self.r + 8
                or math.hypot(x - cx_, y - cy_) <= HANDLE)

    def press(self, x, y):
        cx_, cy_ = self.close_pos()
        if math.hypot(x - cx_, y - cy_) <= HANDLE:
            self.closed = True
            return
        self.mode = "move"
        self.grab = (x - self.x, y - self.y)

    def move(self, x, y):
        if self.mode == "move":
            self.x, self.y = x - self.grab[0], y - self.grab[1]

    def release(self, x, y):
        self.mode = None

    def bbox(self):
        m = self.r + 40
        return (self.x - m, self.y - m, self.x + m, self.y + m)

    def draw(self, cr):
        cr.save()
        cr.arc(self.x, self.y, self.r, 0, 2 * math.pi)
        cr.clip()
        cr.set_source_rgb(1, 1, 1)
        cr.paint()
        cr.translate(self.x, self.y)
        cr.scale(self.ZOOM, self.ZOOM)
        cr.translate(-self.x, -self.y)
        for src in self.source():      # alttan üste katmanlar
            if src is not None:
                cr.set_source_surface(src, 0, 0)
                cr.get_source().set_filter(cairo.FILTER_GOOD)
                cr.paint()
        cr.restore()
        cr.save()
        cr.arc(self.x, self.y, self.r, 0, 2 * math.pi)
        cr.set_source_rgb(0.25, 0.25, 0.3)
        cr.set_line_width(7)
        cr.stroke()
        a = math.radians(135)
        cr.move_to(self.x + (self.r + 3) * math.cos(a), self.y + (self.r + 3) * math.sin(a))
        cr.line_to(self.x + (self.r + 70) * math.cos(a), self.y + (self.r + 70) * math.sin(a))
        cr.set_line_width(16)
        cr.set_line_cap(cairo.LINE_CAP_ROUND)
        cr.stroke()
        cr.restore()
        _close_button(cr, *self.close_pos())


class TimerWidget(Overlay):
    """Geri sayım sayacı ve kronometre."""
    kind = "timer"
    W, H = 330.0, 190.0

    def __init__(self, x, y):
        super().__init__()
        self.x, self.y = float(x), float(y)
        self.stopwatch = False
        self.duration = 5 * 60.0
        self.elapsed = 0.0
        self.started_at = None
        self.finished = False

    # ---- zaman
    def now_elapsed(self):
        if self.started_at is None:
            return self.elapsed
        return self.elapsed + time.monotonic() - self.started_at

    def remaining(self):
        return max(0.0, self.duration - self.now_elapsed())

    def running(self):
        return self.started_at is not None

    def tick(self):
        """Pencere düzenli çağırır; süre dolduysa True."""
        if not self.stopwatch and self.running() and self.remaining() <= 0:
            self.elapsed = self.duration
            self.started_at = None
            self.finished = True
            return True
        return False

    def toggle(self):
        if self.running():
            self.elapsed = self.now_elapsed()
            self.started_at = None
        else:
            if not self.stopwatch and self.remaining() <= 0:
                self.elapsed = 0.0
            self.finished = False
            self.started_at = time.monotonic()

    def reset(self):
        self.elapsed = 0.0
        self.started_at = None
        self.finished = False

    # ---- düzen
    def _buttons(self):
        x, y = self.x, self.y
        out = [("close", x + self.W - 18, y + 18, 14)]
        bw, bh, gap = 62.0, 34.0, 8.0
        row = y + self.H - bh - 12
        names = [("mode", _("Kronometre") if not self.stopwatch else _("Sayaç")),
                 ("toggle", _("Durdur") if self.running() else _("Başlat")),
                 ("reset", _("Sıfırla"))]
        widths = [104.0, 92.0, 92.0]
        cx = x + 12
        for (key, label), w in zip(names, widths):
            out.append((key, cx, row, w, bh, label))
            cx += w + gap
        if not self.stopwatch:
            cx = x + 12
            for mins in (1, 3, 5, 10):
                out.append(("preset:%d" % mins, cx, y + 92, bw, 28, "%d dk" % mins))
                cx += bw + gap
        return out

    def hit(self, x, y):
        return (self.x - 6 <= x <= self.x + self.W + 6 and self.y - 6 <= y <= self.y + self.H + 6)

    def press(self, x, y):
        for b in self._buttons():
            if b[0] == "close":
                if math.hypot(x - b[1], y - b[2]) <= HANDLE:
                    self.closed = True
                    return
                continue
            key, bx, by, bw, bh = b[:5]
            if bx <= x <= bx + bw and by <= y <= by + bh:
                if key == "mode":
                    self.stopwatch = not self.stopwatch
                    self.reset()
                elif key == "toggle":
                    self.toggle()
                elif key == "reset":
                    self.reset()
                elif key.startswith("preset:"):
                    self.duration = int(key.split(":")[1]) * 60.0
                    self.reset()
                return
        self.mode = "move"
        self.grab = (x - self.x, y - self.y)

    def move(self, x, y):
        if self.mode == "move":
            self.x, self.y = x - self.grab[0], y - self.grab[1]

    def release(self, x, y):
        self.mode = None

    def bbox(self):
        return (self.x - 10, self.y - 10, self.x + self.W + 10, self.y + self.H + 10)

    def draw(self, cr):
        x, y, W, H = self.x, self.y, self.W, self.H
        flash = self.finished and int(time.monotonic() * 2) % 2 == 0
        cr.save()
        r = 14
        cr.new_sub_path()
        cr.arc(x + W - r, y + r, r, -math.pi / 2, 0)
        cr.arc(x + W - r, y + H - r, r, 0, math.pi / 2)
        cr.arc(x + r, y + H - r, r, math.pi / 2, math.pi)
        cr.arc(x + r, y + r, r, math.pi, 1.5 * math.pi)
        cr.close_path()
        cr.set_source_rgba(0.85, 0.1, 0.1, 0.95) if flash else cr.set_source_rgba(1, 1, 1, 0.96)
        cr.fill_preserve()
        cr.set_source_rgb(*ORANGE)
        cr.set_line_width(2.5)
        cr.stroke()

        secs = self.now_elapsed() if self.stopwatch else self.remaining()
        if self.stopwatch:
            text = "%02d:%02d.%d" % (int(secs) // 60, int(secs) % 60, int(secs * 10) % 10)
        else:
            s = int(math.ceil(secs))
            text = "%02d:%02d" % (s // 60, s % 60)
        cr.select_font_face("Sans", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_BOLD)
        cr.set_font_size(52)
        ext = cr.text_extents(text)
        cr.set_source_rgb(1, 1, 1) if flash else cr.set_source_rgb(0.2, 0.2, 0.25)
        cr.move_to(x + (W - ext.width) / 2.0 - ext.x_bearing, y + 70)
        cr.show_text(text)
        cr.set_font_size(12)
        title = _("Kronometre") if self.stopwatch else (_("Süre doldu!") if self.finished else _("Sayaç"))
        cr.move_to(x + 14, y + 20)
        cr.show_text(title)

        cr.select_font_face("Sans", cairo.FONT_SLANT_NORMAL, cairo.FONT_WEIGHT_NORMAL)
        cr.set_font_size(14)
        for b in self._buttons():
            if b[0] == "close":
                _close_button(cr, b[1], b[2], b[3])
                continue
            key, bx, by, bw, bh, label = b
            active = key.startswith("preset:") and int(key.split(":")[1]) * 60 == self.duration
            cr.rectangle(bx, by, bw, bh)
            cr.set_source_rgb(*ORANGE) if active else cr.set_source_rgb(0.93, 0.93, 0.95)
            cr.fill_preserve()
            cr.set_source_rgba(0.5, 0.5, 0.55, 0.8)
            cr.set_line_width(1)
            cr.stroke()
            ext = cr.text_extents(label)
            cr.set_source_rgb(1, 1, 1) if active else cr.set_source_rgb(0.2, 0.2, 0.25)
            cr.move_to(bx + (bw - ext.width) / 2.0 - ext.x_bearing,
                       by + (bh + ext.height) / 2.0)
            cr.show_text(label)
        cr.restore()


class LaserTrail:
    """Lazer işaretçi: iz 1,2 saniyede söner, sahneye eklenmez."""
    LIFE = 1.2

    def __init__(self):
        self.points = []     # (x, y, zaman)

    def add(self, x, y):
        self.points.append((x, y, time.monotonic()))

    def prune(self):
        now = time.monotonic()
        self.points = [p for p in self.points if now - p[2] < self.LIFE]
        return bool(self.points)

    def bbox(self):
        if not self.points:
            return None
        xs = [p[0] for p in self.points]
        ys = [p[1] for p in self.points]
        return (min(xs) - 20, min(ys) - 20, max(xs) + 20, max(ys) + 20)

    def draw(self, cr):
        now = time.monotonic()
        pts = self.points
        cr.save()
        cr.set_line_cap(cairo.LINE_CAP_ROUND)
        for a, b in zip(pts, pts[1:]):
            if b[2] - a[2] > 0.25:      # ayrı vuruşlar
                continue
            life = max(0.0, 1.0 - (now - b[2]) / self.LIFE)
            cr.set_source_rgba(1, 0.1, 0.1, 0.25 * life)
            cr.set_line_width(16)
            cr.move_to(a[0], a[1])
            cr.line_to(b[0], b[1])
            cr.stroke()
            cr.set_source_rgba(1, 0.2, 0.2, 0.9 * life)
            cr.set_line_width(5)
            cr.move_to(a[0], a[1])
            cr.line_to(b[0], b[1])
            cr.stroke()
        if pts:
            x, y, t = pts[-1]
            life = max(0.0, 1.0 - (now - t) / self.LIFE)
            cr.arc(x, y, 9, 0, 2 * math.pi)
            cr.set_source_rgba(1, 0.15, 0.15, life)
            cr.fill()
        cr.restore()


def snap_point(x, y, overlays, threshold=28.0):
    """(x, y) bir aracın kenarına yakınsa o kenarı döndürür."""
    best = None
    for ov in overlays:
        for edge in ov.snap_edges():
            (qx, qy), d, _t = _project(x, y, *edge)
            if d <= threshold and (best is None or d < best[0]):
                best = (d, edge)
    return best[1] if best else None


def project_on(edge, x, y):
    (qx, qy), _d, _t = _project(x, y, *edge)
    return qx, qy
