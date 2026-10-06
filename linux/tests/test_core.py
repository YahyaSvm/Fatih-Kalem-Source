"""Ekran gerektirmeyen çekirdek testleri: mürekkep, silgi, geçmiş, ayarlar,
menü bölgeleri ve şekiller."""

import json
import os
import sys

import cairo
import pytest

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

from fatihkalem import styles as st  # noqa: E402
from fatihkalem import toolbar as tb  # noqa: E402
from fatihkalem.config import Settings, hex_to_rgb, rgb_to_hex  # noqa: E402
from fatihkalem.ink import (Eraser, Stroke, densify, interpolate_centers,  # noqa: E402
                            render_strokes)
from fatihkalem.scene import History, LibraryItem, Scene  # noqa: E402


def line_stroke(x0, y0, x1, y1, w=3):
    return Stroke(densify([(x0, y0, 0.5), (x1, y1, 0.5)]), (0, 0, 1), w, w)


# ------------------------------------------------------------------ mürekkep

def test_densify_spacing():
    pts = densify([(0, 0, 0.5), (100, 0, 0.5)])
    assert pts[0][:2] == (0, 0) and pts[-1][:2] == (100, 0)
    gaps = [b[0] - a[0] for a, b in zip(pts, pts[1:])]
    assert max(gaps) <= 2.0 + 1e-9


def test_eraser_splits_stroke_in_two():
    s = line_stroke(0, 100, 400, 100)
    er = Eraser(20, 32)
    out, changed, dirty = er.erase([s], [(200, 100)])
    assert changed and dirty is not None
    assert len(out) == 2
    left, right = out
    assert max(p[0] for p in left.points) < 200 - 5
    assert min(p[0] for p in right.points) > 200 + 5


def test_eraser_misses_far_stroke():
    s = line_stroke(0, 100, 400, 100)
    out, changed, _ = Eraser(20, 32).erase([s], [(200, 400)])
    assert not changed and out == [s]


def test_eraser_removes_whole_small_stroke():
    s = line_stroke(195, 100, 205, 100)
    out, changed, _ = Eraser(50, 83).erase([s], [(200, 100)])
    assert changed and out == []


def test_interpolate_centers_has_no_gaps():
    cs = interpolate_centers((0, 0), (100, 0), step=4)
    assert cs[-1] == (100, 0)
    assert len(cs) >= 25


def test_render_strokes_draws_pixels():
    surf = cairo.ImageSurface(cairo.FORMAT_ARGB32, 50, 50)
    cr = cairo.Context(surf)
    styles = [st.pen_attributes(style, 4) for style in (st.MARKER, st.STYLOGRAPH, st.HIGHLIGHTER)]
    strokes = [Stroke(densify([(5, 10 + 12 * i, 0.5), (45, 10 + 12 * i, 0.5)]), (1, 0, 0),
                      w, h, tip, rot, hl) for i, (w, h, tip, rot, hl) in enumerate(styles)]
    render_strokes(cr, strokes)
    surf.flush()
    data = bytes(surf.get_data())
    stride = surf.get_stride()
    for i in range(3):
        y = 10 + 12 * i
        alpha = data[y * stride + 25 * 4 + 3]
        assert alpha > 0, "stil %d çizilmedi" % i


def test_highlighter_is_half_transparent():
    surf = cairo.ImageSurface(cairo.FORMAT_ARGB32, 40, 40)
    cr = cairo.Context(surf)
    w, h, tip, rot, hl = st.pen_attributes(st.HIGHLIGHTER, 5)
    s = Stroke(densify([(5, 20, 0.5), (35, 20, 0.5)]), (1, 1, 0), w, h, tip, rot, hl)
    render_strokes(cr, [s, s])        # üst üste iki kez: koyulaşmamalı
    surf.flush()
    alpha = bytes(surf.get_data())[20 * surf.get_stride() + 20 * 4 + 3]
    assert 100 <= alpha <= 200


# ------------------------------------------------------------------- geçmiş

def test_history_undo_redo():
    h = History()
    s1 = h.scene.replace(strokes=(line_stroke(0, 0, 10, 10),))
    h.commit(s1)
    s2 = s1.replace(curtain=(10, 10, 100, 100))
    h.commit(s2)
    assert h.can_undo() and not h.can_redo()
    assert h.undo() and h.scene is s1
    assert h.undo() and h.scene.is_empty()
    assert not h.undo()
    assert h.redo() and h.scene is s1
    h.commit(s1.replace(strokes=()))
    assert not h.can_redo()


def test_history_limit():
    h = History(limit=5)
    for i in range(20):
        h.commit(Scene(strokes=(line_stroke(i, 0, i + 1, 0),)))
    n = 0
    while h.undo():
        n += 1
    assert n == 5


def test_library_item_hit_and_rotation():
    it = LibraryItem("x.png", 100, 100, 200, 100)
    assert it.contains(200, 150)
    assert not it.contains(90, 90)
    it.angle = 90
    # 90 derece dönünce yükseklik yatay olur: (200, 60) içeride kalır
    assert it.contains(200, 60)
    assert not it.contains(110, 150)


# ------------------------------------------------------------------- ayarlar

def test_settings_roundtrip_and_validation(tmp_path):
    path = tmp_path / "ayarlar.json"
    s = Settings(str(path))
    assert s["PenStyle"] == 1 and s["ColorNo"] == 2 and s["InkSize"] == 3
    s["Favourites"] = ["MarkerRed", "Undo"]
    s["InkSize"] = 6
    s.save()
    s2 = Settings(str(path))
    assert s2["Favourites"] == ["MarkerRed", "Undo"] and s2["InkSize"] == 6
    path.write_text(json.dumps({"InkSize": 99, "PenStyle": "bozuk"}))
    s3 = Settings(str(path))
    assert s3["InkSize"] == 3 and s3["PenStyle"] == 1


def test_settings_survives_corrupt_file(tmp_path):
    path = tmp_path / "ayarlar.json"
    path.write_text("{ bozuk json")
    assert Settings(str(path))["ColorNo"] == 2


def test_color_hex():
    assert rgb_to_hex(hex_to_rgb("#FF00009D")) == "#00009D"   # orijinal #AARRGGBB
    assert rgb_to_hex(hex_to_rgb("#123456")) == "#123456"


# -------------------------------------------------------- orijinal değerler

def test_original_palette_and_sizes():
    assert st.PALETTE[st.STYLOGRAPH][2] == (0, 0, 157 / 255.0)       # varsayılan mavi
    assert st.PALETTE[st.HIGHLIGHTER][1] == (1.0, 0, 1.0)            # fosforlu "kırmızı" = pembe
    assert st.INK_SIZES[st.MARKER][6] == (22, 22)
    assert st.INK_SIZES[st.HIGHLIGHTER][6] == (30, 60)
    assert st.INK_SIZES[st.STYLOGRAPH][1] == (0.75, 2.25)
    assert st.SHAPE_SIZES == {1: 1, 2: 2, 3: 3, 4: 5, 5: 10, 6: 22}


@pytest.mark.parametrize("x,y,expected", [
    (5, 5, None),
    (40, 20, ("tab", st.MARKER)),
    (80, 20, ("tab", st.STYLOGRAPH)),
    (120, 20, ("tab", st.HIGHLIGHTER)),
    (30, 50, ("color", 1)), (70, 50, ("color", 2)), (100, 50, ("color", 3)),
    (140, 50, ("color", 4)), (30, 90, ("color", 5)), (70, 90, ("color", 6)),
    (100, 90, ("colorful", None)), (140, 90, ("nopen", None)),
    (25, 120, ("size", 1)), (40, 120, ("size", 2)), (60, 120, ("size", 3)),
    (80, 120, ("size", 4)), (110, 120, ("size", 5)), (140, 120, ("size", 6)),
    (160, 120, None), (80, 140, None),
])
def test_pen_submenu_regions(x, y, expected):
    assert tb.pen_submenu_action(x, y) == expected


@pytest.mark.parametrize("x,y,expected", [
    (40, 30, ("undo", None)), (90, 30, ("redo", None)), (130, 30, ("curtain", None)),
    (30, 70, ("eraser", 1)), (60, 70, ("eraser", 2)), (100, 70, ("eraser", 3)),
    (130, 70, ("eraser", 4)), (10, 70, None), (100, 95, None),
])
def test_eraser_submenu_regions(x, y, expected):
    assert tb.eraser_submenu_action(x, y) == expected


@pytest.mark.parametrize("x,y,expected", [
    (40, 30, ("shape", st.LINE)), (90, 30, ("shape", st.DASHLINE)),
    (130, 30, ("shape", st.ARROW)), (40, 60, ("shape", st.RECTANGLE)),
    (90, 60, ("shape", st.ELLIPSE)), (130, 60, ("shape", st.TRIANGLE)),
    (50, 100, ("library", None)), (120, 100, ("background", None)),
])
def test_shape_submenu_regions(x, y, expected):
    assert tb.shape_submenu_action(x, y) == expected


def test_color_selector_regions():
    assert tb.color_selector_action(30, 30) == ("cartela", (94, 124, 139))
    assert tb.color_selector_action(53, 30) == ("cartela", (94, 124, 139))
    assert tb.color_selector_action(54, 30) == ("cartela", (136, 196, 64))
    assert tb.color_selector_action(140, 200) == ("cartela", (61, 77, 183))
    assert tb.color_selector_action(10, 30) is None
    assert tb.color_selector_action(100, 210) is None


def test_favourite_tags_match_original():
    assert tb.favourite_tag_for("pen", ("color", 1), st.MARKER) == "MarkerRed"
    assert tb.favourite_tag_for("pen", ("color", 4), st.HIGHLIGHTER) == "HighlighterYellow"
    assert tb.favourite_tag_for("pen", ("colorful", None), st.STYLOGRAPH) == "StylographColorFul"
    assert tb.favourite_tag_for("pen", ("size", 3), st.MARKER) == "Size3"
    assert tb.favourite_tag_for("eraser", ("undo", None), st.MARKER) == "Undo"
    assert tb.favourite_tag_for("shape", ("shape", st.DASHLINE), st.MARKER) == "DashLine"


def test_expanded_height_matches_original():
    # İlk sürüm: 223 + iFav * 42; 2.1: + Araçlar düğmesi (42) + imza satırı
    assert tb.expanded_height(0) == 223 + 42 + tb.BYLINE_HEIGHT
    assert tb.expanded_height(3) == 223 + 42 + 126 + tb.BYLINE_HEIGHT
    assert tb.BYLINE_TEXT == "By YhySvm"


# ------------------------------------------------------------------ şekiller

def test_arrow_has_head():
    (s,) = st.arrow((0, 0), (100, 0), (0, 0, 0), 5)
    xs = [p[0] for p in s.points]
    ys = [p[1] for p in s.points]
    assert max(xs) == pytest.approx(100)
    assert min(ys) < -5 and max(ys) > 5      # iki kanat


def test_dash_line_segments():
    segs = st.dash_line((0, 0), (200, 0), (0, 0, 0), 5)   # parça 20 px
    assert len(segs) == 5
    assert all(max(p[0] for p in s.points) - min(p[0] for p in s.points) <= 20.01 for s in segs)


def test_triangle_closed():
    (s,) = st.triangle((0, 0), (100, 0), (50, 80), (0, 0, 0), 3)
    assert s.points[0][:2] == pytest.approx(s.points[-1][:2])


def test_pen_image_names_exist():
    img_dir = os.path.join(os.path.dirname(__file__), "..", "data", "images")
    for style in (st.MARKER, st.STYLOGRAPH, st.HIGHLIGHTER):
        for no in range(1, 8):
            name = st.pen_image_name(style, no).lower()
            assert os.path.exists(os.path.join(img_dir, name + ".png")), name
        assert os.path.exists(os.path.join(img_dir, st.pen_image_name(style, 1, True).lower() + ".png"))
