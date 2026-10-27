# Мокапы интерфейса EvenUp (SysML v2)

Модель: [EvenUp-mockups.sysml](EvenUp-mockups.sysml). Библиотека элементов UI находится в пакете `EvenUpMockups::UI`, восемь экранов — в пакете `EvenUpMockups::Screens`.

## Как получить PNG (Jupyter с ядром SysML v2)

1. В первую ячейку вставить весь файл `EvenUp-mockups.sysml` и выполнить её.
2. В следующих ячейках выполнить команды и сохранить каждую картинку под указанным именем в `srs/images/`:

| Файл PNG | Команда |
|----------|---------|
| ui-01-login.png | `%viz --view=Interconnection EvenUpMockups::Screens::loginScreen` |
| ui-02-groups.png | `%viz --view=Interconnection EvenUpMockups::Screens::groupsScreen` |
| ui-03-expenses.png | `%viz --view=Interconnection EvenUpMockups::Screens::expensesScreen` |
| ui-04-expense-form.png | `%viz --view=Interconnection EvenUpMockups::Screens::expenseFormScreen` |
| ui-05-balances.png | `%viz --view=Interconnection EvenUpMockups::Screens::balancesScreen` |
| ui-06-members.png | `%viz --view=Interconnection EvenUpMockups::Screens::membersScreen` |
| ui-07-history.png | `%viz --view=Interconnection EvenUpMockups::Screens::historyScreen` |
| ui-08-join.png | `%viz --view=Interconnection EvenUpMockups::Screens::joinScreen` |

Если в Interconnection-виде не видны значения атрибутов (подписи кнопок, полей), используйте `--view=Tree`. Иерархию типов элементов UI показывает команда `%viz --view=Tree EvenUpMockups::UI`.
