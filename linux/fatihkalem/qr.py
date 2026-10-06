"""Bağımlılıksız QR kod üretici (ISO/IEC 18004, model 2).

Bayt kipi, M hata düzeltme seviyesi, sürüm 1-10 (en çok 213 bayt). Pardus'ta
ek paket gerektirmeden ders paylaşım bağlantısını QR olarak göstermek için.
"""

# Sürüm -> (toplam kod sözcüğü, blok başına ECC, [(blok sayısı, blok başına veri)])
_M_BLOCKS = {
    1: (10, [(1, 16)]),
    2: (16, [(1, 28)]),
    3: (26, [(1, 44)]),
    4: (18, [(2, 32)]),
    5: (24, [(2, 43)]),
    6: (16, [(4, 27)]),
    7: (18, [(4, 31)]),
    8: (22, [(2, 38), (2, 39)]),
    9: (22, [(3, 36), (2, 37)]),
    10: (26, [(4, 43), (1, 44)]),
}
_ALIGN = {1: [], 2: [6, 18], 3: [6, 22], 4: [6, 26], 5: [6, 30], 6: [6, 34],
          7: [6, 22, 38], 8: [6, 24, 42], 9: [6, 26, 46], 10: [6, 28, 50]}
_REMAINDER = {1: 0, 2: 7, 3: 7, 4: 7, 5: 7, 6: 7, 7: 0, 8: 0, 9: 0, 10: 0}

# ------------------------------------------------------------ Reed-Solomon --
_EXP = [0] * 512
_LOG = [0] * 256
_x = 1
for _i in range(255):
    _EXP[_i] = _x
    _LOG[_x] = _i
    _x <<= 1
    if _x & 0x100:
        _x ^= 0x11D
for _i in range(255, 512):
    _EXP[_i] = _EXP[_i - 255]


def _gmul(a, b):
    if a == 0 or b == 0:
        return 0
    return _EXP[_LOG[a] + _LOG[b]]


def _rs_generator(degree):
    g = [1]
    for i in range(degree):
        ng = [0] * (len(g) + 1)
        for j, c in enumerate(g):
            ng[j] ^= c
            ng[j + 1] ^= _gmul(c, _EXP[i])
        g = ng
    return g


def _rs_remainder(data, degree):
    gen = _rs_generator(degree)
    res = list(data) + [0] * degree
    for i in range(len(data)):
        coef = res[i]
        if coef:
            for j in range(1, len(gen)):
                res[i + j] ^= _gmul(gen[j], coef)
    return res[len(data):]


# ------------------------------------------------------------------ veri ----
def _capacity(version):
    ecc, groups = _M_BLOCKS[version]
    return sum(n * d for n, d in groups)


def _choose_version(nbytes):
    for v in range(1, 11):
        count_bits = 8 if v < 10 else 16
        bits = 4 + count_bits + 8 * nbytes
        if (bits + 7) // 8 <= _capacity(v):
            return v
    raise ValueError("QR için metin çok uzun (en çok ~210 bayt)")


def _codewords(data, version):
    count_bits = 8 if version < 10 else 16
    bits = []

    def put(value, n):
        for i in range(n - 1, -1, -1):
            bits.append((value >> i) & 1)

    put(0b0100, 4)
    put(len(data), count_bits)
    for b in data:
        put(b, 8)
    cap_bits = _capacity(version) * 8
    put(0, min(4, cap_bits - len(bits)))
    while len(bits) % 8:
        bits.append(0)
    cws = [int("".join(map(str, bits[i:i + 8])), 2) for i in range(0, len(bits), 8)]
    pad = 0xEC
    while len(cws) < _capacity(version):
        cws.append(pad)
        pad = 0x11 if pad == 0xEC else 0xEC

    ecc, groups = _M_BLOCKS[version]
    blocks, eccs, pos = [], [], 0
    for n, size in groups:
        for _ in range(n):
            blk = cws[pos:pos + size]
            pos += size
            blocks.append(blk)
            eccs.append(_rs_remainder(blk, ecc))
    out = []
    for i in range(max(len(b) for b in blocks)):
        for b in blocks:
            if i < len(b):
                out.append(b[i])
    for i in range(ecc):
        for e in eccs:
            out.append(e[i])
    return out


# -------------------------------------------------------------- matris -----
class _Matrix:
    def __init__(self, version):
        self.v = version
        self.n = 17 + 4 * version
        self.m = [[None] * self.n for _ in range(self.n)]
        self.reserved = [[False] * self.n for _ in range(self.n)]

    def set(self, r, c, val, reserve=True):
        self.m[r][c] = bool(val)
        if reserve:
            self.reserved[r][c] = True

    def finder(self, r, c):
        for dr in range(-1, 8):
            for dc in range(-1, 8):
                rr, cc = r + dr, c + dc
                if 0 <= rr < self.n and 0 <= cc < self.n:
                    inside = 0 <= dr <= 6 and 0 <= dc <= 6
                    on = inside and (dr in (0, 6) or dc in (0, 6)
                                     or (2 <= dr <= 4 and 2 <= dc <= 4))
                    self.set(rr, cc, on)

    def function_patterns(self):
        n = self.n
        self.finder(0, 0)
        self.finder(0, n - 7)
        self.finder(n - 7, 0)
        for i in range(8, n - 8):
            self.set(6, i, i % 2 == 0)
            self.set(i, 6, i % 2 == 0)
        pos = _ALIGN[self.v]
        last = len(pos) - 1
        for i, r in enumerate(pos):
            for j, c in enumerate(pos):
                # Yalnızca bulucu desenlerin olduğu üç köşe atlanır; zamanlama
                # çizgisine denk gelenler çizilir.
                if (i, j) in ((0, 0), (0, last), (last, 0)):
                    continue
                for dr in range(-2, 3):
                    for dc in range(-2, 3):
                        self.set(r + dr, c + dc, max(abs(dr), abs(dc)) != 1)
        # biçim bilgisi alanlarını ayır
        for i in range(9):
            if not self.reserved[8][i]:
                self.set(8, i, False)
            if not self.reserved[i][8]:
                self.set(i, 8, False)
        for i in range(8):
            self.set(8, n - 1 - i, False)
            self.set(n - 1 - i, 8, False)
        self.set(n - 8, 8, True)          # karanlık modül
        if self.v >= 7:
            bits = _version_bits(self.v)
            for i in range(18):
                b = (bits >> i) & 1
                a, c = n - 11 + i % 3, i // 3
                self.set(a, c, b)
                self.set(c, a, b)

    def place_data(self, cws):
        bits = []
        for cw in cws:
            for i in range(7, -1, -1):
                bits.append((cw >> i) & 1)
        bits += [0] * _REMAINDER[self.v]
        n = self.n
        i = 0
        upward = True
        col = n - 1
        while col > 0:
            if col == 6:
                col -= 1
            rows = range(n - 1, -1, -1) if upward else range(n)
            for r in rows:
                for c in (col, col - 1):
                    if not self.reserved[r][c]:
                        self.m[r][c] = bool(bits[i]) if i < len(bits) else False
                        i += 1
            upward = not upward
            col -= 2


def _version_bits(version):
    rem = version
    for _ in range(12):
        rem = (rem << 1) ^ ((rem >> 11) * 0x1F25)
    return (version << 12) | rem


def _format_bits(mask):
    data = (0b00 << 3) | mask          # M seviyesi = 00
    rem = data
    for _ in range(10):
        rem = (rem << 1) ^ ((rem >> 9) * 0x537)
    return ((data << 10) | rem) ^ 0x5412


_MASKS = [
    lambda r, c: (r + c) % 2 == 0,
    lambda r, c: r % 2 == 0,
    lambda r, c: c % 3 == 0,
    lambda r, c: (r + c) % 3 == 0,
    lambda r, c: (r // 2 + c // 3) % 2 == 0,
    lambda r, c: (r * c) % 2 + (r * c) % 3 == 0,
    lambda r, c: ((r * c) % 2 + (r * c) % 3) % 2 == 0,
    lambda r, c: ((r + c) % 2 + (r * c) % 3) % 2 == 0,
]


def _apply(base, mask):
    n = base.n
    m = [row[:] for row in base.m]
    f = _MASKS[mask]
    for r in range(n):
        for c in range(n):
            if not base.reserved[r][c] and f(r, c):
                m[r][c] = not m[r][c]
    bits = _format_bits(mask)
    for i in range(15):
        b = bool((bits >> i) & 1)
        # sol üst
        if i < 6:
            m[i][8] = b
        elif i < 8:
            m[i + 1][8] = b
        else:
            m[8][14 - i if i >= 9 else 7] = b
        # sağ üst / sol alt
        if i < 8:
            m[8][n - 1 - i] = b
        else:
            m[n - 15 + i][8] = b
    m[n - 8][8] = True
    return m


def _penalty(m):
    n = len(m)
    score = 0
    for lines in (m, [list(col) for col in zip(*m)]):
        for line in lines:
            run, prev = 0, None
            for v in line:
                if v == prev:
                    run += 1
                else:
                    if run >= 5:
                        score += run - 2
                    run, prev = 1, v
            if run >= 5:
                score += run - 2
            s = "".join("1" if v else "0" for v in line)
            for pat in ("10111010000", "00001011101"):
                start = s.find(pat)
                while start != -1:
                    score += 40
                    start = s.find(pat, start + 1)
    for r in range(n - 1):
        for c in range(n - 1):
            v = m[r][c]
            if v == m[r][c + 1] == m[r + 1][c] == m[r + 1][c + 1]:
                score += 3
    dark = sum(v for row in m for v in row)
    k = abs(dark * 20 - n * n * 10) // (n * n)
    score += k * 10
    return score


def encode(text, mask=None):
    """Metni QR matrisine çevirir: list[list[bool]] (True = siyah)."""
    data = text.encode("utf-8")
    version = _choose_version(len(data))
    base = _Matrix(version)
    base.function_patterns()
    base.place_data(_codewords(data, version))
    best = None
    for mask in (range(8) if mask is None else [mask]):
        m = _apply(base, mask)
        p = _penalty(m)
        if best is None or p < best[0]:
            best = (p, m)
    return best[1]


def draw(cr, matrix, x, y, size):
    """QR'ı (sessiz bölge dahil) kare alana çizer."""
    n = len(matrix)
    quiet = 4
    cell = size / float(n + 2 * quiet)
    cr.save()
    cr.set_source_rgb(1, 1, 1)
    cr.rectangle(x, y, size, size)
    cr.fill()
    cr.set_source_rgb(0, 0, 0)
    for r in range(n):
        for c in range(n):
            if matrix[r][c]:
                cr.rectangle(x + (c + quiet) * cell, y + (r + quiet) * cell,
                             cell + 0.3, cell + 0.3)
    cr.fill()
    cr.restore()
