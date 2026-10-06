# Değişiklik Günlüğü

## 2.1.0 — 2026-10-06

### Yeni: Araçlar menüsü (Pardus / Linux)
Ana menüde şekillerin altına **Araçlar** düğmesi eklendi. İçindekiler:

- **Kalem çeşitleri:** normal, kesikli, noktalı ve oklu kalem
- **Şekil tanıma:** elle çizilen çizgi, daire, üçgen ve dikdörtgen düzgün şekle dönüşür
- **Seç:** çizimleri kementle seçip taşıma, büyütme, kopyalama ve silme
- **Metin:** ekrana yazı ekleme ve düzenleme (Ctrl+Enter ile ekle)
- **Lazer işaretçi** ve birkaç saniyede kendiliğinden silinen **kaybolan kalem**
- **Dolgu:** kapalı şekillerin içini boyama
- **Cetvel, gönye, iletki ve pergel:** kalem cetvelin kenarına yapışır, düz çizgi çekilir;
  pergelle yay ve çember çizilir
- **Spot ışığı, büyüteç, sayaç / kronometre ve kura çekme**
- **Ekran görüntüsünü sayfa yapma** ve üzerine yazma
- **Çok sayfalı tahta:** yeni sayfa, sayfalar arası geçiş, sayfa silme
- **Kaydetme:** PNG, tüm sayfaları tek PDF, ders dosyası (`.fkalem`) kaydetme ve açma;
  ders dosyasına çift tıklayınca açılır
- **Paylaş:** dersi PDF olarak aynı ağdaki telefonlara QR kodla gönderme
- **Son renkler** ve kendi renk paletiniz
- **Tümünü temizle** (onaylı, geri alınabilir)

### Yeni: Şablonlar
Dört çizgili defter (ilkokul), müzik portesi, koordinat düzlemi, sayı doğrusu ve
izometrik kâğıt.

### Yeni: Ayarlar
- **Görünüm:** dil (Türkçe / English), menü boyutu (%100–%200), alt menülerin yönü
  (sol el kullanımı), ekran seçimi (çoklu ekran), renk körlüğüne uygun renkler
- **Gelişmiş:** el yazısını yumuşatma, şekil tanıma, dakikada bir otomatik kayıt,
  güncelleme denetimi, ayarları dışa / içe aktarma, tek tıkla hata raporu

### Pardus / ETAP
- El moduna dönerken ve tahta kapatılırken ders otomatik kaydedilir
  (Araçlar → Ders aç ile geri gelir)
- Okul yöneticileri için ortak ayar dosyası: `/etc/fatih-kalem/ayarlar.json`
- ETAP ekran klavyesiyle uyum için erişilebilirlik köprüsü açık bırakıldı
- Yeni sürüm çıkınca bildirim

### Windows
- Kurulum sihirbazı (yönetici yetkisi gerekmez; isteğe bağlı masaüstü kısayolu ve açılışta başlatma)
- Yeni sürüm bildirimi
- Ekran başına yüksek DPI desteği (4K tahta, farklı ölçekli ikinci ekran)

### Altyapı
- 94 otomatik test; arayüz testleri GitHub'da sanal ekranda çalışır
- Çok uzun çizgiler parçalanarak çizim ve silgi hızlandırıldı

## 2.0.0 — 2026-10-06

### Yeni
- Pardus / Linux sürümü (Python 3 + GTK3): Pardus ETAP 23, Pardus 23/25
  (GNOME, XFCE), Debian ve Ubuntu desteği; `.deb` paketi.
- Wayland oturumlarında otomatik XWayland kullanımı.
- Saydamlık (bileşikleştirici) olmayan masaüstlerinde ekran görüntüsü kipi.
- İki parmak jestleri ve kısa çekince menüyü yanına getirme.
- Kalem basıncı ve kalemin silgi ucu desteği.
- HiDPI / 4K tahta desteği.
- Yeni arka plan sayfaları: beyaz, çizgili, kareli, milimetrik, noktalı,
  yeşil tahta, siyah tahta.
- Ana menünün en altında "By YhySvm" imzası.
- Ayarlar → Hakkında: geliştirici bilgisi ve GitHub bağlantıları.

### Düzeltmeler (Windows)
- Geri al / yinele, perde, kütüphane ve silgi geçmişinin bozulması
  giderildi (dizi boyutlandırmaları kayboluyordu).
- "Otomatik başlat" kapatıldığında program açılışta başlamaya devam
  ediyordu; düzeltildi.
- Otomatik başlatma kaydı boşluk içeren yollarda tırnaklı yazılıyor;
  kayıt defteri anahtarı yoksa program çökmüyor.
- Çökme durumunda kullanıcıya anlaşılır mesaj gösterilip
  `%LOCALAPPDATA%\Fatih Kalem\hata.log` dosyasına günlük yazılıyor.
