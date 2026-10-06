"""Gerçek pencereyle arayüz testleri (X sunucusu gerekir; CI'da xvfb-run).

Ekran yoksa atlanır. Dokunma/fare olayları pencere metotlarıyla verilir;
böylece testler hızlı ve kararlıdır.
"""

import os
import sys

import pytest

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

if not os.environ.get("DISPLAY"):
    pytest.skip("X ekranı yok", allow_module_level=True)

os.environ["GDK_BACKEND"] = "x11"
import gi  # noqa: E402

gi.require_version("Gtk", "3.0")
from gi.repository import Gtk  # noqa: E402

if not Gtk.init_check([])[0]:
    pytest.skip("GTK başlatılamadı", allow_module_level=True)

from fatihkalem import styles as st  # noqa: E402
from fatihkalem import tools as tl  # noqa: E402
from fatihkalem.config import Settings  # noqa: E402
from fatihkalem.window import KalemWindow  # noqa: E402


def pump():
    while Gtk.events_pending():
        Gtk.main_iteration_do(False)


@pytest.fixture
def win(tmp_path, monkeypatch):
    monkeypatch.setenv("XDG_DATA_HOME", str(tmp_path / "data"))
    monkeypatch.setenv("XDG_CONFIG_HOME", str(tmp_path / "cfg"))
    monkeypatch.setenv("FATIHKALEM_TEST_DIR", str(tmp_path))
    s = Settings(str(tmp_path / "ayarlar.json"), None)
    s["CheckUpdates"] = False
    w = KalemWindow(None, s)
    w.show_all()
    pump()
    w.toggle_minimized()          # kalem moduna geç
    pump()
    yield w
    w.destroy()
    pump()


def draw(w, pts):
    w.pointer_down(pts[0][0], pts[0][1], 0.5, False)
    for x, y in pts[1:]:
        w.pointer_move(x, y, 0.5)
    w.pointer_up(*pts[-1])
    pump()


def test_draw_undo_redo(win):
    draw(win, [(400, 400), (500, 420), (600, 400)])
    assert len(win.history.scene.strokes) == 1
    win.undo()
    assert not win.history.scene.strokes
    win.redo()
    assert len(win.history.scene.strokes) == 1


def test_menu_buttons_open_submenus(win):
    for name, sub in (("pen", "pen"), ("eraser", "eraser"), ("shape", "shape"), ("tools", "tools")):
        win.button_clicked(name)
        assert win.shown_submenu == sub
        win.collapse_submenus()


def test_dashed_pen_and_eraser(win):
    win.perform_tool("stroke:dash")
    draw(win, [(300, 300), (700, 300)])
    n = len(win.history.scene.strokes)
    assert n > 5
    win.perform_tool("stroke:normal")
    win.activate_eraser(*st.ERASER_SIZES[4], number=4)
    draw(win, [(300, 300), (700, 300)])
    assert len(win.history.scene.strokes) < n


def test_ruler_snaps_stroke(win):
    win.perform_tool("ruler")
    ruler = [o for o in win.overlays if isinstance(o, tl.Ruler)][0]
    top = ruler.cy - ruler.WIDTH / 2
    draw(win, [(ruler.cx - 100, top - 8), (ruler.cx, top + 9), (ruler.cx + 100, top - 5)])
    s = win.history.scene.strokes[-1]
    assert all(abs(p[1] - top) < 0.01 for p in s.points)
    win.perform_tool("ruler")
    assert not win.overlays


def test_select_move_delete(win):
    draw(win, [(300, 300), (350, 320), (400, 300)])
    win.perform_tool("select")
    assert win.draw_state == st.SELECT
    draw(win, [(280, 280), (420, 280), (420, 340), (280, 340), (280, 280)])
    assert len(win.selection) == 1
    before = win.history.scene.strokes[0].points[0]
    draw(win, [(350, 310), (450, 410)])
    after = win.history.scene.strokes[0].points[0]
    assert after[0] == pytest.approx(before[0] + 100) and after[1] == pytest.approx(before[1] + 100)
    win._delete_selection()
    assert not win.history.scene.strokes


def test_fill_closed_shape(win):
    win.activate_shape(st.RECTANGLE)
    draw(win, [(300, 300), (500, 450)])
    win.perform_tool("fill")
    win.pointer_down(400, 380, 0.5, False)
    win.pointer_up(400, 380)
    assert len(win.history.scene.fills) == 1


def test_pages_and_lesson_save(win, tmp_path):
    draw(win, [(100, 100), (200, 200)])
    win.perform_tool("page:new")
    assert len(win.book) == 2 and win.history.scene.is_empty()
    draw(win, [(300, 100), (400, 200)])
    win.perform_tool("page:prev")
    assert win.book.index == 0
    win.save_lesson_dialog()
    saved = [f for f in os.listdir(tmp_path) if f.endswith(".fkalem")]
    assert saved
    win.perform_tool("pdf")
    assert [f for f in os.listdir(tmp_path) if f.endswith(".pdf")]


def test_minimize_autosaves_and_clears(win, tmp_path):
    draw(win, [(100, 100), (200, 200)])
    win.toggle_minimized()
    assert win.minimized and win.book.is_empty()
    auto = os.path.join(str(tmp_path / "data"), "fatih-kalem", "Otomatik Kayıt")
    assert os.listdir(auto)


def test_undo_after_minimize_restores_all_pages(win):
    draw(win, [(100, 100), (200, 200)])
    win.perform_tool("page:new")
    draw(win, [(300, 300), (400, 400)])
    win.toggle_minimized()            # el modu: temizlenir
    win.toggle_minimized()            # kalem modu: boş tahta
    assert win.book.is_empty() and win.can_undo()
    win.undo()                        # Silgi → Geri Al
    assert len(win.book) == 2 and not win.book.is_empty()
    assert win.can_redo()
    win.redo()                        # tekrar temizle
    assert win.book.is_empty()
    win.undo()
    assert len(win.book) == 2


def test_new_drawing_after_minimize_then_undo_twice(win):
    draw(win, [(100, 100), (200, 200)])
    win.toggle_minimized()
    win.toggle_minimized()
    draw(win, [(500, 500), (600, 600)])
    win.undo()                        # önce yeni çizgi gider
    assert win.history.scene.is_empty()
    win.undo()                        # sonra eski ders geri gelir
    assert len(win.history.scene.strokes) == 1
    assert win.history.scene.strokes[0].points[0][:2] == (100, 100)


def test_menu_scale_hit_testing(win):
    win.settings["MenuScale"] = 1.5
    win.settings_changed()
    name, x, y, w, h = [i for i in win.toolbar.items() if i[0] == "eraser"][0]
    assert w == pytest.approx(63)
    assert win.toolbar.hit_button(x + w / 2, y + h / 2) == "eraser"


def test_gesture_left_selects_red(win):
    win.gesture_down(800, 500)
    win.gesture_move(600, 500)
    win.gesture_up()
    assert win.color_no == 1
