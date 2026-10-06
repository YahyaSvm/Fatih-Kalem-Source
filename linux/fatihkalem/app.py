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

    import signal

    from . import APP_ID, i18n, update
    from .config import Settings
    from .diagnostics import setup_logging

    log = setup_logging()
    settings = Settings()
    i18n.set_language(settings["Language"])

    from .window import KalemWindow   # çeviri dili ayarlandıktan sonra

    def parse(args):
        monitor, files = None, []
        it = iter(range(1, len(args)))
        for i in it:
            a = args[i]
            if a in ("--ekran", "--monitor") and i + 1 < len(args):
                try:
                    monitor = int(args[i + 1])
                except ValueError:
                    pass
                next(it, None)
            elif not a.startswith("-") and a.lower().endswith(".fkalem"):
                files.append(a)
        return monitor, files

    GLib.set_prgname("fatih-kalem")
    GLib.set_application_name("Fatih Kalem")

    class KalemApp(Gtk.Application):
        def __init__(self):
            super().__init__(application_id=APP_ID,
                             flags=Gio.ApplicationFlags.HANDLES_COMMAND_LINE)
            self.window = None
            self.set_property("register-session", True)

        def do_command_line(self, command_line):
            args = command_line.get_arguments()
            monitor, files = parse(args)
            cwd = command_line.get_cwd() or os.getcwd()
            self._start(monitor)
            for f in files:
                self.window.open_lesson(os.path.join(cwd, f))
            return 0

        def do_activate(self):
            self._start(None)

        def _start(self, monitor):
            if self.window is None:
                self.window = KalemWindow(self, settings, monitor)
                self.window.show_all()
                self.connect("query-end", lambda *_a: self.window.autosave_now())
                GLib.timeout_add_seconds(15, self._check_updates)
            else:
                self.window.present()

        def _check_updates(self):
            win = self.window
            update.check_async(settings, lambda tag, url: GLib.idle_add(
                win.new_version_available, tag, url))
            return False

    app = KalemApp()

    def on_term(*_a):
        # Tahta kapatılırken / oturum sonlanırken dersi kaydet
        if app.window is not None:
            log.info("Sonlandırma sinyali: otomatik kayıt")
            app.window.autosave_now()
        app.quit()
        return False

    for sig in (signal.SIGTERM, signal.SIGHUP, signal.SIGINT):
        GLib.unix_signal_add(GLib.PRIORITY_DEFAULT, sig, on_term)
    return app.run(argv)


if __name__ == "__main__":
    sys.exit(main())
