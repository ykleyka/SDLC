#!/usr/bin/env python3
"""Экспорт официальных диаграмм модели EvenUp в PNG через Jupyter-ядро SysML v2.

Скрипт собирает ноутбук из EvenUp-mockups.sysml (ячейка с моделью и по ячейке
%viz на каждую диаграмму из VIEWS), выполняет его через `jupyter nbconvert`,
достаёт SVG из результатов и конвертирует их в PNG в каталог srs/images/.
Каркасы экранов рисует отдельный скрипт render_wireframes.py.

Запуск (WSL, в conda-окружении с jupyter-sysml-kernel):
    python3 export_mockups.py
    python3 export_mockups.py --notebook EvenUp-mockups.ipynb   # только экспорт из уже выполненного ноутбука
"""
import argparse
import json
import re
import shutil
import subprocess
import sys
import tempfile
from pathlib import Path

HERE = Path(__file__).resolve().parent
MODEL = HERE / "EvenUp-mockups.sysml"
NOTEBOOK = HERE / "EvenUp-mockups.ipynb"
OUT_DIR = HERE.parent / "images"
# (имя PNG, элемент модели, вид %viz)
VIEWS = [
    ("ui-00-navigation", "EvenUpMockups::Navigation::AppNavigation", "State"),
]


def code_cell(source):
    return {"cell_type": "code", "metadata": {}, "execution_count": None, "outputs": [], "source": source}


def build_notebook(kernel):
    cells = [code_cell(MODEL.read_text(encoding="utf-8"))]
    cells += [code_cell(f"%viz --view={view} {element}") for _, element, view in VIEWS]
    return {
        "nbformat": 4,
        "nbformat_minor": 4,
        "metadata": {"kernelspec": {"name": kernel, "display_name": "SysML", "language": "sysml"}},
        "cells": cells,
    }


def execute_notebook(notebook, kernel):
    with tempfile.TemporaryDirectory() as tmp:
        source = Path(tmp) / "source.ipynb"
        source.write_text(json.dumps(notebook, ensure_ascii=False, indent=1), encoding="utf-8")
        subprocess.run(
            [
                "jupyter", "nbconvert", "--to", "notebook", "--execute", "--allow-errors",
                f"--ExecutePreprocessor.kernel_name={kernel}",
                "--ExecutePreprocessor.timeout=600",
                "--output-dir", str(HERE), "--output", NOTEBOOK.stem,
                str(source),
            ],
            check=True,
        )


def joined(value):
    return "".join(value) if isinstance(value, list) else (value or "")


def output_text(cell):
    parts = []
    for out in cell.get("outputs", []):
        if out.get("output_type") == "stream":
            parts.append(joined(out.get("text")))
        elif out.get("output_type") == "error":
            parts.append(f"{out.get('ename')}: {out.get('evalue')}")
        else:
            parts.append(joined(out.get("data", {}).get("text/plain")))
    return "".join(parts).strip()


def find_svg(cell):
    for out in cell.get("outputs", []):
        data = out.get("data", {})
        if "image/svg+xml" in data:
            return joined(data["image/svg+xml"])
        match = re.search(r"<svg\b.*?</svg>", joined(data.get("text/html")), re.S)
        if match:
            return match.group(0)
    return None


def svg_to_png(svg, png):
    if shutil.which("rsvg-convert"):
        with tempfile.NamedTemporaryFile("w", suffix=".svg", encoding="utf-8", delete=False) as f:
            f.write(svg)
        try:
            subprocess.run(["rsvg-convert", "-z", "2", "-b", "white", "-o", str(png), f.name], check=True)
        finally:
            Path(f.name).unlink()
        return
    try:
        import cairosvg
    except ImportError:
        sys.exit("Нужен rsvg-convert (sudo apt install librsvg2-bin) или cairosvg (pip install cairosvg).")
    cairosvg.svg2png(bytestring=svg.encode("utf-8"), write_to=str(png), scale=2, background_color="white")


def export(notebook):
    cells = notebook["cells"]
    model_log = output_text(cells[0])
    if model_log:
        print("Результат загрузки модели:\n" + model_log + "\n")

    OUT_DIR.mkdir(parents=True, exist_ok=True)
    failed = 0
    for (file_name, element, _), cell in zip(VIEWS, cells[1:]):
        svg = find_svg(cell)
        if svg is None:
            failed += 1
            print(f"[нет картинки] {element}: {output_text(cell) or 'пустой вывод'}")
            continue
        png = OUT_DIR / f"{file_name}.png"
        svg_to_png(svg, png)
        print(f"[ok] {element} -> {png.relative_to(HERE.parent.parent)}")
    return failed


def main():
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("--kernel", default="sysml", help="имя Jupyter-ядра SysML (см. `jupyter kernelspec list`)")
    parser.add_argument("--notebook", type=Path, help="экспортировать из уже выполненного ноутбука, не запуская ядро")
    args = parser.parse_args()

    if args.notebook:
        path = args.notebook
    else:
        execute_notebook(build_notebook(args.kernel), args.kernel)
        path = NOTEBOOK
    failed = export(json.loads(path.read_text(encoding="utf-8")))
    sys.exit(1 if failed else 0)


if __name__ == "__main__":
    main()
