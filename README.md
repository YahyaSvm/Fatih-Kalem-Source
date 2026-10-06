<div align="center">

<img src="linux/data/icons/256.png" width="110" alt="Fatih Kalem logosu">

# Fatih Kalem 2.1

### Etkileşimli tahtalar için ekran kalemi

Ekrandaki her şeyin (e-kitap, sunum, video, tarayıcı) üzerine yazın, çizin, vurgulayın.
**Pardus ETAP** ve **Windows** çalıştıran Fatih Projesi tahtaları için geliştirildi.

[![Son sürüm](https://img.shields.io/github/v/release/YahyaSvm/Fatih-Kalem-Source?label=son%20s%C3%BCr%C3%BCm&color=f05a28)](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases/latest)
[![Derleme](https://github.com/YahyaSvm/Fatih-Kalem-Source/actions/workflows/release.yml/badge.svg)](https://github.com/YahyaSvm/Fatih-Kalem-Source/actions/workflows/release.yml)
[![İndirmeler](https://img.shields.io/github/downloads/YahyaSvm/Fatih-Kalem-Source/total?label=indirme&color=1e90ff)](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases)
[![Pardus](https://img.shields.io/badge/Pardus-ETAP%20%C2%B7%2023%20%C2%B7%2025-1f8dd6)](#platform-desteği)
[![Windows](https://img.shields.io/badge/Windows-7%20%C2%B7%2010%20%C2%B7%2011-0078d4)](#platform-desteği)

**[⬇ İndir](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases/latest)** &nbsp;·&nbsp;
[Kurulum](#kurulum) &nbsp;·&nbsp;
[Kullanım](#kullanım) &nbsp;·&nbsp;
[Pardus Rehberi](linux/README.md) &nbsp;·&nbsp;
[Hata Bildir](https://github.com/YahyaSvm/Fatih-Kalem-Source/issues/new)

<br>

<img src="docs/images/ekran-kalem.png" width="860" alt="Fatih Kalem — kareli sayfa üzerinde çizim ve açık kalem menüsü">

</div>

<br>

## İçindekiler

- [Neden Fatih Kalem 2?](#neden-fatih-kalem-2)
- [Özellikler](#özellikler)
- [Ekran Görüntüleri](#ekran-görüntüleri)
- [Platform Desteği](#platform-desteği)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Sık Sorulan Sorular](#sık-sorulan-sorular)
- [Kaynaktan Derleme](#kaynaktan-derleme)
- [Proje Yapısı](#proje-yapısı)
- [Yol Haritası](#yol-haritası)
- [Katkıda Bulunma](#katkıda-bulunma)
- [Haklar ve Teşekkür](#haklar-ve-teşekkür)

---

## Neden Fatih Kalem 2?

Okullardaki etkileşimli tahtalar artık büyük ölçüde **Pardus ETAP** ile çalışıyor,
ancak öğretmenlerin alıştığı Fatih Kalem yalnızca Windows'ta vardı. Fatih Kalem 2 bu
boşluğu kapatır ve tahtaya yeni ders araçları getirir:

- **Aynı arayüz, her iki sistemde.** Menü, simgeler, renkler ve dokunma noktaları
  Windows ve Pardus'ta birebir aynıdır; öğretmenin yeniden öğrenmesi gereken bir şey yoktur.
- **Ek kurulum gerektirmez.** Pardus sürümü, sistemde hazır gelen Python 3 ve GTK3
  ile çalışır; internet bağlantısı olmadan tek paketle kurulur.
- **Tahta için tasarlandı.** İki parmak jestleri, kalem basıncı, silgi ucu,
  büyük dokunma alanları ve 4K ekran desteği.
- **Daha kararlı.** Windows sürümündeki geri al/yinele ve otomatik başlatma
  hataları giderildi; beklenmeyen hatalar kayıt altına alınır.

## Özellikler

<table>
<tr>
<td width="50%" valign="top">

#### ✏️ Yazma ve çizme
- **3 kalem tipi:** keçeli, dolma, fosforlu
- **6 hazır renk** ve **24 renklik kartela**
- **6 kalınlık**, basınca duyarlı kalem desteği
- Fosforlu kalem yazının **altında** kalır, üst üste geçince koyulaşmaz

#### 📐 Şekiller
- Çizgi, kesikli çizgi, ok
- Dikdörtgen, elips, üçgen

#### 🧽 Silgi ve geçmiş
- **4 boyutta silgi**; çizginin yalnızca değdiği kısmını siler
- Kalemin **silgi ucu** otomatik olarak silgi olur
- Çok adımlı **geri al / yinele**

</td>
<td width="50%" valign="top">

#### 🖐️ Tahta deneyimi
- **İki parmak jestleri:** çekilen yöne göre renk ya da silgi
- **Kısa çekiş:** menü parmağınızın yanına gelir
- **El modu:** menü küçülür, masaüstü normal kullanılır
- **Kalemsiz mod:** çizimler kalır, tıklamalar alttaki uygulamaya geçer
- Sürüklenip fırlatılabilen menü, ekran kenarı **çağırma okları**

#### 🎓 Ders araçları
- **Perde:** ekranın yalnızca seçilen kısmını gösterir
- **Görsel kütüphanesi:** taşı, büyüt, döndür
- **Arka plan sayfaları:** beyaz, çizgili, kareli, milimetrik,
  noktalı, yeşil tahta, siyah tahta
- **Sık kullanılanlar:** en çok kullandığınız 20 komut ana menüde

</td>
</tr>
</table>

#### 🧰 Araçlar menüsü (2.1 · Pardus / Linux)

| | |
|---|---|
| **Kalem çeşitleri** | Normal, kesikli, noktalı ve oklu kalem · el yazısını yumuşatma |
| **Şekil tanıma** | Elle çizilen çizgi, daire, üçgen ve dikdörtgen düzgün şekle dönüşür |
| **Seç ve düzenle** | Kementle seç; taşı, büyüt, kopyala, sil |
| **Metin** | Ekrana yazı ekleme ve düzenleme |
| **Lazer ve kaybolan kalem** | Anlatırken iz bırakmadan gösterme |
| **Dolgu** | Kapalı şekillerin içini boyama |
| **Ölçme araçları** | Cetvel, gönye, iletki, pergel — kalem cetvelin kenarına yapışır |
| **Sınıf araçları** | Spot ışığı, büyüteç, sayaç / kronometre, kura çekme |
| **Sayfalar** | Çok sayfalı tahta; ekran görüntüsünü sayfa yapıp üzerine yazma |
| **Kaydet ve paylaş** | PNG, çok sayfalı PDF, `.fkalem` ders dosyası; QR kodla telefona gönderme |
| **Şablonlar** | Dört çizgili defter, müzik portesi, koordinat düzlemi, sayı doğrusu, izometrik kâğıt |
| **Renkler** | Son kullanılan renkler ve kendi paletiniz; renk körlüğüne uygun palet |

## Ekran Görüntüleri

<table>
<tr>
<td align="center" width="14%"><img src="docs/images/menu.png" height="250" alt="Ana menü"><br><sub><b>Ana menü</b></sub></td>
<td align="center" width="26%"><img src="docs/images/araclar.png" height="250" alt="Araçlar menüsü"><br><sub><b>Araçlar menüsü</b></sub></td>
<td align="center" width="26%"><img src="docs/images/ekran-jest.png" height="250" alt="İki parmak jesti"><br><sub><b>İki parmak jesti</b></sub></td>
<td align="center" width="34%"><img src="docs/images/hakkinda-pardus.png" height="250" alt="Ayarlar ve Hakkında (Pardus)"><br><sub><b>Ayarlar</b></sub></td>
</tr>
</table>

## Platform Desteği

| Sistem | Masaüstü | Durum | Paket |
|---|---|:---:|---|
| **Pardus ETAP 23** | ETAP | ✅ Birincil hedef | `.deb` |
| Pardus 25 / 23 | GNOME (Wayland / X11), XFCE | ✅ Destekleniyor | `.deb` |
| Pardus 21 / ETAP 19–21 | XFCE | 🧪 Deneysel | `.deb` |
| Debian 11+ / Ubuntu 22.04+ | GNOME, XFCE, Cinnamon, MATE | 🧪 Deneysel | `.deb` |
| Windows 10 / 11 | — | ✅ Destekleniyor | `.exe` kurulum · `.zip` |
| Windows 7 / 8.1 | — | ⚠️ .NET Framework 4.8 gerekir | `.exe` kurulum · `.zip` |

> **Wayland:** GNOME Wayland oturumlarında program otomatik olarak XWayland üzerinden
> açılır; ek ayar gerekmez. <br>
> **Saydamlık kapalı masaüstleri:** Bileşikleştirici (compositor) olmayan eski
> tahtalarda program ekran görüntüsü kipine geçerek çalışmaya devam eder.

## Kurulum

En güncel dosyalar **[Releases](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases/latest)** sayfasındadır.

### Pardus / ETAP

**Grafik arayüzle:** `fatih-kalem_2.1.1_all.deb` dosyasına çift tıklayın ve **Yükle**'ye basın.

**Uçbirimle:**

```bash
sudo apt install ./fatih-kalem_2.1.1_all.deb
```

**İnternet bağlantılı tahtada tek komutla:**

```bash
wget https://github.com/YahyaSvm/Fatih-Kalem-Source/releases/download/v2.1.1/fatih-kalem_2.1.1_all.deb \
  && sudo apt install ./fatih-kalem_2.1.1_all.deb
```

Kurulumdan sonra program **Uygulamalar → Eğitim → Fatih Kalem** altında yer alır.

<details>
<summary><b>Tüm kullanıcılarda açılışta otomatik başlatma (okul BT yöneticileri için)</b></summary>

<br>

```bash
sudo cp /usr/share/applications/fatih-kalem.desktop /etc/xdg/autostart/
```

Tek bir kullanıcı için **Ayarlar → Otomatik Başlatma** bölümü yeterlidir. Eski (Faz-1)
tahtalarda daha hızlı açılış için aynı bölümdeki **ön yükleme** seçeneği önerilir.

</details>

<details>
<summary><b>Kaldırma</b></summary>

<br>

```bash
sudo apt remove fatih-kalem
```

Kullanıcı ayarları `~/.config/fatih-kalem/` klasöründe kalır.

</details>

Wayland, saydamlık, dokunmatik kalibrasyon ve sorun giderme ayrıntıları için
**[Pardus Rehberi](linux/README.md)**'ne bakın.

### Windows

**Kurulum sihirbazıyla (önerilen):** `FatihKalem-2.1.1-kurulum.exe` dosyasını çalıştırın.
Yönetici yetkisi gerekmez; isteğe bağlı masaüstü kısayolu ve açılışta başlatma seçenekleri vardır.

**Taşınabilir:** `FatihKalem-2.1.1-windows.zip` dosyasını bir klasöre açıp `Fatih Kalem.exe`
dosyasını çalıştırın.

.NET Framework 4.8 gerekir (Windows 10 ve 11'de hazır gelir). Yeni sürüm çıktığında
program sizi bilgilendirir.

## Kullanım

| Menü öğesi | Ne yapar? |
|---|---|
| **Fatih Kalem başlığı** | Basılı tutup sürükleyin; fırlatırsanız kayarak ilerler. |
| **Kalem / El** | Kalem moduna geçer. El moduna dönüldüğünde çizimler temizlenir; kalemi tekrar açıp **Geri Al** ile geri getirebilirsiniz. |
| **Kalem** | Kalem tipi, renk, kartela, kalemsiz mod ve kalınlık. |
| **Silgi** | Geri al, yinele, perde ve dört silgi boyutu. |
| **Şekiller** | Altı şekil, görsel kütüphanesi ve arka plan sayfaları. |
| **Araçlar** | Ölçme araçları, seçme, metin, lazer, sayfalar, kaydetme ve paylaşma (Pardus / Linux). |
| **⚙ Ayarlar** | Başlangıç kalemi ve konumu, sık kullanılanlar, hızlı erişim, otomatik başlatma, görünüm (dil, menü boyutu, sol el), gelişmiş. |
| **✕ Kapat** | Programı kapatır (isteğe bağlı onayla). |

**İki parmak jestleri** — bir parmak tahtadayken ikinci parmağı koyup çekin:

| Yön | Sonuç |
|:---:|---|
| ⬅️ Sol | Kırmızı kalem |
| ➡️ Sağ | Mavi kalem |
| ⬆️ Yukarı | Siyah kalem (fosforluda sarı) |
| ⬇️ Aşağı | Silgi (tekrar çekince kaleme döner) |
| Kısa çekiş | Menü parmağınızın yanına gelir |

Farede aynı jestler **sağ tuşa basılı tutarak** yapılır. Alt menüdeki bir öğeye
**uzun basmak** (farede sağ tık) onu ana menüye sık kullanılan olarak ekler.

## Sık Sorulan Sorular

<details>
<summary><b>Kurulum için internet gerekir mi?</b></summary>
<br>
Hayır. Pardus'ta gereken tüm kitaplıklar hazır gelir; <code>.deb</code> dosyasını USB bellekle taşımanız yeterlidir.
</details>

<details>
<summary><b>Yönetici parolam yok, kurabilir miyim?</b></summary>
<br>
Paket kurulumu yönetici (sudo) yetkisi ister. Bu parola genellikle okulun bilişim rehber
öğretmeninde ya da ilçe teknik ekibindedir.
</details>

<details>
<summary><b>Kalem modunda ekran kararıyor ya da donmuş görünüyor.</b></summary>
<br>
Masaüstünde saydamlık (bileşikleştirici) kapalı olabilir. Program bu durumda ekran
görüntüsü kipinde çalışır. XFCE'de <i>Pencere Yöneticisi İnce Ayarları → Birleştirici</i>
bölümünden saydamlığı açabilirsiniz. Durumu <b>Ayarlar → Hakkında</b> bölümünde görebilirsiniz.
</details>

<details>
<summary><b>İki parmak jesti çalışmıyor.</b></summary>
<br>
<b>Ayarlar → Hızlı Erişim</b> bölümünde jest seçeneğinin açık olduğundan emin olun.
Bazı eski kızılötesi tahtalar çoklu dokunmayı desteklemez; bu tahtalarda menü ya da sağ tık kullanılabilir.
</details>

<details>
<summary><b>Windows'ta program kapanırsa ne yapmalıyım?</b></summary>
<br>
Hata ayrıntıları <code>%LOCALAPPDATA%\Fatih Kalem\hata.log</code> dosyasına yazılır.
Bu dosyayı <a href="https://github.com/YahyaSvm/Fatih-Kalem-Source/issues/new">yeni bir hata kaydına</a> ekleyin.
</details>

## Kaynaktan Derleme

**Pardus / Linux**

```bash
cd linux
python3 -m fatihkalem      # kurmadan çalıştır
make test                  # birim testleri (python3-pytest)
make deb                   # ../dist/fatih-kalem_<sürüm>_all.deb
```

**Windows** (.NET SDK 8 veya üzeri)

```powershell
dotnet build "Fatih Kalem.csproj" -c Release
```

**Sürüm yayınlama** — `v*` biçiminde bir etiket gönderildiğinde GitHub Actions
Windows paketini derler, Linux testlerini çalıştırır, `.deb` paketini üretir ve
release'i otomatik oluşturur:

```bash
git tag v2.1.1 && git push origin v2.1.1
```

## Proje Yapısı

```
Fatih-Kalem-Source/
├── canvas/                 Windows sürümü (WPF, C#)
├── Properties/             derleme bilgileri
├── resources/              menü görselleri ve simgeler
├── linux/                  Pardus / Linux sürümü
│   ├── fatihkalem/         uygulama (Python 3 + GTK3)
│   │   ├── window.py       tam ekran kalem penceresi, dokunma ve jestler
│   │   ├── toolbar.py      menü yerleşimi ve dokunma bölgeleri
│   │   ├── ink.py          mürekkep, kalem uçları, noktasal silgi
│   │   ├── scene.py        sahne, sayfalar ve geri al / yinele
│   │   ├── features.py     Araçlar menüsünün komutları
│   │   ├── tools.py        cetvel, gönye, iletki, pergel, spot, büyüteç, sayaç
│   │   ├── recognize.py    şekil tanıma, yumuşatma, kesikli / oklu kalem
│   │   ├── lesson.py       ders dosyası, PNG / PDF, otomatik kayıt
│   │   ├── qr.py           QR kod üretici
│   │   └── ...
│   ├── debian/             Debian / Pardus paket tanımı
│   ├── packaging/          .deb üretim betiği
│   └── tests/              birim testleri
├── installer/              Windows kurulum sihirbazı (Inno Setup)
├── docs/images/            ekran görüntüleri
└── .github/workflows/      otomatik derleme ve sürüm yayınlama
```

## Yol Haritası

### ✅ 2.0
- [x] Pardus / ETAP sürümü ve `.deb` paketi
- [x] İki parmak jestleri, kalem basıncı, silgi ucu
- [x] HiDPI / 4K ekran desteği ve yeni arka plan sayfaları
- [x] Windows geri al / yinele ve otomatik başlatma düzeltmeleri
- [x] Otomatik derleme ve sürüm yayınlama

### ✅ 2.1
- [x] Metin kutusu, seçme aracı, şekil tanıma, dolgu
- [x] Lazer işaretçi, kaybolan kalem, kesikli / noktalı / oklu kalem, el yazısı yumuşatma
- [x] Son kullanılan renkler, kendi renk paleti, onaylı tümünü temizle
- [x] Cetvel, gönye, iletki, pergel
- [x] Spot ışığı, büyüteç, sayaç / kronometre, kura
- [x] Dört çizgili defter, müzik portesi, koordinat düzlemi, sayı doğrusu, izometrik kâğıt
- [x] Ekran görüntüsünü sayfa yapıp üzerine yazma
- [x] Çok sayfalı tahta; PNG, PDF ve `.fkalem` ders dosyası; USB'ye kaydetme
- [x] QR kodla öğrencilerle paylaşma
- [x] Otomatik kayıt (her dakika, el moduna dönerken ve tahta kapanırken)
- [x] Okul yöneticileri için ortak ayar dosyası (`/etc/fatih-kalem/ayarlar.json`)
- [x] Çoklu ekran seçimi
- [x] Windows kurulum sihirbazı, güncelleme bildirimi, ekran başına yüksek DPI
- [x] Menü boyutu, sol el yerleşimi, renk körlüğü paleti, İngilizce arayüz
- [x] Ayarları dışa / içe aktarma, tek tıkla hata raporu
- [x] Sanal ekranda otomatik arayüz testleri, uzun çizgilerde hızlandırma

### 🔜 Sırada
- [ ] Pardus Yazılım Merkezi'nde yer alma ve Pardus deposuna girme (`apt install fatih-kalem`)
- [ ] ETAP ekran klavyesi ve tahta araçlarıyla uyumun farklı tahtalarda doğrulanması
- [ ] Wayland'de XWayland olmadan doğrudan çalışma
- [ ] Araçlar menüsünün Windows sürümüne de gelmesi (tek kod tabanı)
- [ ] Hazır harita şablonları
- [ ] Daha fazla dil

Öneriniz mi var? [Bir öneri kaydı açın](https://github.com/YahyaSvm/Fatih-Kalem-Source/issues/new).

## Katkıda Bulunma

Hata bildirimleri, öneriler ve kod katkıları memnuniyetle karşılanır.

1. Depoyu çatallayın (fork) ve yeni bir dal açın.
2. Değişikliğinizi yapın; Linux tarafında `make test` ile testlerin geçtiğinden emin olun.
3. Ne yaptığınızı kısaca açıklayan bir çekme isteği (pull request) gönderin.

Hata bildirirken lütfen sistem bilgisini (Pardus sürümü, masaüstü ortamı) ve
**Ayarlar → Hakkında** bölümündeki oturum bilgisini ekleyin.

## Haklar ve Teşekkür

**Fatih Kalem 2** — [Yahya Eren Sevim](https://github.com/YahyaSvm) (YhySvm)

İlk Fatih Kalem (1.0), Fizik Öğretmeni **Hasan Yunus ATEŞ** tarafından Milli Eğitim
Bakanlığı YEĞİTEK için yazılmıştır. Menü görselleri o sürümden gelmektedir ve hakları
Milli Eğitim Bakanlığına aittir. Ayrıntılar için [LICENSE](LICENSE) dosyasına bakın.

<div align="center">
<br>
<sub>Fatih Projesi etkileşimli tahtalarında kullanılmak üzere geliştirilmiştir.</sub>
</div>
