<div align="center">

<img src="linux/data/icons/256.png" width="96" alt="Fatih Kalem">

# Fatih Kalem 2.0

**Etkileşimli tahtalar için kalem programı — Windows ve Pardus / Linux**

[![Sürüm](https://img.shields.io/github/v/release/YahyaSvm/Fatih-Kalem-Source?label=s%C3%BCr%C3%BCm)](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases/latest)
[![İndirmeler](https://img.shields.io/github/downloads/YahyaSvm/Fatih-Kalem-Source/total?label=indirme)](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases)
![Pardus](https://img.shields.io/badge/Pardus-ETAP%20%7C%2023%20%7C%2025-1f8dd6)
![Windows](https://img.shields.io/badge/Windows-7%2B-0078d4)

[İndir](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases/latest) ·
[Pardus kurulum rehberi](linux/README.md) ·
[Hata bildir](https://github.com/YahyaSvm/Fatih-Kalem-Source/issues)

<img src="docs/images/ekran-kalem.png" width="820" alt="Fatih Kalem 2.0 ekran görüntüsü">

</div>

---

Fatih Kalem, akıllı tahtada ekrandaki her şeyin (e-kitap, sunum, video,
tarayıcı...) üzerine yazıp çizmenizi sağlar. **2.0 sürümü** Fatih Projesi
tahtalarında kullanılan Pardus ETAP için baştan yazıldı, Windows sürümü de
elden geçirildi.

## Yenilikler (2.0)

- 🐧 **Pardus / Linux desteği** — Pardus ETAP 23, Pardus 23/25 (GNOME, XFCE) ve
  diğer Debian tabanlı dağıtımlarda çalışır. Ek çalışma zamanı gerekmez;
  Pardus'ta hazır gelen Python 3 + GTK3 kullanılır.
- ✌️ **İki parmak jestleri** — iki parmakla çekerek renk ya da silgi seçin;
  kısa çekince menü yanınıza gelir.
- 🖊️ Kalem **basıncı** ve kalemin **silgi ucu** desteği.
- 🖥️ **HiDPI / 4K** tahta desteği.
- 📄 Yeni **arka plan sayfaları**: beyaz, çizgili, kareli, milimetrik, noktalı,
  yeşil ve siyah tahta (ekran çözünürlüğünde üretilir).
- ↩️ Daha kararlı **geri al / yinele** (perde, görsel ve silgi işlemleri dahil).
- 🚀 Açılışta otomatik başlatma ve hızlı açılış için **ön yükleme**.
- 🐛 Windows: otomatik başlatmanın kapatılamaması ve geri al/yinele
  hataları giderildi; çökme durumunda hata günlüğü tutulur.

## Özellikler

| | |
|---|---|
| **Kalemler** | Keçeli, dolma ve fosforlu kalem · 6 renk + 24 renklik kartela · 6 kalınlık |
| **Silgi** | 4 boyut, çizginin yalnızca değdiği kısmını siler |
| **Şekiller** | Çizgi, kesikli çizgi, ok, dikdörtgen, elips, üçgen |
| **Perde** | Ekranı kapatıp yalnızca seçilen alanı açık bırakır |
| **Kütüphane** | Görsel ekleme; taşıma, büyütme, döndürme |
| **Kalemsiz mod** | Çizimler kalır, tıklamalar alttaki uygulamaya geçer |
| **El modu** | Menü küçülür, masaüstü normal kullanılır |
| **Sık kullanılanlar** | Öğeye uzun basarak ana menüye ekleyin (en fazla 20) |

<p align="center">
  <img src="docs/images/menu.png" height="260" alt="Ana menü">
  &nbsp;&nbsp;
  <img src="docs/images/ekran-jest.png" height="260" alt="İki parmak jest menüsü">
</p>

## İndirme ve kurulum

En son sürümü **[Releases](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases/latest)**
sayfasından indirin.

### Pardus (ETAP, GNOME, XFCE)

`fatih-kalem_2.0.0_all.deb` dosyasına çift tıklayıp **Yükle**'ye basın ya da:

```bash
sudo apt install ./fatih-kalem_2.0.0_all.deb
```

Program **Uygulamalar → Eğitim → Fatih Kalem** altında görünür.
Wayland, saydamlık, dokunmatik ve otomatik başlatma ayrıntıları için
[Pardus kurulum rehberine](linux/README.md) bakın.

### Windows

`FatihKalem-2.0.0-windows.zip` dosyasını açın ve `Fatih Kalem.exe`'yi
çalıştırın. .NET Framework 4.8 gerekir (Windows 10/11'de hazır gelir).

## Kaynaktan derleme

```bash
# Pardus / Linux
cd linux
python3 -m fatihkalem        # doğrudan çalıştır
make test                    # testler (python3-pytest)
make deb                     # ../dist/fatih-kalem_<sürüm>_all.deb
```

```powershell
# Windows (.NET SDK)
dotnet build "Fatih Kalem.csproj" -c Release
```

## Proje yapısı

```
├── canvas/, canvas.My/, Properties/   Windows sürümü (WPF, C#)
├── resources/                         menü görselleri
├── linux/                             Pardus / Linux sürümü (Python 3 + GTK3)
│   ├── fatihkalem/                    uygulama kodu
│   ├── debian/                        Debian / Pardus paketleme
│   └── tests/                         birim testleri
├── tools/                             Windows yardımcı betikleri
└── docs/                              belgeler ve ekran görüntüleri
```

## Geliştirici

**Yahya Eren Sevim** — [@YahyaSvm](https://github.com/YahyaSvm)

Öneri ve hata bildirimleri için
[Issues](https://github.com/YahyaSvm/Fatih-Kalem-Source/issues) sayfasını kullanın.

## Haklar

İlk Fatih Kalem (1.0), Fizik Öğretmeni Hasan Yunus ATEŞ tarafından Milli
Eğitim Bakanlığı YEĞİTEK için yazılmıştır; menü görselleri o sürümden
gelmektedir ve hakları Milli Eğitim Bakanlığına aittir. Fatih Kalem 2.0,
Fatih Projesi etkileşimli tahtalarında kullanılmak üzere geliştirilmiştir.
