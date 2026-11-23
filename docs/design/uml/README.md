# UML-модель поведения

Исходные тексты диаграмм на PlantUML и их изображения (`png/`).

| Файл | Диаграмма | Где описана |
|------|-----------|-------------|
| [uc-01-use-cases.puml](uc-01-use-cases.puml) | Прецеденты | [use-cases.md](../use-cases.md), [SDD 3.3](../sdd.md#33-views) |
| [act-01-add-expense.puml](act-01-add-expense.puml) | Активность: добавление траты | [SDD: поведенческая модель](../sdd.md#behavioral-uml-model), UC-08 |
| [st-01-group-member.puml](st-01-group-member.puml) | Состояния группы и участника | [SDD: поведенческая модель](../sdd.md#behavioral-uml-model), REQ-FUNC-005, 010, 016 |
| [seq-01-add-expense.puml](seq-01-add-expense.puml) | Последовательность: добавление траты | [SDD: поведенческая модель](../sdd.md#behavioral-uml-model), UC-08 |

`style.iuml` — общий стиль, подключается в каждой диаграмме. PNG получены расширением PlantUML для VS Code (PlantUML 1.2024.3): «Export Workspace Diagrams» или из консоли:

```bash
java -jar plantuml.jar -charset UTF-8 -tpng -o png *.puml
```
