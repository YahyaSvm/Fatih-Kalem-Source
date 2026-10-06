"""Ayarlar penceresi.

Orijinaldeki ayar bölümlerinin aynısı: Başlangıç Kalemi, Başlangıç Konumu,
Sık Kullanılanlar, Hızlı Erişim, Kapatma Onayı, Otomatik Başlatma ve
Hakkında. Dokunmatik tahtada rahat kullanılsın diye düğmeler büyük tutuldu.
"""

import gi

gi.require_version("Gtk", "3.0")
from gi.repository import Gdk, GdkPixbuf, Gtk  # noqa: E402

from . import (APP_NAME, AUTHOR_NAME, GITHUB_PROFILE, GITHUB_URL,  # noqa: E402
               PORT_AUTHOR, VERSION, system)
from . import resources as res  # noqa: E402
from . import styles as st  # noqa: E402
from .config import DEFAULTS, hex_to_rgb  # noqa: E402
from .i18n import _  # noqa: E402
from .toolbar import BYLINE_TEXT  # noqa: E402

CSS = b"""
.fk-settings { font-size: 13pt; }
.fk-title { font-weight: bold; font-size: 15pt; color: #525252; }
.fk-text { color: #525252; }
.fk-link { color: #0066CC; padding: 8px 18px; }
.fk-byline { color: #6e6e6e; font-weight: bold; }
.fk-settings stacksidebar row { padding: 10px 14px; }
.fk-settings checkbutton { padding: 6px 0; }
"""

FAV_TITLES = {
    "Undo": "Geri Al", "Redo": "Yinele", "Curtain": "Perde", "Library": "Kütüphane",
    "Background": "Arka Plan Sayfası", "Line": "Çizgi", "DashLine": "Kesikli Çizgi",
    "Arrow": "Ok", "Rectangle": "Dikdörtgen", "Ellipse": "Elips", "Triangle": "Üçgen",
}


def fav_title(tag):
    if tag in FAV_TITLES:
        return FAV_TITLES[tag]
    if tag.startswith("Eraser"):
        return "Silgi %s" % tag[6:]
    if tag.startswith("Size"):
        return "Kalınlık %s" % tag[4:]
    for style, name in st.STYLE_NAMES.items():
        if tag.startswith(name):
            rest = tag[len(name):]
            colors = {"Red": "Kırmızı", "Blue": "Mavi", "Green": "Yeşil",
                      "Orange": "Turuncu", "Yellow": "Sarı", "Black": "Siyah",
                      "White": "Beyaz", "ColorFul": "Renk Kartelası",
                      "NoPen": "Kalemsiz"}
            return "%s - %s" % (st.STYLE_TITLES[style], colors.get(rest, rest))
    return tag


def _surface_pixbuf(name, size=None):
    path = res.data_dir() + "/images/" + name.lower() + ".png"
    try:
        if size:
            return GdkPixbuf.Pixbuf.new_from_file_at_size(path, size, size)
        return GdkPixbuf.Pixbuf.new_from_file(path)
    except Exception:
        return None


class SettingsWindow(Gtk.Window):
    def __init__(self, kalem):
        super().__init__(title=_("Fatih Kalem - Ayarlar"))
        self.kalem = kalem
        self.s = kalem.settings
        self.set_transient_for(kalem)
        self.set_keep_above(True)
        self.set_type_hint(Gdk.WindowTypeHint.DIALOG)
        self.set_position(Gtk.WindowPosition.CENTER_ALWAYS)
        self.set_default_size(760, 480)
        self.set_icon_name("fatih-kalem")
        self.get_style_context().add_class("fk-settings")

        provider = Gtk.CssProvider()
        provider.load_from_data(CSS)
        Gtk.StyleContext.add_provider_for_screen(
            Gdk.Screen.get_default(), provider,
            Gtk.STYLE_PROVIDER_PRIORITY_APPLICATION)

        header = Gtk.HeaderBar(title=_("Ayarlar"), show_close_button=True)
        self.set_titlebar(header)

        self.stack = Gtk.Stack(transition_type=Gtk.StackTransitionType.CROSSFADE,
                               transition_duration=200)
        sidebar = Gtk.StackSidebar(stack=self.stack)
        sidebar.set_size_request(220, -1)
        box = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL)
        box.pack_start(sidebar, False, False, 0)
        box.pack_start(Gtk.Separator(orientation=Gtk.Orientation.VERTICAL), False, False, 0)
        box.pack_start(self.stack, True, True, 0)
        self.add(box)

        self.stack.add_titled(self._page_pen(), "pen", _("Başlangıç Kalemi"))
        self.stack.add_titled(self._page_position(), "position", _("Başlangıç Konumu"))
        self.stack.add_titled(self._page_favourites(), "favourites", _("Sık Kullanılanlar"))
        self.stack.add_titled(self._page_speed(), "speed", _("Hızlı Erişim"))
        self.stack.add_titled(self._page_close(), "close", _("Kapatma Onayı"))
        self.stack.add_titled(self._page_autostart(), "autostart", _("Otomatik Başlatma"))
        self.stack.add_titled(self._page_appearance(), "appearance", _("Görünüm"))
        self.stack.add_titled(self._page_advanced(), "advanced", _("Gelişmiş"))
        self.stack.add_titled(self._page_about(), "about", _("Hakkında"))
        self.connect("key-press-event", self._on_key)
        self.show_all()

    def _on_key(self, _w, event):
        if event.keyval == Gdk.KEY_Escape:
            self.destroy()
            return True
        return False

    # ------------------------------------------------------------ yardımcı
    @staticmethod
    def _page():
        page = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=14)
        page.set_border_width(24)
        return page

    @staticmethod
    def _label(text, cls="fk-text", wrap=True):
        lbl = Gtk.Label(label=text, xalign=0)
        lbl.set_line_wrap(wrap)
        lbl.set_max_width_chars(50)
        lbl.get_style_context().add_class(cls)
        return lbl

    @staticmethod
    def _button(text, callback):
        btn = Gtk.Button(label=text)
        btn.set_size_request(240, 46)
        btn.set_halign(Gtk.Align.START)
        btn.connect("clicked", lambda *_: callback())
        return btn

    def _check(self, text, key, on_toggle=None):
        chk = Gtk.CheckButton(label=text)
        chk.set_active(bool(self.s[key]))

        def toggled(btn):
            self.s[key] = btn.get_active()
            self.s.save()
            self.kalem.settings_changed()
            if on_toggle:
                on_toggle(btn.get_active())
        chk.connect("toggled", toggled)
        return chk

    # -------------------------------------------------------- Başlangıç Kalemi
    def _page_pen(self):
        page = self._page()
        page.pack_start(self._label("Program açıldığında kullanılacak kalem", "fk-title"),
                        False, False, 0)
        self.pen_info = self._label("")
        page.pack_start(self.pen_info, False, False, 0)
        self.pen_swatch = Gtk.DrawingArea()
        self.pen_swatch.set_size_request(48, 48)
        self.pen_swatch.set_halign(Gtk.Align.START)
        self.pen_swatch.connect("draw", self._draw_swatch)
        page.pack_start(self.pen_swatch, False, False, 0)
        page.pack_start(self._button("Geçerli Kalemi Kaydet", self._save_pen), False, False, 0)
        page.pack_start(self._button("Varsayılan", self._default_pen), False, False, 0)
        self._refresh_pen()
        return page

    def _refresh_pen(self):
        style = self.s["PenStyle"]
        self.pen_info.set_text(_("Kalem tipi : %s\nBoyut : %d\nRenk :") % (
            st.STYLE_TITLES.get(style, "?"), self.s["InkSize"]))
        self.pen_swatch.queue_draw()

    def _draw_swatch(self, _w, cr):
        style, no = self.s["PenStyle"], self.s["ColorNo"]
        rgb = st.color_for(style, no, hex_to_rgb(self.s["InkColor"]))
        cr.arc(24, 24, 18, 0, 6.2832)
        cr.set_source_rgb(*rgb)
        cr.fill_preserve()
        cr.set_source_rgb(0.5, 0.5, 0.5)
        cr.set_line_width(1)
        cr.stroke()

    def _save_pen(self):
        self.kalem.save_current_pen()
        self._refresh_pen()

    def _default_pen(self):
        for key in ("PenStyle", "ColorNo", "InkSize", "InkColor"):
            self.s[key] = DEFAULTS[key]
        self.s.save()
        self._refresh_pen()

    # ------------------------------------------------------- Başlangıç Konumu
    def _page_position(self):
        page = self._page()
        page.pack_start(self._label("Program açıldığında menünün duracağı yer", "fk-title"),
                        False, False, 0)
        self.mini_screen = Gtk.DrawingArea()
        self.mini_screen.set_size_request(192, 108)
        self.mini_screen.set_halign(Gtk.Align.START)
        self.mini_screen.connect("draw", self._draw_mini_screen)
        page.pack_start(self.mini_screen, False, False, 0)
        page.pack_start(self._button("Geçerli Konumu Kaydet", self._save_position), False, False, 0)
        page.pack_start(self._button("Varsayılan", self._default_position), False, False, 0)
        return page

    def _draw_mini_screen(self, _w, cr):
        k = self.kalem
        cr.set_source_rgb(0.93, 0.93, 0.95)
        cr.rectangle(0, 0, 192, 108)
        cr.fill()
        cr.set_source_rgb(0.6, 0.6, 0.6)
        cr.rectangle(0.5, 0.5, 191, 107)
        cr.stroke()
        if self.s["isStartupPositionDefault"]:
            left = 8
            top = (k.H - (223 + 42 * len(k.favourites))) / 3.0
        else:
            left, top = self.s["Left"], self.s["Top"]
        x = 1 + left * 192.0 / max(1, k.W)
        y = 1 + top * 108.0 / max(1, k.H)
        pb = _surface_pixbuf("minimainmenu")
        if pb is not None:
            Gdk.cairo_set_source_pixbuf(cr, pb, x, y)
            cr.paint()

    def _save_position(self):
        self.kalem.save_current_position()
        self.mini_screen.queue_draw()

    def _default_position(self):
        self.s["isStartupPositionDefault"] = True
        self.s["Left"] = 0
        self.s["Top"] = 0
        self.s.save()
        self.mini_screen.queue_draw()

    # ------------------------------------------------------ Sık Kullanılanlar
    def _page_favourites(self):
        page = self._page()
        page.pack_start(self._label(
            _("Sık kullandığınız komutları ana menüye eklemek için üzerine uzun basın "
              "(farede sağ tıklayın). Komutu ana menüden kaldırmak için üzerine uzun basın.")),
            False, False, 0)
        self.fav_store = Gtk.ListStore(GdkPixbuf.Pixbuf, str, str)
        view = Gtk.TreeView(model=self.fav_store, headers_visible=False, reorderable=True)
        view.append_column(Gtk.TreeViewColumn("", Gtk.CellRendererPixbuf(), pixbuf=0))
        view.append_column(Gtk.TreeViewColumn("", Gtk.CellRendererText(), text=1))
        self.fav_view = view
        scroll = Gtk.ScrolledWindow()
        scroll.set_min_content_height(180)
        scroll.add(view)
        page.pack_start(scroll, True, True, 0)
        row = Gtk.Box(spacing=8)
        for text, cb in (("Yukarı", lambda: self._move_fav(-1)),
                         ("Aşağı", lambda: self._move_fav(1)),
                         ("Kaldır", self._remove_fav)):
            b = Gtk.Button(label=text)
            b.set_size_request(110, 42)
            b.connect("clicked", lambda _b, f=cb: f())
            row.pack_start(b, False, False, 0)
        page.pack_start(row, False, False, 0)
        page.pack_start(self._button("Geçerli Sıralamayı Kaydet", self._save_favs), False, False, 0)
        page.pack_start(self._button("Varsayılan", self._default_favs), False, False, 0)
        self._load_favs()
        return page

    def _load_favs(self):
        self.fav_store.clear()
        for tag in self.kalem.favourites:
            self.fav_store.append([_surface_pixbuf("fav" + tag, 32), fav_title(tag), tag])

    def _selected_index(self):
        model, it = self.fav_view.get_selection().get_selected()
        if it is None:
            return None
        return model.get_path(it).get_indices()[0]

    def _move_fav(self, delta):
        i = self._selected_index()
        if i is None:
            return
        j = i + delta
        if 0 <= j < len(self.fav_store):
            a = self.fav_store.get_iter(i)
            b = self.fav_store.get_iter(j)
            self.fav_store.swap(a, b)

    def _remove_fav(self):
        i = self._selected_index()
        if i is not None:
            self.fav_store.remove(self.fav_store.get_iter(i))
            self._save_favs()

    def _save_favs(self):
        self.s["Favourites"] = [row[2] for row in self.fav_store]
        self.s.save()
        self.kalem.favourites_changed()

    def _default_favs(self):
        self.s["Favourites"] = []
        self.s.save()
        self.kalem.favourites_changed()
        self._load_favs()

    # ----------------------------------------------------------- Hızlı Erişim
    def _page_speed(self):
        page = self._page()
        pics = Gtk.Box(spacing=16)
        for name in ("settingsgestureellipse", "settingsgesturehand2finger"):
            pb = _surface_pixbuf(name)
            if pb is not None:
                pics.pack_start(Gtk.Image.new_from_pixbuf(pb), False, False, 0)
        page.pack_start(pics, False, False, 0)
        short = self._check("Kısa çekince programı yanıma getir.", "isShortGestureEnabled")
        short.set_margin_start(24)
        short.set_sensitive(self.s["isGestureEnabled"])
        page.pack_start(self._check("Çift parmakla çekince hızlı erişim menüsünü aç.",
                                    "isGestureEnabled", short.set_sensitive), False, False, 0)
        page.pack_start(short, False, False, 0)
        page.pack_start(self._check("Ekran alt kenarlarında çağırma oklarını göster.",
                                    "isSideArrowsEnabled"), False, False, 0)
        page.pack_start(self._label(
            _("Fare ile: sağ tuşa basılı tutup sola (kırmızı), sağa (mavi), yukarı (siyah) "
              "ya da aşağı (silgi) çekin.")), False, False, 0)
        return page

    # ---------------------------------------------------------- Kapatma Onayı
    def _page_close(self):
        page = self._page()
        page.pack_start(self._check("Programı kapatırken onay iste.",
                                    "isCloseConfirmationEnabled"), False, False, 0)
        return page

    # ------------------------------------------------------ Otomatik Başlatma
    def _page_autostart(self):
        page = self._page()
        state = system.autostart_state()
        self.chk_auto = Gtk.CheckButton(label=_("Sistem açılışında otomatik başlat"))
        self.chk_pre = Gtk.CheckButton(
            label=_("Sistem açılışında ön yükleme yap. (Faz 1 tahtaları için önerilir.)"))
        self.chk_auto.set_active(state == "start")
        self.chk_pre.set_active(state == "preload")
        self.chk_pre.set_sensitive(state != "start")
        self.chk_auto.connect("toggled", self._auto_toggled)
        self.chk_pre.connect("toggled", self._pre_toggled)
        page.pack_start(self.chk_auto, False, False, 0)
        page.pack_start(self.chk_pre, False, False, 0)
        page.pack_start(self._label(
            _("Ön yükleme, program dosyalarını açılışta belleğe alıp hemen kapanır; "
              "böylece kalem ilk açılışta daha hızlı gelir.")), False, False, 0)
        return page

    def _auto_toggled(self, chk):
        if chk.get_active():
            self.chk_pre.handler_block_by_func(self._pre_toggled)
            self.chk_pre.set_active(False)
            self.chk_pre.handler_unblock_by_func(self._pre_toggled)
            self.chk_pre.set_sensitive(False)
            system.set_autostart("start")
        else:
            self.chk_pre.set_sensitive(True)
            system.set_autostart(None)
        self.s["AutoStart"] = chk.get_active()
        self.s["PreLoad"] = False
        self.s.save()

    def _pre_toggled(self, chk):
        if self.chk_auto.get_active():
            return
        system.set_autostart("preload" if chk.get_active() else None)
        self.s["PreLoad"] = chk.get_active()
        self.s.save()

    # ------------------------------------------------------------- Görünüm
    def _combo(self, title, options, key, cast=str):
        row = Gtk.Box(spacing=12)
        lbl = self._label(title, wrap=False)
        lbl.set_size_request(170, -1)
        row.pack_start(lbl, False, False, 0)
        combo = Gtk.ComboBoxText()
        values = []
        for value, text in options:
            combo.append_text(text)
            values.append(value)
        current = self.s[key]
        combo.set_active(values.index(current) if current in values else 0)

        def changed(c):
            self.s[key] = cast(values[c.get_active()])
            self.s.save()
            self.kalem.settings_changed()
        combo.connect("changed", changed)
        row.pack_start(combo, False, False, 0)
        return row

    def _page_appearance(self):
        page = self._page()
        page.pack_start(self._combo(_("Dil"), [("auto", _("Otomatik")), ("tr", "Türkçe"),
                                               ("en", "English")], "Language"), False, False, 0)
        page.pack_start(self._combo(_("Menü boyutu"),
                                    [(1.0, "%100"), (1.25, "%125"), (1.5, "%150"), (2.0, "%200")],
                                    "MenuScale", float), False, False, 0)
        page.pack_start(self._combo(_("Alt menüler"),
                                    [("auto", _("Otomatik (yer varsa sağda)")),
                                     ("left", _("Her zaman solda (sol el)")),
                                     ("right", _("Her zaman sağda"))], "SubmenuSide"),
                        False, False, 0)
        display = Gdk.Display.get_default()
        screens = [(-1, _("Birincil ekran"))]
        screens += [(i, _("Ekran %d") % (i + 1)) for i in range(display.get_n_monitors())]
        if display.get_n_monitors() > 1:
            screens.append((-2, _("Tüm ekranlar")))
        page.pack_start(self._combo(_("Ekran"), screens, "Monitor", int), False, False, 0)
        page.pack_start(self._label(_("Ekran değişikliği programı yeniden başlatınca uygulanır.")),
                        False, False, 0)
        page.pack_start(self._check(_("Renk körlüğüne uygun renkler"), "ColorBlindPalette"),
                        False, False, 0)
        return page

    # ------------------------------------------------------------ Gelişmiş
    def _page_advanced(self):
        page = self._page()
        page.pack_start(self._check(_("El yazısını yumuşat"), "SmoothInk"), False, False, 0)
        page.pack_start(self._check(_("Elle çizilen şekilleri düzelt (şekil tanıma)"),
                                    "ShapeRecognition"), False, False, 0)
        page.pack_start(self._check(_("Dersi her dakika otomatik kaydet"), "Autosave"),
                        False, False, 0)
        page.pack_start(self._check(_("Güncellemeleri denetle"), "CheckUpdates"), False, False, 0)
        page.pack_start(self._button(_("Ayarları dışa aktar"), self._export_settings), False, False, 0)
        page.pack_start(self._button(_("Ayarları içe aktar"), self._import_settings), False, False, 0)
        page.pack_start(self._button(_("Hata raporu oluştur"), self._bug_report), False, False, 0)
        return page

    def _pick_file(self, title, action, name=None):
        dlg = Gtk.FileChooserDialog(title=title, transient_for=self, action=action)
        dlg.add_buttons(_("Vazgeç"), Gtk.ResponseType.CANCEL,
                        _("Kaydet") if action == Gtk.FileChooserAction.SAVE else _("Aç"),
                        Gtk.ResponseType.ACCEPT)
        if name:
            dlg.set_current_name(name)
            dlg.set_do_overwrite_confirmation(True)
        flt = Gtk.FileFilter()
        flt.set_name("JSON")
        flt.add_pattern("*.json")
        dlg.add_filter(flt)
        resp = dlg.run()
        path = dlg.get_filename() if resp == Gtk.ResponseType.ACCEPT else None
        dlg.destroy()
        return path

    def _message(self, text, error=False):
        dlg = Gtk.MessageDialog(transient_for=self, modal=True,
                                message_type=Gtk.MessageType.ERROR if error else Gtk.MessageType.INFO,
                                buttons=Gtk.ButtonsType.OK, text=text)
        dlg.run()
        dlg.destroy()

    def _export_settings(self):
        path = self._pick_file(_("Ayarları dışa aktar"), Gtk.FileChooserAction.SAVE,
                               "fatih-kalem-ayarlar.json")
        if path:
            try:
                self.s.export_to(path)
                self._message(_("Kaydedildi: %s") % path)
            except OSError as e:
                self._message(_("Kaydedilemedi: %s") % e, True)

    def _import_settings(self):
        path = self._pick_file(_("Ayarları içe aktar"), Gtk.FileChooserAction.OPEN)
        if not path:
            return
        try:
            self.s.import_from(path)
        except (OSError, ValueError) as e:
            self._message(_("Açılamadı: %s") % e, True)
            return
        self.kalem.favourites_changed()
        self.kalem.settings_changed()
        self._message(_("Ayarlar içe aktarıldı. Bazı ayarlar yeniden başlatınca uygulanır."))

    def _bug_report(self):
        from .diagnostics import write_report
        try:
            path, url = write_report(self.kalem.composited)
        except OSError as e:
            self._message(_("Kaydedilemedi: %s") % e, True)
            return
        self._message(_("Hata raporu kaydedildi:\n%s\n\nGitHub'da yeni bir kayıt açılıyor; "
                        "dosyayı oraya ekleyin.") % path)
        try:
            Gtk.show_uri_on_window(self, url, Gdk.CURRENT_TIME)
        except Exception:
            pass

    # --------------------------------------------------------------- Hakkında
    def _page_about(self):
        page = self._page()
        pb = _surface_pixbuf("fatihpenlogo_wide")
        if pb is not None:
            img = Gtk.Image.new_from_pixbuf(pb)
            img.set_halign(Gtk.Align.START)
            page.pack_start(img, False, False, 0)
        short_version = ".".join(VERSION.split(".")[:2])     # 2.0.0 -> 2.0
        page.pack_start(self._label("%s %s" % (APP_NAME, short_version), "fk-title"),
                        False, False, 0)
        page.pack_start(self._label(
            "Fatih Kalem'in yeni sürümü. Pardus (ETAP dahil), Linux ve Windows için "
            "baştan elden geçirildi: dokunmatik tahtada iki parmak jestleri, kalem "
            "basıncı, HiDPI ekran desteği, yeni arka plan sayfaları ve daha kararlı "
            "geri al / yinele."), False, False, 0)

        dev = self._label(_("Geliştiren: %s (%s)") % (AUTHOR_NAME, PORT_AUTHOR), "fk-title")
        page.pack_start(dev, False, False, 0)
        links = Gtk.Grid(column_spacing=6, row_spacing=2)
        for i, (url, text) in enumerate(((GITHUB_URL, _("Proje sayfası (GitHub)")),
                                         (GITHUB_URL + "/releases", _("Yeni sürümler")),
                                         (GITHUB_URL + "/issues", _("Hata bildir")),
                                         (GITHUB_PROFILE, "GitHub: @YahyaSvm"))):
            link = Gtk.LinkButton.new_with_label(url, text)
            link.set_halign(Gtk.Align.START)
            links.attach(link, i % 2, i // 2, 1, 1)
        page.pack_start(links, False, False, 0)
        page.pack_start(self._label(BYLINE_TEXT, "fk-byline"), False, False, 0)

        info = "Sürüm %s  |  Oturum: %s  |  Masaüstü: %s  |  Saydamlık: %s%s" % (
            VERSION, system.session_type(), system.desktop_name() or "?",
            "var" if self.kalem.composited else "yok (ekran görüntüsü kipi)",
            "  |  Pardus ETAP" if system.is_etap() else
            ("  |  Pardus" if system.is_pardus() else ""))
        page.pack_start(self._label(info), False, False, 0)
        page.pack_start(self._label(
            "İlk Fatih Kalem (1.0): Hasan Yunus ATEŞ, MEB YEĞİTEK.", "fk-text"),
            False, False, 0)
        scroll = Gtk.ScrolledWindow()
        scroll.set_policy(Gtk.PolicyType.NEVER, Gtk.PolicyType.AUTOMATIC)
        scroll.add(page)
        return scroll
