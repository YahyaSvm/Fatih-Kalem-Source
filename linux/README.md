# Fatih Kalem 2.0 — Pardus / Linux Rehberi

Fatih Projesi etkileşimli tahtaları için yazılmış **Fatih Kalem** programının
Pardus (özellikle **Pardus ETAP**) ve diğer GNU/Linux dağıtımları için GTK3
ile yeniden yazılmış sürümü.

Geliştiren: **Yahya Eren Sevim** ([@YahyaSvm](https://github.com/YahyaSvm)) · [GitHub](https://github.com/YahyaSvm/Fatih-Kalem-Source) · [Sürümler](https://github.com/YahyaSvm/Fatih-Kalem-Source/releases)

Menü görselleri, renkler, kalem kalınlıkları ve menüdeki dokunma bölgeleri
Windows sürümüyle birebir aynıdır. Menü aynı yerde açılır ve aynı
yere dokunulduğunda aynı işi yapar. Ana menü açıldığında en altta
**"By YhySvm"** yazısı görünür.

---

## 1. Kurulum

### 1.1 Pardus (ETAP, XFCE, GNOME) — önerilen yol

`fatih-kalem_2.0.0_all.deb` dosyasını tahtaya (USB bellekle ya da indirerek)
kopyalayın, sonra:

* **Grafik arayüzle:** dosyaya çift tıklayın. Paket yükleyici açılır,
  "Yükle"ye basın ve yönetici parolasını girin.
* **Uçbirimle:**

```bash
sudo apt install ./fatih-kalem_2.0.0_all.deb
```

`apt` bağımlılıkları kendisi kurar. Pardus'ta bu bağımlılıklar
(`python3-gi`, `python3-gi-cairo`, `python3-cairo`, `gir1.2-gtk-3.0`)
zaten kurulu gelir; internet bağlantısı gerekmez.

Kurulumdan sonra program **Uygulamalar → Eğitim → Fatih Kalem** altında
görünür. Uçbirimden `fatih-kalem` yazarak da açılabilir.

### 1.2 Paket olmadan (geliştirici / deneme)

```bash
cd linux
python3 -m fatihkalem
```

ya da sisteme kurmak için:

```bash
sudo make install PREFIX=/usr
```

### 1.3 Kaldırma

```bash
sudo apt remove fatih-kalem
```

Kullanıcı ayarları `~/.config/fatih-kalem/` klasöründe kalır. İsterseniz
elle silebilirsiniz.

---

## 2. Pardus'a özel notlar (önemli)

### 2.1 Desteklenen Pardus sürümleri

| Sürüm | Taban | Durum |
|---|---|---|
| Pardus 25 (GNOME / XFCE) | Debian 13 | Desteklenir |
| Pardus 23 (GNOME / XFCE) | Debian 12 | Desteklenir |
| **Pardus ETAP 23** | Pardus 23 | Desteklenir (asıl hedef) |
| Pardus 21 / ETAP 19-21 | Debian 11 | Desteklenir (Python 3.9) |

Program yalnızca Pardus'ta hazır gelen Python 3 + GTK3 kitaplıklarını
kullanır. Ayrı bir çalışma zamanı (.NET, Wine vb.) **gerekmez**.

### 2.2 Wayland ve X11

* **X11 oturumu** (Pardus XFCE, ETAP): doğrudan çalışır.
* **Wayland oturumu** (Pardus GNOME varsayılanı): Wayland, "her zaman üstte
  duran, tıklamaları alttaki pencereye geçiren saydam pencere" yapılmasına
  standart olarak izin vermez. Bu yüzden başlatıcı programı otomatik olarak
  **XWayland** üzerinden açar (`GDK_BACKEND=x11`). Ek ayar gerekmez.
  Wayland'i zorlamak için (önerilmez): `FATIHKALEM_NATIVE_WAYLAND=1 fatih-kalem`

### 2.3 Saydamlık (bileşikleştirici)

* XFCE'de "Pencere Yöneticisi İnce Ayarları → Birleştirici → Görüntü
  birleştirmeyi etkinleştir" açıksa kalem modunda masaüstü saydam görünür.
  GNOME'da bu her zaman açıktır.
* Birleştirici **kapalıysa** (bazı eski Faz-1 tahtalarında performans için
  kapatılır) program yine çalışır. Bu durumda kalem moduna geçerken ekranın
  görüntüsü alınır ve bu görüntünün üzerine yazılır. El moduna dönünce
  normal masaüstüne geri dönülür. Ayarlar → Hakkında bölümünde
  "Saydamlık: yok (ekran görüntüsü kipi)" yazar.

### 2.4 Dokunmatik tahta ve kalem

* Tek parmak, kalem ucu ya da fare ile çizilir.
* **İki parmak jesti:** Bir parmak tahtadayken ikinci parmağı koyup çekin.
  Sola çekince kırmızı, sağa çekince mavi, yukarı çekince siyah kalem
  (fosforluda sarı), aşağı çekince silgi seçilir. Kısa çekerseniz menü
  parmağınızın yanına gelir. Farede aynı işi **sağ tuşa basılı tutup
  çekerek** yapabilirsiniz.
* Basınca duyarlı kalemlerde çizgi kalınlığı basınca göre değişir.
* Kalemin **silgi ucu** (ters ucu) otomatik olarak silgi olur.
* Kızılötesi (IR) çerçeveli bazı eski tahtalar kendini fare gibi tanıtır.
  Bu tahtalarda tek parmakla çizim çalışır, iki parmak jesti çalışmaz. Bu
  durumda menüden ya da sağ tıkla seçim yapın.
* Dokunma kayıksa tahtanın dokunmatik kalibrasyon aracını kullanın
  (ör. `xinput_calibrator`). Bu, Fatih Kalem'e özel bir sorun değildir.

### 2.5 Ekran ve çözünürlük

* Program birincil ekranın çalışma alanını (panel hariç) kaplar.
* Başka bir ekranda açmak için: `fatih-kalem --ekran 1`
* HiDPI (4K tahta, ölçek 2) desteklenir. Menü görselleri orijinal boyutta
  çizilir.

### 2.6 Açılışta otomatik başlatma

Ayarlar → **Otomatik Başlatma**:

* "Sistem açılışında otomatik başlat": `~/.config/autostart/fatih-kalem.desktop`
  dosyası oluşturulur. Program oturum açılınca küçük menü olarak gelir.
* "Ön yükleme yap (Faz 1 tahtaları için önerilir)": Program açılışta
  dosyalarını belleğe alıp hemen kapanır. Daha sonra açtığınızda hızlı gelir.

Tüm öğretmen hesaplarında otomatik başlatmak için (yönetici):

```bash
sudo cp /usr/share/applications/fatih-kalem.desktop /etc/xdg/autostart/
```

---

## 3. Kullanım (orijinal programla aynı)

| Menü öğesi | İşlevi |
|---|---|
| Fatih Kalem başlığı | Basılı tutup sürükleyerek menüyü taşıyın. Fırlatırsanız kayarak gider. |
| Kalem / El düğmesi | Kalem moduna geçer. El moduna dönünce çizimler temizlenir ve tıklamalar masaüstüne geçer. |
| Kalem | Keçeli, dolma ve fosforlu kalem; 6 renk, renk kartelası (24 renk), "kalemsiz" ve 6 kalınlık. |
| Silgi | Geri al, yinele, perde ve 4 silgi boyutu. |
| Şekiller | Çizgi, kesikli çizgi, ok, dikdörtgen, elips, üçgen, görsel kütüphanesi ve arka plan sayfası. |
| Ayarlar (dişli) | Başlangıç kalemi/konumu, sık kullanılanlar, hızlı erişim, kapatma onayı, otomatik başlatma, hakkında. |
| Kapat (X) | Programı kapatır. Ayarlardan kapatma onayı açılabilir. |

* **Kalemsiz mod:** Çizimler ekranda kalır ama tıklamalar alttaki programa
  (ör. e-kitap, video) geçer.
* **Perde:** Ekranı beyaz bir perdeyle kapatır, yalnızca seçtiğiniz dikdörtgen
  açık kalır. Perde açıkken düğmeye tekrar basmak perdeyi kaldırır.
* **Üçgen:** Önce bir kenarı sürükleyerek çizin, sonra üçüncü köşeye dokunun.
* **Kütüphane görseli:** Görsel seçilince kırmızı çerçeveyle gelir. Ortasından
  sürükleyerek taşıyın, sağ alttaki tutamakla büyütün, sol üstteki ile
  döndürün. Dışarıya dokununca mürekkebin altına yerleşir. Yerleşmiş bir
  görseli tekrar düzenlemek için üzerine sağ tıklayın.
* **Arka plan sayfaları:** `~/.local/share/fatih-kalem/Arka Plan Sayfaları`
  klasöründe beyaz, çizgili, kareli, milimetrik, noktalı, yeşil tahta ve
  siyah tahta sayfaları ilk açılışta ekran çözünürlüğünde hazırlanır. Kendi
  resimlerinizi de bu klasöre koyabilirsiniz.
* **Görseller (kütüphane):** `~/.local/share/fatih-kalem/Görseller` ya da
  sistem geneli için `/usr/share/fatih-kalem/gorseller`.
* **Sık kullanılanlar:** Alt menülerdeki bir öğeye **uzun basın** (farede sağ
  tık). Öğe ana menüye eklenir (en fazla 20). Kaldırmak için ana menüdeki
  öğeye uzun basın. Sıralama Ayarlar → Sık Kullanılanlar'dan değiştirilir.
* **Yan oklar:** Ekranın sol/sağ alt köşesindeki oklar menüyü o kenara getirir.
  Oka iki kez hızlıca dokunursanız ok 5 saniye gizlenir.

---

## 4. Sorun giderme

| Belirti | Çözüm |
|---|---|
| Kalem modunda ekran siyah | Birleştirici çalışmıyor ve ekran görüntüsü alınamıyor olabilir. XFCE'de birleştiriciyi açın ya da programı yeniden başlatın. |
| Menü görünüyor ama dokunma alttaki uygulamaya gidiyor | Program el modundadır. Kalem düğmesine dokunun. |
| Wayland'de menü diğer pencerelerin altında kalıyor | `GDK_BACKEND=x11 fatih-kalem` ile açın (başlatıcı bunu zaten yapar; elle `python3 -m fatihkalem` ile açtıysanız gerekebilir). |
| İki parmak jesti çalışmıyor | Ayarlar → Hızlı Erişim → "Çift parmakla çekince..." açık olmalı. Tahta çoklu dokunma desteklemiyorsa sağ tıkla çekin. |
| Ayarları sıfırlamak | `rm -rf ~/.config/fatih-kalem` |

Hata ayıklama için programı uçbirimden açın:

```bash
fatih-kalem 2>&1 | tee ~/fatih-kalem-hata.txt
```

---

## 5. Geliştirici notları

```
linux/
├── bin/fatih-kalem            başlatıcı (XWayland seçimi, PYTHONPATH)
├── fatihkalem/
│   ├── app.py                 Gtk.Application, tek örnek, --preload
│   ├── window.py              tam ekran kaplama penceresi, girdiler, jestler
│   ├── toolbar.py             menü yerleşimi, orijinal dokunma bölgeleri
│   ├── ink.py                 çizgiler, uç şekilleri, noktasal silgi
│   ├── styles.py              renkler, kalınlıklar, şekil geometrisi
│   ├── scene.py               sahne + geri al/yinele
│   ├── settings_window.py     Ayarlar penceresi
│   ├── config.py              ~/.config/fatih-kalem/ayarlar.json
│   ├── system.py              otomatik başlatma, oturum/ETAP algılama
│   └── papers.py              arka plan sayfası üretimi
├── data/                      orijinal görseller, simgeler, .desktop, metainfo
├── debian/                    Debian/Pardus kaynak paketi (dpkg-buildpackage)
├── packaging/build-deb.sh     debhelper gerektirmeden .deb üretimi
└── tests/                     pytest birim testleri
```

* Paket üretmek: `make deb` (sonuç: `../dist/fatih-kalem_<sürüm>_all.deb`)
  ya da `dpkg-buildpackage -us -uc -b`
* Testler: `make test` (`python3-pytest` gerekir)
* Otomatik testlerde dosya seçiciyi atlamak için `FATIHKALEM_TEST_PICK=/yol/resim.png`

---

## 6. Haklar

İlk Fatih Kalem (1.0), Fizik Öğretmeni Hasan Yunus ATEŞ tarafından Milli
Eğitim Bakanlığı YEĞİTEK için yazılmıştır; menü görselleri o sürümden
gelmektedir ve hakları Milli Eğitim Bakanlığına aittir. Fatih Kalem 2.0,
Pardus çalıştıran Fatih Projesi etkileşimli tahtaları (ETAP) için
geliştirilmiştir.
