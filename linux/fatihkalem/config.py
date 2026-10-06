"""Kullanıcı ayarları (orijinaldeki My.Settings karşılığı).

Ayarlar ~/.config/fatih-kalem/ayarlar.json dosyasında tutulur. Dosya her
kayıtta önce geçici dosyaya yazılıp sonra taşınır; tahta aniden kapatılsa
bile ayar dosyası bozulmaz.
"""

import json
import os
import tempfile

DEFAULTS = {
    "PenStyle": 1,                 # 0 keçeli, 1 dolma, 2 fosforlu
    "ColorNo": 2,
    "InkSize": 3,
    "InkColor": "#00009D",         # ColorNo 7 iken kullanılan özel renk
    "isStartupPositionDefault": True,
    "Left": 0,
    "Top": 0,
    "AutoStart": False,
    "PreLoad": False,
    "isSideArrowsEnabled": True,
    "isShortGestureEnabled": True,
    "isCloseConfirmationEnabled": False,
    "isGestureEnabled": True,
    "Favourites": [],              # en fazla 20 etiket ("MarkerRed", "Undo", ...)
    "LastBrowsedPath": "",
}

MAX_FAVOURITES = 20


def config_dir():
    base = os.environ.get("XDG_CONFIG_HOME") or os.path.expanduser("~/.config")
    return os.path.join(base, "fatih-kalem")


def data_home():
    base = os.environ.get("XDG_DATA_HOME") or os.path.expanduser("~/.local/share")
    return os.path.join(base, "fatih-kalem")


class Settings:
    def __init__(self, path=None):
        self.path = path or os.path.join(config_dir(), "ayarlar.json")
        self.values = dict(DEFAULTS)
        self.load()

    def load(self):
        try:
            with open(self.path, encoding="utf-8") as f:
                data = json.load(f)
        except (OSError, ValueError):
            return
        for key, default in DEFAULTS.items():
            if key in data and isinstance(data[key], type(default)):
                self.values[key] = data[key]
        favs = [f for f in self.values["Favourites"] if isinstance(f, str)]
        self.values["Favourites"] = favs[:MAX_FAVOURITES]
        for key, lo, hi in (("PenStyle", 0, 2), ("ColorNo", 1, 7), ("InkSize", 1, 6)):
            if not lo <= self.values[key] <= hi:
                self.values[key] = DEFAULTS[key]

    def save(self):
        os.makedirs(os.path.dirname(self.path), exist_ok=True)
        fd, tmp = tempfile.mkstemp(prefix=".ayarlar-", dir=os.path.dirname(self.path))
        try:
            with os.fdopen(fd, "w", encoding="utf-8") as f:
                json.dump(self.values, f, ensure_ascii=False, indent=2)
            os.replace(tmp, self.path)
        except OSError:
            try:
                os.unlink(tmp)
            except OSError:
                pass

    def __getitem__(self, key):
        return self.values[key]

    def __setitem__(self, key, value):
        self.values[key] = value


def hex_to_rgb(text):
    text = text.lstrip("#")
    if len(text) == 8:       # #AARRGGBB (orijinal biçim)
        text = text[2:]
    try:
        return tuple(int(text[i:i + 2], 16) / 255.0 for i in (0, 2, 4))
    except ValueError:
        return (0.0, 0.0, 157 / 255.0)


def rgb_to_hex(rgb):
    return "#%02X%02X%02X" % tuple(int(round(c * 255)) for c in rgb)
