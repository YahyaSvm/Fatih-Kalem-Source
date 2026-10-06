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
    # --- 2.1 ile gelenler
    "Language": "auto",            # auto | tr | en
    "MenuScale": 1.0,              # 1.0, 1.25, 1.5, 2.0
    "SubmenuSide": "auto",         # auto | left | right
    "ColorBlindPalette": False,
    "SmoothInk": False,
    "ShapeRecognition": False,
    "Monitor": -1,                 # -1 birincil, -2 tüm ekranlar, 0.. ekran no
    "CheckUpdates": True,
    "LastUpdateCheck": 0.0,
    "DismissedVersion": "",
    "Autosave": True,
    "RecentColors": [],            # "#RRGGBB" (en yeni başta)
    "CustomPalette": [],           # kullanıcının kaydettiği renkler
    "StudentList": "",
    "LotRange": "1-30",
}

MAX_FAVOURITES = 20
MAX_RECENT_COLORS = 5
MAX_CUSTOM_PALETTE = 4

# Okul / BT yöneticisinin tüm kullanıcılar için belirlediği varsayılanlar.
SYSTEM_DEFAULTS_PATH = "/etc/fatih-kalem/ayarlar.json"


def _same_type(value, default):
    if isinstance(default, bool):
        return isinstance(value, bool)
    if isinstance(default, float):
        return isinstance(value, (int, float)) and not isinstance(value, bool)
    if isinstance(default, int):
        return isinstance(value, int) and not isinstance(value, bool)
    return isinstance(value, type(default))


def _merge(target, data):
    for key, default in DEFAULTS.items():
        if key in data and _same_type(data[key], default):
            target[key] = float(data[key]) if isinstance(default, float) else data[key]


def config_dir():
    base = os.environ.get("XDG_CONFIG_HOME") or os.path.expanduser("~/.config")
    return os.path.join(base, "fatih-kalem")


def data_home():
    base = os.environ.get("XDG_DATA_HOME") or os.path.expanduser("~/.local/share")
    return os.path.join(base, "fatih-kalem")


class Settings:
    def __init__(self, path=None, system_path=SYSTEM_DEFAULTS_PATH):
        self.path = path or os.path.join(config_dir(), "ayarlar.json")
        self.system_path = system_path
        self.values = dict(DEFAULTS)
        self.load()

    @staticmethod
    def _read(path):
        try:
            with open(path, encoding="utf-8") as f:
                data = json.load(f)
            return data if isinstance(data, dict) else {}
        except (OSError, ValueError):
            return {}

    def load(self):
        self.values = dict(DEFAULTS)
        if self.system_path:
            _merge(self.values, self._read(self.system_path))
        _merge(self.values, self._read(self.path))
        self._validate()

    def _validate(self):
        v = self.values
        v["Favourites"] = [f for f in v["Favourites"] if isinstance(f, str)][:MAX_FAVOURITES]
        for key, lo, hi in (("PenStyle", 0, 2), ("ColorNo", 1, 7), ("InkSize", 1, 6)):
            if not lo <= v[key] <= hi:
                v[key] = DEFAULTS[key]
        if v["Language"] not in ("auto", "tr", "en"):
            v["Language"] = "auto"
        if v["SubmenuSide"] not in ("auto", "left", "right"):
            v["SubmenuSide"] = "auto"
        v["MenuScale"] = min(2.0, max(1.0, v["MenuScale"]))
        for key, limit in (("RecentColors", MAX_RECENT_COLORS),
                           ("CustomPalette", MAX_CUSTOM_PALETTE)):
            v[key] = [c for c in v[key] if isinstance(c, str) and c.startswith("#")][:limit]

    def export_to(self, path):
        with open(path, "w", encoding="utf-8") as f:
            json.dump(self.values, f, ensure_ascii=False, indent=2)

    def import_from(self, path):
        data = self._read(path)
        if not data:
            raise ValueError("geçersiz ayar dosyası")
        _merge(self.values, data)
        self._validate()
        self.save()

    def add_recent_color(self, rgb):
        h = rgb_to_hex(rgb)
        lst = [c for c in self.values["RecentColors"] if c != h]
        self.values["RecentColors"] = ([h] + lst)[:MAX_RECENT_COLORS]

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
