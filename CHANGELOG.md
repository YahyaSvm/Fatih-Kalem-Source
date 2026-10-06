# Değişiklik Günlüğü

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
