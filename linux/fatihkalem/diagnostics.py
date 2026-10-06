"""Günlük (log) dosyası ve tek tıkla hata raporu."""

import logging
import os
import platform
import sys
import time
import traceback
import urllib.parse

from . import GITHUB_URL, VERSION, system


def log_dir():
    base = os.environ.get("XDG_CACHE_HOME") or os.path.expanduser("~/.cache")
    d = os.path.join(base, "fatih-kalem")
    os.makedirs(d, exist_ok=True)
    return d


def log_path():
    return os.path.join(log_dir(), "fatih-kalem.log")


def setup_logging():
    path = log_path()
    try:
        if os.path.exists(path) and os.path.getsize(path) > 512 * 1024:
            os.replace(path, path + ".1")
    except OSError:
        pass
    logging.basicConfig(filename=path, level=logging.INFO,
                        format="%(asctime)s %(levelname)s %(name)s: %(message)s")
    log = logging.getLogger("fatih-kalem")
    log.info("Fatih Kalem %s başladı (Python %s)", VERSION, platform.python_version())

    def hook(exc_type, exc, tb):
        log.error("Yakalanmamış hata:\n%s", "".join(traceback.format_exception(exc_type, exc, tb)))
        sys.__excepthook__(exc_type, exc, tb)
    sys.excepthook = hook
    return log


def system_info(composited=None):
    lines = [
        "Fatih Kalem: %s" % VERSION,
        "Python: %s" % platform.python_version(),
        "Çekirdek: %s" % platform.release(),
        "Oturum: %s" % system.session_type(),
        "Masaüstü: %s" % (system.desktop_name() or "?"),
        "Pardus: %s, ETAP: %s" % (system.is_pardus(), system.is_etap()),
    ]
    try:
        with open("/etc/os-release", encoding="utf-8") as f:
            for line in f:
                if line.startswith("PRETTY_NAME="):
                    lines.append("Sistem: %s" % line.split("=", 1)[1].strip().strip('"'))
    except OSError:
        pass
    if composited is not None:
        lines.append("Saydamlık (bileşikleştirici): %s" % composited)
    try:
        import gi
        gi.require_version("Gtk", "3.0")
        from gi.repository import Gtk
        lines.append("GTK: %d.%d.%d" % (Gtk.get_major_version(), Gtk.get_minor_version(),
                                        Gtk.get_micro_version()))
    except Exception:
        pass
    return lines


def write_report(composited=None):
    """Raporu masaüstüne/ana klasöre yazar; (dosya yolu, GitHub adresi) döndürür."""
    info = system_info(composited)
    try:
        with open(log_path(), encoding="utf-8", errors="replace") as f:
            log_tail = f.read()[-20000:]
    except OSError:
        log_tail = "(günlük yok)"
    target_dir = os.path.expanduser("~/Masaüstü")
    if not os.path.isdir(target_dir):
        target_dir = os.path.expanduser("~/Desktop")
    if not os.path.isdir(target_dir):
        target_dir = os.path.expanduser("~")
    path = os.path.join(target_dir, time.strftime("fatih-kalem-hata-raporu-%Y%m%d-%H%M.txt"))
    with open(path, "w", encoding="utf-8") as f:
        f.write("\n".join(info))
        f.write("\n\n--- Günlük ---\n")
        f.write(log_tail)
    body = "**Sorun:**\n\n\n**Nasıl oluştu:**\n\n\n**Sistem:**\n```\n%s\n```\n" \
           "(Ayrıntılı günlük için `%s` dosyasını ekleyin.)" % ("\n".join(info), os.path.basename(path))
    url = "%s/issues/new?%s" % (GITHUB_URL, urllib.parse.urlencode(
        {"title": "Hata: ", "body": body}))
    return path, url
