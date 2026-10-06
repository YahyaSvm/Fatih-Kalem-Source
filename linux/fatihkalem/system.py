"""Masaüstü ortamına ait işler: otomatik başlatma, oturum türü, ekran alanı.

Pardus'ta (XFCE, GNOME ve ETAP) ve diğer XDG uyumlu masaüstlerinde çalışır.
Windows'taki "Run" kayıt defteri anahtarının karşılığı
~/.config/autostart/fatih-kalem.desktop dosyasıdır.
"""

import os
import shutil

AUTOSTART_NAME = "fatih-kalem.desktop"


def session_type():
    """'x11', 'wayland' ya da 'unknown'."""
    st = (os.environ.get("XDG_SESSION_TYPE") or "").lower()
    if st in ("x11", "wayland"):
        return st
    if os.environ.get("WAYLAND_DISPLAY"):
        return "wayland"
    if os.environ.get("DISPLAY"):
        return "x11"
    return "unknown"


def desktop_name():
    return (os.environ.get("XDG_CURRENT_DESKTOP") or
            os.environ.get("DESKTOP_SESSION") or "").lower()


def is_pardus():
    try:
        with open("/etc/os-release", encoding="utf-8") as f:
            return "pardus" in f.read().lower()
    except OSError:
        return False


def is_etap():
    """Pardus ETAP (etkileşimli tahta sürümü) mi?"""
    if os.path.exists("/etc/pardus/etap") or os.path.isdir("/usr/share/pardus/etap"):
        return True
    try:
        with open("/etc/os-release", encoding="utf-8") as f:
            return "etap" in f.read().lower()
    except OSError:
        return False


def _autostart_dir():
    base = os.environ.get("XDG_CONFIG_HOME") or os.path.expanduser("~/.config")
    return os.path.join(base, "autostart")


def _launcher():
    found = shutil.which("fatih-kalem")
    return found or "fatih-kalem"


def autostart_state():
    """None, 'start' ya da 'preload'."""
    path = os.path.join(_autostart_dir(), AUTOSTART_NAME)
    try:
        with open(path, encoding="utf-8") as f:
            text = f.read()
    except OSError:
        return None
    if "Hidden=true" in text or "X-GNOME-Autostart-enabled=false" in text:
        return None
    return "preload" if "--preload" in text else "start"


def set_autostart(mode):
    """mode: None (kapalı), 'start' ya da 'preload'."""
    path = os.path.join(_autostart_dir(), AUTOSTART_NAME)
    if mode is None:
        try:
            os.unlink(path)
        except OSError:
            pass
        return
    os.makedirs(_autostart_dir(), exist_ok=True)
    args = " --preload" if mode == "preload" else ""
    text = (
        "[Desktop Entry]\n"
        "Type=Application\n"
        "Name=Fatih Kalem\n"
        "Comment=Etkileşimli tahta kalemi\n"
        "Exec=%s%s\n"
        "Icon=fatih-kalem\n"
        "Terminal=false\n"
        "X-GNOME-Autostart-enabled=true\n"
        "X-GNOME-Autostart-Delay=3\n"
        "X-MATE-Autostart-Delay=3\n" % (_launcher(), args))
    tmp = path + ".tmp"
    with open(tmp, "w", encoding="utf-8") as f:
        f.write(text)
    os.replace(tmp, path)


def default_pictures_dir():
    try:
        from gi.repository import GLib
        d = GLib.get_user_special_dir(GLib.UserDirectory.DIRECTORY_PICTURES)
        if d and os.path.isdir(d):
            return d
    except Exception:
        pass
    return os.path.expanduser("~")
