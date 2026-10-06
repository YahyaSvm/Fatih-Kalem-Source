"""GitHub'daki son sürümü denetler (günde en fazla bir kez, arka planda)."""

import json
import threading
import time
import urllib.request

from . import GITHUB_URL, VERSION

API = GITHUB_URL.replace("https://github.com/", "https://api.github.com/repos/") + "/releases/latest"


def parse_version(text):
    text = text.strip().lstrip("vV")
    parts = []
    for p in text.split("."):
        num = "".join(ch for ch in p if ch.isdigit())
        parts.append(int(num) if num else 0)
    while len(parts) < 3:
        parts.append(0)
    return tuple(parts[:3])


def is_newer(remote, local=VERSION):
    return parse_version(remote) > parse_version(local)


def fetch_latest(timeout=6):
    req = urllib.request.Request(API, headers={"Accept": "application/vnd.github+json",
                                               "User-Agent": "FatihKalem/" + VERSION})
    with urllib.request.urlopen(req, timeout=timeout) as r:
        data = json.loads(r.read().decode("utf-8"))
    return data.get("tag_name", ""), data.get("html_url", GITHUB_URL + "/releases")


def check_async(settings, on_new):
    """Yeni sürüm varsa on_new(sürüm, adres) arka iş parçacığından çağrılır."""
    if not settings["CheckUpdates"]:
        return
    now = time.time()
    if now - settings["LastUpdateCheck"] < 20 * 3600:
        return

    def work():
        try:
            tag, url = fetch_latest()
        except Exception:
            return
        settings["LastUpdateCheck"] = now
        settings.save()
        if tag and is_newer(tag) and tag != settings["DismissedVersion"]:
            on_new(tag, url)

    threading.Thread(target=work, daemon=True).start()
