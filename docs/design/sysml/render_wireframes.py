#!/usr/bin/env python3
"""Каркасные мокапы экранов EvenUp из модели SysML v2.

Читает EvenUp-mockups.sysml, берёт части типа Screen из пакета Screens и рисует
каждую как страницу веб-приложения шириной мобильного экрана. Тип вложенной
части (Button, TextField, ListItem…) определяет, как элемент выглядит, атрибуты
(label, value, style…) — что на нём написано. PNG сохраняются в srs/images/.

Зависимость: Pillow (pip install pillow).
    python render_wireframes.py
"""
import re
import sys
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont

HERE = Path(__file__).resolve().parent
MODEL = HERE / "EvenUp-mockups.sysml"
OUT_DIR = HERE.parent / "images"
PACKAGE = "EvenUpMockups::Screens"

FILES = {
    "loginScreen": "ui-01-login",
    "groupsScreen": "ui-02-groups",
    "expensesScreen": "ui-03-expenses",
    "expenseFormScreen": "ui-04-expense-form",
    "balancesScreen": "ui-05-balances",
    "membersScreen": "ui-06-members",
    "historyScreen": "ui-07-history",
    "joinScreen": "ui-08-join",
}

S = 2              # масштаб: все размеры ниже в логических пикселях
SCREEN_W = 400     # ширина страницы
PAD = 16           # отступы содержимого страницы

TEXT = "#111827"
MUTED = "#6b7280"
DISABLED = "#9ca3af"
BORDER = "#d1d5db"
SOFT = "#f3f4f6"
PRIMARY = "#2563eb"
APP_BAR = "#1e3a8a"
POSITIVE = "#059669"
NEGATIVE = "#dc2626"
ERROR_BG = "#fef2f2"
ERROR_BORDER = "#fecaca"

FONT_FILES = {
    False: ["C:/Windows/Fonts/segoeui.ttf", "/mnt/c/Windows/Fonts/segoeui.ttf",
            "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf"],
    True: ["C:/Windows/Fonts/segoeuib.ttf", "/mnt/c/Windows/Fonts/segoeuib.ttf",
           "/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf"],
}


# ---------------------------------------------------------------- модель

class Part:
    def __init__(self, name, type_):
        self.name, self.type, self.attrs, self.children = name, type_, {}, []

    def get(self, key, default=""):
        return self.attrs.get(key, default)


TOKEN = re.compile(r'"[^"]*"|/\*.*?\*/|//[^\n]*|:>>|:>|::|[A-Za-z_]\w*|\S', re.S)


def literal(token):
    if token.startswith('"'):
        return token[1:-1]
    return {"true": True, "false": False}.get(token, token)


def parse(text):
    """Разбирает подмножество SysML v2, которое используется в модели мокапов:
    part def (с :>), вложенные part-usage и переопределения `:>> attr = value;`."""
    tokens = [t for t in TOKEN.findall(text) if not t.startswith(("//", "/*"))]
    defs, root, pos = {}, Part("", ""), 0

    def skip_block():
        nonlocal pos
        depth = 0
        while pos < len(tokens):
            depth += {"{": 1, "}": -1}.get(tokens[pos], 0)
            pos += 1
            if depth == 0:
                return

    def qualified_name():
        nonlocal pos
        name = tokens[pos]
        pos += 1
        while tokens[pos] == "::":
            name = tokens[pos + 1]
            pos += 2
        return name

    def body(parent, path):
        nonlocal pos
        while pos < len(tokens):
            tok = tokens[pos]
            if tok == "}":
                pos += 1
                return
            if tok == "package":
                name = tokens[pos + 1]
                pos += 2
                if tokens[pos] == "{":
                    pos += 1
                    body(parent, path + [name])
                continue
            if tok == "part" and tokens[pos + 1] == "def":
                name = tokens[pos + 2]
                pos += 3
                defs[name] = None
                if tokens[pos] == ":>":
                    pos += 1
                    defs[name] = qualified_name()
                while tokens[pos] not in ("{", ";"):
                    pos += 1
                if tokens[pos] == "{":
                    skip_block()
                else:
                    pos += 1
                continue
            if tok == "part" and tokens[pos + 2] == ":":
                part = Part(tokens[pos + 1], "")
                pos += 3
                part.type = qualified_name()
                part.package = "::".join(path)
                parent.children.append(part)
                while tokens[pos] not in ("{", ";"):
                    pos += 1
                pos += 1
                if tokens[pos - 1] == "{":
                    body(part, path)
                continue
            if tok == ":>>" and tokens[pos + 2] == "=":
                parent.attrs[tokens[pos + 1]] = literal(tokens[pos + 3])
                pos += 4
                continue
            if tok == "{":
                skip_block()
                continue
            pos += 1

    body(root, [])
    return root, defs


KINDS = {"Screen", "Panel", "TabBar", "ListView", "Label", "Button", "Link", "TextField",
         "PasswordField", "DateField", "ComboBox", "CheckBox", "RadioButton", "Tab", "ListItem"}
DEFS = {}


def kind(part):
    t = part.type
    while t not in KINDS:
        if DEFS.get(t) is None:
            sys.exit(f"Неизвестный тип элемента: {part.name} : {part.type}")
        t = DEFS[t]
    return t


# ---------------------------------------------------------------- рисование

_fonts = {}


def font(size, bold=False):
    key = (size, bold)
    if key not in _fonts:
        path = next((p for p in FONT_FILES[bold] if Path(p).exists()), None)
        if path is None:
            sys.exit("Не найден шрифт с кириллицей (Segoe UI или DejaVu Sans).")
        _fonts[key] = ImageFont.truetype(path, int(size * S))
    return _fonts[key]


def text_w(s, f):
    return f.getlength(s) / S


def line_h(f):
    return f.size / S * 1.35


def wrap(s, f, max_w):
    lines, cur = [], ""
    for word in str(s).split():
        cand = f"{cur} {word}".strip()
        if not cur or text_w(cand, f) <= max_w:
            cur = cand
        else:
            lines.append(cur)
            cur = word
    return lines + [cur] if cur else (lines or [""])


def ellipsize(s, f, max_w):
    if text_w(s, f) <= max_w:
        return s
    while s and text_w(s + "…", f) > max_w:
        s = s[:-1]
    return s + "…"


class Canvas:
    """Рисует в логических координатах; без картинки только измеряет."""

    def __init__(self, image=None):
        self.d = ImageDraw.Draw(image) if image else None

    def rect(self, x, y, w, h, fill=None, outline=None, r=6, width=1):
        if self.d:
            self.d.rounded_rectangle([x * S, y * S, (x + w) * S, (y + h) * S], radius=r * S,
                                     fill=fill, outline=outline, width=int(width * S))

    def ellipse(self, x, y, w, h, fill=None, outline=None, width=1):
        if self.d:
            self.d.ellipse([x * S, y * S, (x + w) * S, (y + h) * S], fill=fill, outline=outline,
                           width=int(width * S))

    def line(self, points, fill, width=1):
        if self.d:
            self.d.line([(px * S, py * S) for px, py in points], fill=fill, width=int(width * S),
                        joint="curve")

    def polygon(self, points, fill=None, outline=None):
        if self.d:
            self.d.polygon([(px * S, py * S) for px, py in points], fill=fill, outline=outline)

    def text(self, x, y, s, f, fill):
        if self.d:
            self.d.text((x * S, y * S), s, font=f, fill=fill)

    def lines(self, x, y, lines, f, fill, w=0, align="left"):
        for i, ln in enumerate(lines):
            dx = {"center": (w - text_w(ln, f)) / 2, "right": w - text_w(ln, f)}.get(align, 0)
            self.text(x + dx, y + i * line_h(f), ln, f, fill)
        return len(lines) * line_h(f)


MEASURE = Canvas()


def render(c, part, x, y, w):
    return RENDERERS[kind(part)](c, part, x, y, w)


def stack(c, parts, x, y, w, gap):
    h = 0
    for i, part in enumerate(parts):
        h += gap if i else 0
        h += render(c, part, x, y + h, w)
    return h


def row(c, parts, x, y, w, gap=10):
    cw = (w - gap * (len(parts) - 1)) / len(parts)
    heights = [render(MEASURE, p, 0, 0, cw) for p in parts]
    total = max(heights)
    # строки с подписанными полями выравниваются по низу (кнопка напротив поля), остальные — по центру
    bottom = any(kind(p) in ("TextField", "PasswordField", "DateField", "ComboBox") and p.get("label") for p in parts)
    for i, (part, h) in enumerate(zip(parts, heights)):
        render(c, part, x + i * (cw + gap), y + (total - h if bottom else (total - h) / 2), cw)
    return total


def r_panel(c, part, x, y, w):
    is_row = part.get("layout") == "row"

    def content(c2, x2, y2, w2):
        return row(c2, part.children, x2, y2, w2) if is_row else stack(c2, part.children, x2, y2, w2, 10)

    label = part.get("label")
    if not label:
        return content(c, x, y, w)
    pad, f = 12, font(13, True)
    caption = wrap(label, f, w - 2 * pad)
    cap_h = len(caption) * line_h(f)
    h = pad + cap_h + 8 + content(MEASURE, 0, 0, w - 2 * pad) + pad
    c.rect(x, y, w, h, fill="white", outline=BORDER, r=8)
    c.lines(x + pad, y + pad, caption, f, TEXT)
    content(c, x + pad, y + pad + cap_h + 8, w - 2 * pad)
    return h


def r_listview(c, part, x, y, w):
    h, pad = 0, 12
    if part.get("label"):
        f = font(13, True)
        h += c.lines(x, y, wrap(part.get("label"), f, w), f, TEXT) + 8
    heights = [render(MEASURE, it, 0, 0, w - 2 * pad) for it in part.children]
    box_h = sum(heights) + 2 * pad * len(heights)
    c.rect(x, y + h, w, box_h, fill="white", outline=BORDER, r=8)
    yy = y + h
    for i, (item, ih) in enumerate(zip(part.children, heights)):
        if i:
            c.line([(x, yy), (x + w, yy)], BORDER)
        render(c, item, x + pad, yy + pad, w - 2 * pad)
        yy += ih + 2 * pad
    return h + box_h


def r_list_item(c, part, x, y, w):
    label, details, value = part.get("label"), part.get("details"), part.get("value")
    enabled, style = part.get("enabled", True), part.get("style", "normal")
    f_label, f_det = font(14), font(12)
    is_amount = any(ch.isdigit() for ch in value)
    f_val = font(14, True) if is_amount else font(12)
    val_w = (text_w(value, f_val) + (0 if is_amount else 20)) if value else 0
    left_w = w - (val_w + 12 if value else 0)
    lab_lines = wrap(label, f_label, left_w)
    det_lines = wrap(details, f_det, left_w) if details else []
    text_h = len(lab_lines) * line_h(f_label) + (2 + len(det_lines) * line_h(f_det) if det_lines else 0)
    h = max(text_h, 28 if value and not is_amount else 0)
    yy = y + (h - text_h) / 2
    yy += c.lines(x, yy, lab_lines, f_label, TEXT)
    if det_lines:
        c.lines(x, yy + 2, det_lines, f_det, MUTED)
    if value and is_amount:
        color = {"positive": POSITIVE, "negative": NEGATIVE}.get(style, TEXT)
        c.text(x + w - val_w, y + (h - line_h(f_val)) / 2, value, f_val, color)
    elif value:
        bx, by = x + w - val_w, y + (h - 28) / 2
        if enabled:
            c.rect(bx, by, val_w, 28, fill="white", outline=NEGATIVE, r=6)
            c.lines(bx, by + (28 - line_h(f_val)) / 2, [value], f_val, NEGATIVE, val_w, "center")
        else:
            c.rect(bx, by, val_w, 28, fill=SOFT, outline=BORDER, r=6)
            c.lines(bx, by + (28 - line_h(f_val)) / 2, [value], f_val, DISABLED, val_w, "center")
    return h


def r_label(c, part, x, y, w):
    style = part.get("style", "normal")
    f, color = {
        "heading": (font(18, True), TEXT),
        "muted": (font(12), MUTED),
        "error": (font(12), NEGATIVE),
        "positive": (font(13), POSITIVE),
        "negative": (font(13), NEGATIVE),
    }.get(style, (font(13), TEXT))
    if style == "error":
        lines = wrap(part.get("label"), f, w - 20)
        h = len(lines) * line_h(f) + 16
        c.rect(x, y, w, h, fill=ERROR_BG, outline=ERROR_BORDER, r=6)
        c.lines(x + 10, y + 8, lines, f, color)
        return h
    return c.lines(x, y, wrap(part.get("label"), f, w), f, color)


def r_link(c, part, x, y, w):
    f = font(13)
    return c.lines(x, y, wrap(part.get("label"), f, w), f, PRIMARY)


def r_button(c, part, x, y, w):
    style, enabled = part.get("style", "normal"), part.get("enabled", True)
    f = font(13, True)
    lines = wrap(part.get("label"), f, w - 20)
    h = max(38, len(lines) * line_h(f) + 16)
    if not enabled:
        fill, outline, color = SOFT, BORDER, DISABLED
    elif style == "primary":
        fill, outline, color = PRIMARY, PRIMARY, "white"
    elif style == "danger":
        fill, outline, color = "white", NEGATIVE, NEGATIVE
    else:
        fill, outline, color = "white", BORDER, TEXT
    c.rect(x, y, w, h, fill=fill, outline=outline, r=6)
    c.lines(x, y + (h - len(lines) * line_h(f)) / 2, lines, f, color, w, "center")
    return h


def r_field(c, part, x, y, w):
    k = kind(part)
    h = 0
    if part.get("label"):
        f = font(12)
        h += c.lines(x, y, wrap(part.get("label"), f, w), f, TEXT) + 4
    by, bh = y + h, 38
    c.rect(x, by, w, bh, fill=SOFT if part.get("readOnly", False) else "white", outline=BORDER, r=6)
    value, placeholder = part.get("value"), part.get("placeholder")
    if k == "PasswordField" and value:
        value = "•" * 8
    icon_w = 26 if k in ("ComboBox", "DateField") else 0
    f = font(13)
    shown = ellipsize(value or placeholder, f, w - 20 - icon_w)
    c.text(x + 10, by + (bh - line_h(f)) / 2, shown, f, TEXT if value else DISABLED)
    if k == "ComboBox":
        cx, cy = x + w - 18, by + bh / 2
        c.polygon([(cx - 5, cy - 2), (cx + 5, cy - 2), (cx, cy + 4)], fill=MUTED)
    elif k == "DateField":
        ix, iy = x + w - 24, by + 12
        c.rect(ix, iy, 14, 14, outline=MUTED, r=2)
        c.line([(ix, iy + 4), (ix + 14, iy + 4)], MUTED)
    return h + bh


def r_check(c, part, x, y, w):
    radio, checked = kind(part) == "RadioButton", part.get("checked", False)
    f = font(13)
    lines = wrap(part.get("label"), f, w - 28)
    h = max(22, len(lines) * line_h(f))
    bx, by = x, y + (h - 18) / 2
    if radio:
        c.ellipse(bx, by, 18, 18, fill="white", outline=PRIMARY if checked else BORDER, width=1.5)
        if checked:
            c.ellipse(bx + 5, by + 5, 8, 8, fill=PRIMARY)
    elif checked:
        c.rect(bx, by, 18, 18, fill=PRIMARY, outline=PRIMARY, r=4)
        c.line([(bx + 4, by + 9), (bx + 8, by + 13), (bx + 14, by + 5)], "white", width=2)
    else:
        c.rect(bx, by, 18, 18, fill="white", outline=BORDER, r=4, width=1.5)
    c.lines(x + 28, y + (h - len(lines) * line_h(f)) / 2, lines, f, TEXT)
    return h


def r_tabbar(c, part, x, y, w):
    tabs, h = part.children, 40
    tw = w / len(tabs)
    c.line([(x, y + h), (x + w, y + h)], BORDER)
    for i, tab in enumerate(tabs):
        selected = tab.get("selected", False)
        f = font(13, selected)
        label = ellipsize(tab.get("label"), f, tw - 8)
        c.lines(x + i * tw, y + (h - line_h(f)) / 2, [label], f, PRIMARY if selected else MUTED, tw, "center")
        if selected:
            c.rect(x + i * tw + 6, y + h - 3, tw - 12, 3, fill=PRIMARY, r=1)
    return h + 1


RENDERERS = {
    "Panel": r_panel, "ListView": r_listview, "ListItem": r_list_item, "Label": r_label,
    "Link": r_link, "Button": r_button, "TextField": r_field, "PasswordField": r_field,
    "DateField": r_field, "ComboBox": r_field, "CheckBox": r_check, "RadioButton": r_check,
    "TabBar": r_tabbar,
}


def draw_screen(screen, out):
    content_w = SCREEN_W - 2 * PAD
    content_h = stack(MEASURE, screen.children, 0, 0, content_w, 14)

    margin, addr_h, bar_h = 16, 36, 48
    x0, y0 = margin, margin
    page_h = addr_h + bar_h + PAD + content_h + PAD
    width = SCREEN_W + 2 * margin
    height = int(y0 + page_h + margin)

    image = Image.new("RGB", (width * S, height * S), "white")
    c = Canvas(image)

    # окно браузера: адресная строка и шапка приложения
    c.rect(x0, y0, SCREEN_W, page_h, fill="white", outline=BORDER, r=10)
    c.rect(x0, y0, SCREEN_W, addr_h, fill="#e5e7eb", r=10)
    c.rect(x0, y0 + addr_h - 10, SCREEN_W, 10, fill="#e5e7eb", r=0)
    f_addr = font(11)
    c.rect(x0 + 10, y0 + 7, SCREEN_W - 20, addr_h - 14, fill="white", r=11)
    c.text(x0 + 22, y0 + (addr_h - line_h(f_addr)) / 2, "evenup.local" + screen.get("route"), f_addr, MUTED)

    by = y0 + addr_h
    c.rect(x0, by, SCREEN_W, bar_h, fill=APP_BAR, r=0)
    f_logo = font(17, True)
    c.text(x0 + PAD, by + (bar_h - line_h(f_logo)) / 2, "EvenUp", f_logo, "white")
    user = screen.get("currentUser")
    if user:
        f_user = font(12)
        uw = text_w(user, f_user)
        ux = x0 + SCREEN_W - PAD - uw
        c.text(ux, by + (bar_h - line_h(f_user)) / 2, user, f_user, "white")
        c.ellipse(ux - 32, by + bar_h / 2 - 12, 24, 24, fill="#3b82f6")
        c.lines(ux - 32, by + bar_h / 2 - line_h(f_user) / 2, [user[0]], font(12, True), "white", 24, "center")

    stack(c, screen.children, x0 + PAD, by + bar_h + PAD, content_w, 14)

    image.save(out, optimize=True)


def main():
    root, defs = parse(MODEL.read_text(encoding="utf-8"))
    DEFS.update(defs)
    screens = [p for p in root.children if p.package == PACKAGE and kind(p) == "Screen"]
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    for screen in screens:
        name = FILES.get(screen.name, screen.name)
        out = OUT_DIR / f"{name}.png"
        draw_screen(screen, out)
        print(f"[ok] {screen.name} -> {out.relative_to(HERE.parent.parent)}")


if __name__ == "__main__":
    main()
