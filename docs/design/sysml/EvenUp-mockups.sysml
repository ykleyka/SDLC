package EvenUpMockups {
    doc /* Наброски (low-fidelity мокапы) интерфейса веб-приложения EvenUp (SRS v0.1, раздел 3.1.1).
         * Экран — part-usage типа Screen, вложенные части — элементы интерфейса.
         * Имя части — текст, который видит пользователь, тип — вид элемента.
         * Порядок частей задаёт порядок элементов на экране сверху вниз
         * (при отрисовке вложенные контейнеры идут перед одиночными элементами,
         * поэтому одиночные элементы в начале экрана объединены в панели).
         * Обозначения: [Вкладка] — выбранная вкладка, [x] / [ ] — флажок, (o) / ( ) — переключатель. */

    // ================================================================
    // Виды элементов интерфейса
    // ================================================================
    package UI {
        part def Screen;     // экран (страница)
        part def AppBar;     // верхняя панель приложения
        part def Panel;      // группа элементов
        part def Tabs;       // панель вкладок
        part def Tab;        // вкладка
        part def List;       // список
        part def Item;       // строка списка
        part def Text;       // надпись
        part def Field;      // поле ввода
        part def Select;     // выпадающий список
        part def CheckBox;   // флажок
        part def Radio;      // переключатель
        part def Button;     // кнопка
        part def Link;       // ссылка
    }

    // ================================================================
    // Экраны
    // ================================================================
    package Screens {
        private import EvenUpMockups::UI::*;

        // UI-01. Вход / регистрация (REQ-FUNC-001, 002), маршрут /login
        part <loginScreen> 'UI-01. Вход' : Screen {
            part 'Вход или регистрация' : Tabs {
                part '[Вход]' : Tab;
                part 'Регистрация' : Tab;
            }
            part 'Email: name@example.com' : Field;
            part 'Пароль: не менее 8 символов' : Field;
            part 'Неверный email или пароль' : Text;
            part 'Войти' : Button;
            part 'Нет аккаунта? Зарегистрироваться' : Link;
        }

        // UI-02. Мои группы (REQ-FUNC-003, 004, 005), маршрут /groups
        part <groupsScreen> 'UI-02. Мои группы' : Screen {
            part 'EvenUp · Аня' : AppBar {
                part '+ Создать группу' : Button;
            }
            part 'Группы' : List {
                part 'Поездка в Минск · 3 уч. · +50,00 BYN' : Item;
                part 'Квартира · 4 уч. · −12,40 BYN' : Item;
                part 'День рождения Бориса · 6 уч. · 0,00 BYN' : Item;
            }
            part 'Архив (2) — показать' : Link;
        }

        // UI-03. Группа → Траты (REQ-FUNC-011, 019, 020), маршрут /groups/{id}/expenses
        part <expensesScreen> 'UI-03. Группа: траты' : Screen {
            part '← Мои группы · Поездка в Минск' : Tabs {
                part '[Траты]' : Tab;
                part 'Балансы' : Tab;
                part 'Участники' : Tab;
                part 'История' : Tab;
            }
            part 'Действия' : Panel {
                part 'Участник: Все' : Select;
                part '+ Добавить трату' : Button;
            }
            part 'Траты' : List {
                part 'Ужин · 30,00 BYN · платил Борис, 13.10' : Item;
                part 'Жильё · 90,00 BYN · платила Аня, 12.10' : Item;
            }
        }

        // UI-04. Форма траты (REQ-FUNC-011, 012, 013), маршрут /groups/{id}/expenses/new
        part <expenseFormScreen> 'UI-04. Новая трата' : Screen {
            part 'Трата' : Panel {
                part 'Описание: Ужин' : Field;
                part 'Сумма, BYN: 30,00' : Field;
                part 'Дата: 13.10.2026' : Field;
                part 'Кто платил: Борис' : Select;
            }
            part 'Делить поровну между' : Panel {
                part '[x] Аня' : CheckBox;
                part '[x] Борис' : CheckBox;
                part '[x] Вика (без аккаунта)' : CheckBox;
                part 'По 10,00 BYN на каждого' : Text;
            }
            part 'Отмена' : Button;
            part 'Сохранить' : Button;
        }

        // UI-05. Группа → Балансы (REQ-FUNC-014, 015, 016), маршрут /groups/{id}/balances
        part <balancesScreen> 'UI-05. Группа: балансы' : Screen {
            part '← Мои группы · Поездка в Минск' : Tabs {
                part 'Траты' : Tab;
                part '[Балансы]' : Tab;
                part 'Участники' : Tab;
                part 'История' : Tab;
            }
            part 'Балансы участников' : List {
                part 'Аня: +50,00 BYN' : Item;
                part 'Борис: −10,00 BYN' : Item;
                part 'Вика: −40,00 BYN' : Item;
            }
            part 'Кто кому переводит' : List {
                part 'Вика → Аня: 40,00 BYN' : Item {
                    part 'Отметить выполненным' : Button;
                }
                part 'Борис → Аня: 10,00 BYN' : Item {
                    part 'Отметить выполненным' : Button;
                }
            }
            part 'Записать перевод вручную' : Button;
        }

        // UI-06. Группа → Участники (REQ-FUNC-006, 008, 010, 021), маршрут /groups/{id}/members
        part <membersScreen> 'UI-06. Группа: участники' : Screen {
            part '← Мои группы · Поездка в Минск' : Tabs {
                part 'Траты' : Tab;
                part 'Балансы' : Tab;
                part '[Участники]' : Tab;
                part 'История' : Tab;
            }
            part 'Участники (3 из 30)' : List {
                part 'Аня' : Item {
                    part 'Создатель группы' : Text;
                }
                part 'Борис' : Item {
                    part 'Удалить' : Button;
                }
                part 'Вика · без аккаунта · баланс не 0' : Item {
                    part 'Удалить (недоступно)' : Button;
                }
            }
            part 'Ссылка-приглашение' : Panel {
                part 'evenup.local/join/7f3a…' : Field;
                part 'Скопировать' : Button;
                part 'Перевыпустить' : Button;
            }
            part 'Участник без аккаунта' : Panel {
                part 'Имя' : Field;
                part 'Добавить' : Button;
            }
        }

        // UI-07. Группа → История (REQ-FUNC-018), маршрут /groups/{id}/history
        part <historyScreen> 'UI-07. Группа: история' : Screen {
            part '← Мои группы · Поездка в Минск' : Tabs {
                part 'Траты' : Tab;
                part 'Балансы' : Tab;
                part 'Участники' : Tab;
                part '[История]' : Tab;
            }
            part 'Лента операций' : List {
                part '13.10 21:40 · Борис добавил трату «Ужин», 30,00 BYN' : Item;
                part '12.10 15:05 · Аня добавила трату «Жильё», 90,00 BYN' : Item;
                part '12.10 15:01 · Аня добавила участника Вика' : Item;
                part '12.10 14:58 · Борис вступил в группу' : Item;
            }
        }

        // UI-08. Вступление в группу (REQ-FUNC-007, 009), маршрут /join/{token}
        part <joinScreen> 'UI-08. Приглашение в группу' : Screen {
            part 'Вас пригласили в группу «Поездка в Минск»' : Panel {
                part 'Создатель: Аня · 3 участника · BYN' : Text;
            }
            part 'Кто вы в этой группе?' : Panel {
                part '( ) Я новый участник' : Radio;
                part '(o) Я — Вика (без аккаунта)' : Radio;
            }
            part 'Отказаться' : Button;
            part 'Вступить' : Button;
        }
    }

    // ================================================================
    // Навигация между экранами
    // ================================================================
    package Navigation {
        doc /* Состояние — открытый экран, переход — действие пользователя.
             * Вкладки группы переключаются из любой вкладки (переходы от составного состояния groupView). */

        attribute def SignIn;
        attribute def SignInWithInvite;
        attribute def SignOut;
        attribute def OpenGroup;
        attribute def BackToGroups;
        attribute def OpenInviteLink;
        attribute def JoinGroup;
        attribute def DeclineInvite;
        attribute def SelectExpensesTab;
        attribute def SelectBalancesTab;
        attribute def SelectMembersTab;
        attribute def SelectHistoryTab;
        attribute def AddExpense;
        attribute def EditExpense;
        attribute def SaveExpense;
        attribute def CancelExpense;

        state def AppNavigation {
            entry; then login;

            state login;      // UI-01
            state groups;     // UI-02
            state invitation; // UI-08

            state groupView {
                entry; then expenses;

                state expenses;      // UI-03
                state expenseForm;   // UI-04
                state balances;      // UI-05
                state members;       // UI-06
                state history;       // UI-07

                transition addExpense first expenses accept AddExpense then expenseForm;
                transition editExpense first expenses accept EditExpense then expenseForm;
                transition saveExpense first expenseForm accept SaveExpense then expenses;
                transition cancelExpense first expenseForm accept CancelExpense then expenses;
            }

            transition signIn first login accept SignIn then groups;
            transition signInWithInvite first login accept SignInWithInvite then invitation;
            transition signOut first groups accept SignOut then login;
            transition openGroup first groups accept OpenGroup then groupView;
            transition openInvite first groups accept OpenInviteLink then invitation;
            transition joinGroup first invitation accept JoinGroup then groupView;
            transition declineInvite first invitation accept DeclineInvite then groups;
            transition backToGroups first groupView accept BackToGroups then groups;

            transition toExpenses first groupView accept SelectExpensesTab then groupView.expenses;
            transition toBalances first groupView accept SelectBalancesTab then groupView.balances;
            transition toMembers first groupView accept SelectMembersTab then groupView.members;
            transition toHistory first groupView accept SelectHistoryTab then groupView.history;
        }
    }
}
