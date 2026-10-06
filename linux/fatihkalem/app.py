"""Uygulama giriş noktası.

Orijinaldeki CheckPrevinstanceAndCommandLineArguments karşılığı:
  * Program ikinci kez açılırsa yeni pencere açılmaz; var olan öne gelir.
  * --preload (Windows'taki /PreLoad): dosyaları belleğe alıp hemen çıkar.
"""

import os
import sys


def _prefer_x11():
    """Wayland oturumunda XWayland kullan.

    Her zaman üstte duran, tıklamaları geçiren saydam bir kaplama penceresi
    Wayland'de standart olarak mümkün değildir; XWayland üzerinden hem
    GNOME (Pardus GNOME / ETAP) hem de diğer masaüstlerinde çalışır.
    """
    if os.environ.get("FATIHKALEM_NATIVE_WAYLAND") == "1":
        return
    # WAYLAND_DISPLAY boş olsa bile GTK varsayılan Wayland soketini dener;
    # X sunucusu (ya da XWayland) varsa her durumda X11'i seç.
    if os.environ.get("DISPLAY"):
        os.environ.setdefault("GDK_BACKEND", "x11")


def main(argv=None):
    argv = list(sys.argv if argv is None else argv)
    if any(a.lower() in ("--preload", "/preload") for a in argv[1:]):
        # Ön yükleme: modülleri ve görselleri disk önbelleğine al, çık.
        try:
            import gi
            gi.require_version("Gtk", "3.0")
            from gi.repository import Gtk  # noqa: F401
            from . import resources
            for name in ("titlebar", "pen", "hand", "eraser", "shapes"):
                resources.image(name)
        except Exception:
            pass
        return 0
    if any(a in ("-v", "--version", "--surum") for a in argv[1:]):
        from . import VERSION
        print("Fatih Kalem %s (Pardus/Linux) - By YhySvm" % VERSION)
        return 0

    _prefer_x11()

    import gi
    gi.require_version("Gtk", "3.0")
    from gi.repository import Gio, GLib, Gtk

    from . import APP_ID
    from .config import Settings
    from .window import KalemWindow

    monitor = None
    for i, a in enumerate(argv[1:]):
        if a in ("--ekran", "--monitor") and i + 2 < len(argv):
            try:
                monitor = int(argv[i + 2])
            except ValueError:
                pass

    GLib.set_prgname("fatih-kalem")
    GLib.set_application_name("Fatih Kalem")

    class KalemApp(Gtk.Application):
        def __init__(self):
            super().__init__(application_id=APP_ID,
                             flags=Gio.ApplicationFlags.HANDLES_COMMAND_LINE)
            self.window = None

        def do_command_line(self, command_line):
            self.activate()
            return 0

        def do_activate(self):
            if self.window is None:
                self.window = KalemWindow(self, Settings(), monitor)
                self.window.show_all()
            else:
                self.window.present()

    app = KalemApp()
    return app.run([argv[0]])


if __name__ == "__main__":
    sys.exit(main())
