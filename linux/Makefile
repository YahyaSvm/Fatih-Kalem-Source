# Fatih Kalem (Pardus / Linux) kurulum
#   make install DESTDIR=... PREFIX=/usr
#   make deb          -> ../dist/fatih-kalem_<sürüm>_all.deb
#   make test

PREFIX ?= /usr
DESTDIR ?=
SHARE = $(DESTDIR)$(PREFIX)/share/fatih-kalem
VERSION := $(shell sed -n 's/^VERSION = "\(.*\)"/\1/p' fatihkalem/__init__.py)

.PHONY: all install uninstall deb test clean

all:
	@echo "Kullanım: make install | make deb | make test"

install:
	install -d $(SHARE)/fatihkalem $(SHARE)/images
	install -m 644 fatihkalem/*.py $(SHARE)/fatihkalem/
	install -m 644 data/images/*.png $(SHARE)/images/
	install -D -m 755 bin/fatih-kalem $(DESTDIR)$(PREFIX)/bin/fatih-kalem
	install -D -m 644 data/fatih-kalem.desktop $(DESTDIR)$(PREFIX)/share/applications/fatih-kalem.desktop
	install -D -m 644 data/tr.yhysvm.FatihKalem.metainfo.xml $(DESTDIR)$(PREFIX)/share/metainfo/tr.yhysvm.FatihKalem.metainfo.xml
	install -D -m 644 data/fatih-kalem-mime.xml $(DESTDIR)$(PREFIX)/share/mime/packages/fatih-kalem.xml
	install -D -m 644 data/ayarlar-ornek.json $(DESTDIR)$(PREFIX)/share/doc/fatih-kalem/ayarlar-ornek.json
	for s in 16 24 32 48 256; do \
		install -D -m 644 data/icons/$$s.png \
			$(DESTDIR)$(PREFIX)/share/icons/hicolor/$${s}x$${s}/apps/fatih-kalem.png; \
	done
	install -d $(SHARE)/gorseller

uninstall:
	rm -rf $(DESTDIR)$(PREFIX)/share/fatih-kalem
	rm -f $(DESTDIR)$(PREFIX)/bin/fatih-kalem
	rm -f $(DESTDIR)$(PREFIX)/share/applications/fatih-kalem.desktop
	rm -f $(DESTDIR)$(PREFIX)/share/metainfo/tr.yhysvm.FatihKalem.metainfo.xml
	rm -f $(DESTDIR)$(PREFIX)/share/mime/packages/fatih-kalem.xml
	for s in 16 24 32 48 256; do \
		rm -f $(DESTDIR)$(PREFIX)/share/icons/hicolor/$${s}x$${s}/apps/fatih-kalem.png; \
	done

deb:
	sh packaging/build-deb.sh

test:
	python3 -m pytest -q tests

clean:
	rm -rf build ../dist/*.deb
	find . -name __pycache__ -type d -exec rm -rf {} +
