#!/bin/sh
# debhelper gerektirmeden .deb üretir (Pardus / Debian / Ubuntu).
# Kullanım: sh packaging/build-deb.sh   (linux/ klasöründe)
set -eu

cd "$(dirname "$0")/.."
VERSION=$(sed -n 's/^VERSION = "\(.*\)"/\1/p' fatihkalem/__init__.py)
PKG=fatih-kalem
STAGE=build/deb
OUT=../dist

rm -rf "$STAGE"
make -s install DESTDIR="$STAGE" PREFIX=/usr

mkdir -p "$STAGE/DEBIAN" "$STAGE/usr/share/doc/$PKG"
cp debian/copyright "$STAGE/usr/share/doc/$PKG/copyright"
gzip -9n -c debian/changelog > "$STAGE/usr/share/doc/$PKG/changelog.gz"
cp README.md "$STAGE/usr/share/doc/$PKG/BENIOKU.md"

SIZE=$(du -sk "$STAGE/usr" | cut -f1)
sed -e "s/@VERSION@/$VERSION/" -e "s/@SIZE@/$SIZE/" packaging/control.in > "$STAGE/DEBIAN/control"
install -m 755 debian/postinst "$STAGE/DEBIAN/postinst"
install -m 755 debian/prerm "$STAGE/DEBIAN/prerm"

# Dosya izinleri: dizinler 755, dosyalar 644 (başlatıcı 755)
find "$STAGE/usr" -type d -exec chmod 755 {} +
find "$STAGE/usr" -type f -exec chmod 644 {} +
chmod 755 "$STAGE/usr/bin/fatih-kalem"

( cd "$STAGE" && find usr -type f -exec md5sum {} + ) > "$STAGE/DEBIAN/md5sums"

mkdir -p "$OUT"
dpkg-deb --root-owner-group -Zxz --build "$STAGE" "$OUT/${PKG}_${VERSION}_all.deb"
echo "Paket: $OUT/${PKG}_${VERSION}_all.deb"
