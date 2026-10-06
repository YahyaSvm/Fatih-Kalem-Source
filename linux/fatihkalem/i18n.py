"""Basit arayüz çevirisi.

Kaynak dil Türkçedir; `_("Türkçe metin")` seçili dile çevirir. Dil ayarı
"auto" ise sistem dili (LANG) Türkçe değilse İngilizce kullanılır.
"""

import os

_LANG = "tr"


def set_language(lang):
    global _LANG
    if lang in (None, "", "auto"):
        env = (os.environ.get("LANGUAGE") or os.environ.get("LC_ALL")
               or os.environ.get("LC_MESSAGES") or os.environ.get("LANG") or "tr")
        lang = "tr" if env.lower().startswith("tr") else "en"
    _LANG = lang if lang in ("tr", "en") else "tr"


def language():
    return _LANG


def _(text):
    if _LANG == "tr":
        return text
    return EN.get(text, text)


EN = {
    # --- genel
    "Kapat": "Close",
    "Vazgeç": "Cancel",
    "Tamam": "OK",
    "Kaydet": "Save",
    "Aç": "Open",
    "Evet": "Yes",
    "Hayır": "No",
    "Sil": "Delete",
    "Ayarlar": "Settings",
    "Varsayılan": "Default",
    "Fatih Kalem - Ayarlar": "Fatih Kalem - Settings",
    "Resim Dosyaları": "Image files",
    "Arka Plan Sayfası Seç": "Choose background page",
    "Görsel Seç": "Choose image",
    # --- araçlar menüsü
    "Seç": "Select",
    "Metin": "Text",
    "Lazer": "Laser",
    "Kaybolan": "Fading",
    "Kesikli": "Dashed",
    "Noktalı": "Dotted",
    "Oklu": "Arrow pen",
    "Normal": "Normal",
    "Cetvel": "Ruler",
    "Gönye": "Set square",
    "İletki": "Protractor",
    "Pergel": "Compass",
    "Spot": "Spotlight",
    "Büyüteç": "Magnifier",
    "Sayaç": "Timer",
    "Kura": "Draw lots",
    "Dolgu": "Fill",
    "Ekran": "Screen",
    "Temizle": "Clear",
    "Tanıma": "Recognize",
    "PNG": "PNG",
    "PDF": "PDF",
    "Ders kaydet": "Save lesson",
    "Ders aç": "Open lesson",
    "Paylaş": "Share",
    "Yeni sayfa": "New page",
    "Sayfa sil": "Delete page",
    "Önceki": "Previous",
    "Sonraki": "Next",
    "Son renkler": "Recent colours",
    "Renk kaydet": "Save colour",
    "Sayfa %d / %d": "Page %d / %d",
    # --- iletiler
    "Tüm çizimler silinsin mi?": "Delete all drawings?",
    "Bu sayfadaki her şey silinir. Geri al ile geri getirebilirsiniz.":
        "Everything on this page will be deleted. You can bring it back with Undo.",
    "Bu sayfa silinsin mi?": "Delete this page?",
    "Metin yazın": "Type text",
    "Yazı boyutu": "Text size",
    "Ekle": "Add",
    "Çizimleri resim olarak kaydet": "Save drawing as image",
    "Tüm sayfaları PDF olarak kaydet": "Save all pages as PDF",
    "Dersi kaydet": "Save lesson",
    "Ders aç": "Open lesson",
    "Fatih Kalem dersi": "Fatih Kalem lesson",
    "Kaydedildi: %s": "Saved: %s",
    "Kaydedilemedi: %s": "Could not save: %s",
    "Açılamadı: %s": "Could not open: %s",
    "Telefonunuzla QR kodu okutun": "Scan the QR code with your phone",
    "Aynı ağdaki cihazlar bu adresten indirebilir:":
        "Devices on the same network can download from:",
    "Ağ bağlantısı bulunamadı.": "No network connection found.",
    "Paylaşımı durdur": "Stop sharing",
    "Sayaç": "Timer",
    "Kronometre": "Stopwatch",
    "Başlat": "Start",
    "Durdur": "Pause",
    "Sıfırla": "Reset",
    "Süre doldu!": "Time is up!",
    "Kura çek": "Draw lots",
    "Öğrenci listesi (her satıra bir isim)": "Student list (one name per line)",
    "ya da numara aralığı": "or number range",
    "Seçilen": "Selected",
    "Seçilenleri tekrar seçme": "Do not pick the same one again",
    "Çek": "Draw",
    "Yeni bir sürüm var: %s": "A new version is available: %s",
    "İndirme sayfasını aç": "Open download page",
    "Otomatik kayıt": "Autosave",
    # --- ayarlar penceresi
    "Başlangıç Kalemi": "Starting Pen",
    "Başlangıç Konumu": "Starting Position",
    "Sık Kullanılanlar": "Favourites",
    "Hızlı Erişim": "Quick Access",
    "Kapatma Onayı": "Close Confirmation",
    "Otomatik Başlatma": "Autostart",
    "Görünüm": "Appearance",
    "Gelişmiş": "Advanced",
    "Hakkında": "About",
    "Program açıldığında kullanılacak kalem": "Pen used when the program starts",
    "Kalem tipi : %s\nBoyut : %d\nRenk :": "Pen type : %s\nSize : %d\nColour :",
    "Geçerli Kalemi Kaydet": "Save Current Pen",
    "Program açıldığında menünün duracağı yer": "Where the menu appears at startup",
    "Geçerli Konumu Kaydet": "Save Current Position",
    "Sık kullandığınız komutları ana menüye eklemek için üzerine uzun basın "
    "(farede sağ tıklayın). Komutu ana menüden kaldırmak için üzerine uzun basın.":
        "Long-press a command (right-click with a mouse) to add it to the main menu. "
        "Long-press it in the main menu to remove it.",
    "Yukarı": "Up",
    "Aşağı": "Down",
    "Kaldır": "Remove",
    "Geçerli Sıralamayı Kaydet": "Save Current Order",
    "Kısa çekince programı yanıma getir.": "Bring the menu to me on a short drag.",
    "Çift parmakla çekince hızlı erişim menüsünü aç.":
        "Open the quick access menu with a two-finger drag.",
    "Ekran alt kenarlarında çağırma oklarını göster.":
        "Show call arrows at the bottom screen corners.",
    "Fare ile: sağ tuşa basılı tutup sola (kırmızı), sağa (mavi), yukarı (siyah) "
    "ya da aşağı (silgi) çekin.":
        "With a mouse: hold the right button and drag left (red), right (blue), "
        "up (black) or down (eraser).",
    "Programı kapatırken onay iste.": "Ask for confirmation when closing.",
    "Sistem açılışında otomatik başlat": "Start automatically at login",
    "Sistem açılışında ön yükleme yap. (Faz 1 tahtaları için önerilir.)":
        "Preload at login. (Recommended for older boards.)",
    "Ön yükleme, program dosyalarını açılışta belleğe alıp hemen kapanır; "
    "böylece kalem ilk açılışta daha hızlı gelir.":
        "Preloading loads the program files into memory at login and exits, "
        "so the pen opens faster later.",
    "Dil": "Language",
    "Otomatik": "Automatic",
    "Menü boyutu": "Menu size",
    "Alt menüler": "Submenus",
    "Otomatik (yer varsa sağda)": "Automatic (right if there is room)",
    "Her zaman solda (sol el)": "Always on the left (left-handed)",
    "Her zaman sağda": "Always on the right",
    "Renk körlüğüne uygun renkler": "Colour-blind friendly colours",
    "El yazısını yumuşat": "Smooth handwriting",
    "Elle çizilen şekilleri düzelt (şekil tanıma)": "Straighten hand-drawn shapes (shape recognition)",
    "Ekran": "Screen",
    "Birincil ekran": "Primary screen",
    "Tüm ekranlar": "All screens",
    "Ekran %d": "Screen %d",
    "Ekran değişikliği programı yeniden başlatınca uygulanır.":
        "The screen change applies after restarting the program.",
    "Güncellemeleri denetle": "Check for updates",
    "Dersi her dakika otomatik kaydet": "Autosave the lesson every minute",
    "Ayarları dışa aktar": "Export settings",
    "Ayarları içe aktar": "Import settings",
    "Hata raporu oluştur": "Create bug report",
    "Ayarlar içe aktarıldı. Bazı ayarlar yeniden başlatınca uygulanır.":
        "Settings imported. Some settings apply after a restart.",
    "Hata raporu kaydedildi:\n%s\n\nGitHub'da yeni bir kayıt açılıyor; dosyayı oraya ekleyin.":
        "Bug report saved:\n%s\n\nA new GitHub issue is opening; please attach the file.",
    "Geliştiren: %s (%s)": "Developer: %s (%s)",
    "Proje sayfası (GitHub)": "Project page (GitHub)",
    "Yeni sürümler": "Releases",
    "Hata bildir": "Report a bug",
}
