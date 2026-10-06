"""Dersi yerel ağda paylaşma.

Tek bir dosyayı (ders PDF'i) küçük bir HTTP sunucusuyla yayınlar; aynı ağdaki
öğrenciler QR kodu okutarak indirir. İnternete bir şey gönderilmez.
"""

import http.server
import os
import socket
import threading
import urllib.parse


def local_ip():
    """Varsayılan ağ arayüzünün IP adresi (paket gönderilmez)."""
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    try:
        s.connect(("10.255.255.255", 1))
        ip = s.getsockname()[0]
    except OSError:
        ip = None
    finally:
        s.close()
    if ip and not ip.startswith("127."):
        return ip
    return None


class FileShare:
    def __init__(self, path):
        self.path = path
        self.name = os.path.basename(path)
        self.server = None
        self.thread = None

    def start(self, port=0):
        path, name = self.path, self.name

        class Handler(http.server.BaseHTTPRequestHandler):
            def do_GET(self):
                if urllib.parse.unquote(self.path.lstrip("/")) not in (name, ""):
                    self.send_error(404)
                    return
                try:
                    with open(path, "rb") as f:
                        data = f.read()
                except OSError:
                    self.send_error(404)
                    return
                self.send_response(200)
                ctype = "application/pdf" if name.lower().endswith(".pdf") else "application/octet-stream"
                self.send_header("Content-Type", ctype)
                self.send_header("Content-Length", str(len(data)))
                self.send_header("Content-Disposition",
                                 "attachment; filename*=UTF-8''" + urllib.parse.quote(name))
                self.end_headers()
                self.wfile.write(data)

            def log_message(self, *args):
                pass

        self.server = http.server.ThreadingHTTPServer(("0.0.0.0", port), Handler)
        self.thread = threading.Thread(target=self.server.serve_forever, daemon=True)
        self.thread.start()
        return self.url()

    def url(self):
        ip = local_ip()
        if not ip or self.server is None:
            return None
        return "http://%s:%d/%s" % (ip, self.server.server_address[1],
                                    urllib.parse.quote(self.name))

    def stop(self):
        if self.server is not None:
            self.server.shutdown()
            self.server.server_close()
            self.server = None
