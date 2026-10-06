"""2.1 özelliklerinin ekran gerektirmeyen testleri."""

import json
import math
import os
import sys
import zipfile

import cairo
import pytest

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

from fatihkalem import i18n, lesson, qr, update  # noqa: E402
from fatihkalem import recognize as rec  # noqa: E402
from fatihkalem import tools as tl  # noqa: E402
from fatihkalem.config import Settings  # noqa: E402
from fatihkalem.ink import Stroke, densify  # noqa: E402
from fatihkalem.scene import (Book, FillItem, TextItem, point_in_polygon,  # noqa: E402
                              polygon_area)


def stroke_of(points, w=3):
    return Stroke(densify([(x, y, 0.5) for x, y in points]), (0, 0, 1), w, w)


def wobbly(points, amp=2.0):
    """Elle çizilmiş gibi titrek noktalar."""
    pts = densify([(x, y, 0.5) for x, y in points])
    return [(p[0] + amp * math.sin(i * 0.7), p[1] + amp * math.cos(i * 0.9))
            for i, p in enumerate(pts)]


# ----------------------------------------------------------- şekil tanıma --

def test_recognize_line():
    kind, data = rec.recognize([(p[0], p[1], 0.5) for p in wobbly([(0, 0), (300, 40)], 1.5)])
    assert kind == "line"


def test_recognize_rectangle():
    pts = wobbly([(100, 100), (400, 102), (402, 300), (98, 302), (101, 108)])
    kind, data = rec.recognize([(x, y, 0.5) for x, y in pts])
    assert kind == "polygon" and len(data) == 4


def test_recognize_triangle():
    pts = wobbly([(100, 300), (250, 60), (400, 300), (104, 296)])
    kind, data = rec.recognize([(x, y, 0.5) for x, y in pts])
    assert kind == "polygon" and len(data) == 3


def test_recognize_ellipse():
    pts = [(300 + 150 * math.cos(a / 30.0), 200 + 90 * math.sin(a / 30.0) + 2 * math.sin(a))
           for a in range(0, 190)]
    kind, data = rec.recognize([(x, y, 0.5) for x, y in pts])
    assert kind == "ellipse"
    cx, cy, rx, ry = data
    assert abs(cx - 300) < 10 and abs(rx - 150) < 12 and abs(ry - 90) < 12


def test_open_curve_not_recognized():
    pts = [(x, 100 * math.sin(x / 40.0)) for x in range(0, 400, 4)]
    assert rec.recognize([(x, y, 0.5) for x, y in pts]) is None


def test_smooth_keeps_ends_and_reduces_jitter():
    pts = [(x, 2.5 * (-1) ** (x // 2)) for x in range(0, 200, 2)]
    s = stroke_of(pts)
    out = rec.smooth(s.points)
    assert out[0][:2] == pytest.approx(s.points[0][:2])
    assert out[-1][:2] == pytest.approx(s.points[-1][:2])
    jitter_before = max(abs(p[1]) for p in s.points)
    jitter_after = max(abs(p[1]) for p in out[10:-10])
    assert jitter_after < jitter_before


def test_dashed_and_dotted_split():
    s = stroke_of([(0, 0), (400, 0)], w=3)
    dashes = rec.dashed(s)
    dots = rec.dotted(s)
    assert len(dashes) >= 8
    assert len(dots) > len(dashes)
    for d in dashes:
        assert d.points[-1][0] - d.points[0][0] <= 13


def test_arrow_head_added():
    s = stroke_of([(0, 0), (200, 0)])
    out = rec.with_arrow(s)
    assert len(out) == 2
    head = out[1]
    assert max(p[0] for p in head.points) == pytest.approx(200, abs=0.6)
    assert min(p[1] for p in head.points) < -5 and max(p[1] for p in head.points) > 5


def test_chunk_long_stroke():
    s = stroke_of([(0, 0), (8000, 0)])
    parts = rec.chunk(s, 1000)
    assert len(parts) > 1
    assert parts[0].points[-1] == parts[1].points[0]


# --------------------------------------------------------------- sayfalar --

def test_book_pages():
    b = Book()
    b.current.commit(b.current.scene.replace(strokes=(stroke_of([(0, 0), (9, 9)]),)))
    assert b.new_page() and len(b) == 2 and b.index == 1
    assert b.current.scene.is_empty()
    assert b.go(0) and not b.current.scene.is_empty()
    b.go(1)
    b.delete_page()
    assert len(b) == 1 and not b.current.scene.is_empty()
    assert not b.is_empty()
    b.reset()
    assert b.is_empty()


def test_polygon_helpers():
    sq = [(0, 0), (10, 0), (10, 10), (0, 10)]
    assert point_in_polygon(5, 5, sq) and not point_in_polygon(15, 5, sq)
    assert polygon_area(sq) == pytest.approx(100)


# ---------------------------------------------------------- ders dosyası --

def test_lesson_roundtrip_with_image(tmp_path, monkeypatch):
    monkeypatch.setenv("XDG_DATA_HOME", str(tmp_path / "data"))
    img = tmp_path / "resim.png"
    surf = cairo.ImageSurface(cairo.FORMAT_ARGB32, 4, 4)
    surf.write_to_png(str(img))
    from fatihkalem.scene import LibraryItem
    b = Book()
    sc = b.current.scene.replace(
        strokes=(stroke_of([(10, 10), (100, 40)]),),
        texts=(TextItem("Merhaba ÇĞİÖŞÜ", 5, 6, 30, (1, 0, 0)),),
        fills=(FillItem([(0, 0), (20, 0), (20, 20)], (0, 1, 0)),),
        backgrounds=(str(img),),
        library=(LibraryItem(str(img), 1, 2, 30, 40, 15),),
        curtain=(1, 2, 3, 4))
    b.current.commit(sc)
    b.new_page()
    path = lesson.save_lesson(str(tmp_path / "ders"), b, 800, 600)
    assert path.endswith(".fkalem")
    with zipfile.ZipFile(path) as z:
        names = z.namelist()
        data = json.loads(z.read("ders.json"))
    assert "ders.json" in names and any(n.startswith("gorseller/") for n in names)
    assert len(data["pages"]) == 2
    b2, size = lesson.load_lesson(path)
    assert size == (800, 600) and len(b2) == 2 and b2.index == 1
    s2 = b2.pages[0].scene
    assert s2.texts[0].text == "Merhaba ÇĞİÖŞÜ"
    assert len(s2.strokes) == 1 and len(s2.fills) == 1 and s2.curtain == (1, 2, 3, 4)
    assert os.path.exists(s2.backgrounds[0]) and s2.library[0].angle == 15


def test_export_png_and_pdf(tmp_path):
    b = Book()
    b.current.commit(b.current.scene.replace(
        strokes=(stroke_of([(10, 10), (100, 80)]),),
        texts=(TextItem("Deneme", 20, 20, 24, (0, 0, 0)),)))
    b.new_page()
    png = lesson.export_png(str(tmp_path / "a"), b.pages[0].scene, 200, 100, lambda p: None)
    pdf = lesson.export_pdf(str(tmp_path / "b"), [h.scene for h in b.pages], 200, 100,
                            lambda p: None)
    assert os.path.getsize(png) > 100
    with open(pdf, "rb") as f:
        content = f.read()
    one = lesson.export_pdf(str(tmp_path / "c"), [b.pages[0].scene], 200, 100, lambda p: None)
    assert content.startswith(b"%PDF")
    assert os.path.getsize(pdf) > os.path.getsize(one)      # ikinci sayfa eklendi


def test_autosave_keeps_latest(tmp_path, monkeypatch):
    monkeypatch.setenv("XDG_DATA_HOME", str(tmp_path))
    b = Book()
    assert lesson.autosave(b, 10, 10) is None          # boş ders kaydedilmez
    b.current.commit(b.current.scene.replace(strokes=(stroke_of([(0, 0), (5, 5)]),)))
    p = lesson.autosave(b, 10, 10)
    assert p and os.path.exists(p)


# --------------------------------------------------------------------- QR --

def test_qr_structure():
    m = qr.encode("http://192.168.1.20:8000/Ders.pdf")
    n = len(m)
    assert (n - 17) % 4 == 0
    # üç bulucu desen: köşelerde 7x7 dış çerçeve siyah
    for r0, c0 in ((0, 0), (0, n - 7), (n - 7, 0)):
        assert all(m[r0][c0 + i] for i in range(7))
        assert all(m[r0 + 6][c0 + i] for i in range(7))
        assert not m[r0 + 1][c0 + 1] and m[r0 + 3][c0 + 3]
    # zamanlama deseni
    assert [m[6][i] for i in range(8, n - 8)] == [i % 2 == 0 for i in range(8, n - 8)]


def test_qr_too_long():
    with pytest.raises(ValueError):
        qr.encode("x" * 400)


def test_qr_draw():
    surf = cairo.ImageSurface(cairo.FORMAT_RGB24, 100, 100)
    qr.draw(cairo.Context(surf), qr.encode("abc"), 0, 0, 100)


# ----------------------------------------------------------- ölçme araçları

def test_ruler_snapping_and_edge_zone():
    r = tl.Ruler(500, 500)
    edge = tl.snap_point(450, 500 - r.WIDTH / 2 - 10, [r])
    assert edge is not None
    x, y = tl.project_on(edge, 450, 470)
    assert y == pytest.approx(500 - r.WIDTH / 2)
    assert r.in_edge_zone(450, 500 - r.WIDTH / 2 + 5)
    assert not r.in_edge_zone(500, 500)
    assert r.hit(500, 500)
    r.angle = math.radians(90)
    edge = tl.snap_point(500 - r.WIDTH / 2 - 8, 480, [r])
    x, y = tl.project_on(edge, 500 - r.WIDTH / 2 - 8, 480)
    assert x == pytest.approx(500 - r.WIDTH / 2, abs=0.01) or x == pytest.approx(500 + r.WIDTH / 2, abs=0.01)


def test_ruler_rotate_snaps_to_15_degrees():
    r = tl.Ruler(0, 0)
    hx, hy = r.to_world(*r.rotate_handle())
    r.press(hx, hy)
    a = math.radians(44.2)
    r.move(math.cos(a) * 400, math.sin(a) * 400)
    assert math.degrees(r.angle) == pytest.approx(45.0)


def test_compass_draws_arc():
    c = tl.Compass(0, 0, 100)
    got = {}
    c.on_arc = lambda pts, done: got.update(pts=list(pts), done=done)
    px, py = c.pen_pos()
    c.press(px, py)
    for deg in range(0, 181, 10):
        a = math.radians(deg)
        c.move(100 * math.cos(a), 100 * math.sin(a))
    c.release(0, 0)
    assert got["done"] and len(got["pts"]) > 10
    assert all(math.hypot(x, y) == pytest.approx(100, abs=0.01) for x, y in got["pts"])


def test_timer_countdown_and_stopwatch():
    t = tl.TimerWidget(0, 0)
    t.duration = 0.01
    t.toggle()
    import time
    time.sleep(0.03)
    assert t.tick() and t.finished
    t.stopwatch = True
    t.reset()
    t.toggle()
    time.sleep(0.02)
    assert t.now_elapsed() > 0.0


# ---------------------------------------------------------- ayar / sürüm --

def test_system_defaults_and_user_override(tmp_path):
    sysf = tmp_path / "sistem.json"
    sysf.write_text(json.dumps({"InkSize": 5, "MenuScale": 1.5, "Language": "en"}))
    user = tmp_path / "kullanici.json"
    user.write_text(json.dumps({"InkSize": 2}))
    s = Settings(str(user), str(sysf))
    assert s["InkSize"] == 2 and s["MenuScale"] == 1.5 and s["Language"] == "en"


def test_settings_export_import(tmp_path):
    s = Settings(str(tmp_path / "a.json"), None)
    s["Favourites"] = ["Tool:ruler"]
    s.export_to(str(tmp_path / "disari.json"))
    s2 = Settings(str(tmp_path / "b.json"), None)
    s2.import_from(str(tmp_path / "disari.json"))
    assert s2["Favourites"] == ["Tool:ruler"]
    (tmp_path / "bozuk.json").write_text("[]")
    with pytest.raises(ValueError):
        s2.import_from(str(tmp_path / "bozuk.json"))


def test_recent_colors_limit(tmp_path):
    s = Settings(str(tmp_path / "a.json"), None)
    for i in range(10):
        s.add_recent_color((i / 10.0, 0, 0))
    assert len(s["RecentColors"]) == 5
    assert s["RecentColors"][0] == "#E60000"


def test_version_compare():
    assert update.is_newer("v2.1.0", "2.0.0")
    assert not update.is_newer("v2.0.0", "2.0.0")
    assert update.is_newer("2.10", "2.9.9")
    assert update.parse_version("v3") == (3, 0, 0)


def test_i18n():
    i18n.set_language("en")
    assert i18n._("Cetvel") == "Ruler"
    assert i18n._("Bilinmeyen metin") == "Bilinmeyen metin"
    i18n.set_language("tr")
    assert i18n._("Cetvel") == "Cetvel"
