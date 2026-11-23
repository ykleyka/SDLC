using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var members = new List<string>();
var expenses = new List<Expense>();

Console.WriteLine("EvenUp — учебный прототип учёта совместных расходов");
Console.WriteLine("Данные хранятся только до закрытия программы.\n");

while (true)
{
    Console.WriteLine("1 — добавить участника");
    Console.WriteLine("2 — добавить трату (делится поровну между всеми)");
    Console.WriteLine("3 — показать балансы");
    Console.WriteLine("4 — предложить переводы");
    Console.WriteLine("0 — выход");
    Console.Write("Выбор: ");

    switch (Console.ReadLine())
    {
        case "1": AddMember(); break;
        case "2": AddExpense(); break;
        case "3": ShowBalances(); break;
        case "4": ShowSettlements(); break;
        case "0": return;
        default: Console.WriteLine("Выберите пункт меню от 0 до 4.\n"); break;
    }
}

void AddMember()
{
    Console.Write("Имя участника: ");
    var name = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Имя не должно быть пустым.\n");
        return;
    }
    if (members.Contains(name, StringComparer.OrdinalIgnoreCase))
    {
        Console.WriteLine("Участник с таким именем уже есть.\n");
        return;
    }
    members.Add(name);
    Console.WriteLine($"Добавлен участник: {name}.\n");
}

void AddExpense()
{
    if (members.Count == 0)
    {
        Console.WriteLine("Сначала добавьте участников.\n");
        return;
    }

    Console.Write("Описание траты: ");
    var description = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(description))
    {
        Console.WriteLine("Описание не должно быть пустым.\n");
        return;
    }

    Console.Write("Сумма (например, 12,50): ");
    if (!TryReadAmount(Console.ReadLine(), out var amountCents))
    {
        Console.WriteLine("Введите сумму больше нуля, не более двух знаков после запятой.\n");
        return;
    }

    PrintMembers();
    Console.Write("Номер плательщика: ");
    if (!int.TryParse(Console.ReadLine(), out var payerNumber) || payerNumber < 1 || payerNumber > members.Count)
    {
        Console.WriteLine("Неверный номер участника. Трата не добавлена.\n");
        return;
    }

    var shares = Split(amountCents, members.Count);
    expenses.Add(new Expense(description, payerNumber - 1, amountCents, shares));
    Console.WriteLine("Трата добавлена.\n");
}

void ShowBalances()
{
    if (members.Count == 0)
    {
        Console.WriteLine("Участников пока нет.\n");
        return;
    }

    var balances = CalculateBalances();
    Console.WriteLine("Балансы (плюс — участнику должны, минус — должен он):");
    for (var i = 0; i < members.Count; i++)
        Console.WriteLine($"{members[i]}: {FormatMoney(balances[i])}");
    Console.WriteLine();
}

void ShowSettlements()
{
    if (expenses.Count == 0)
    {
        Console.WriteLine("Трат пока нет.\n");
        return;
    }

    var debtors = CalculateBalances()
        .Select((balance, index) => new Account(index, balance))
        .Where(account => account.Balance < 0)
        .OrderBy(account => account.Balance)
        .ToList();
    var creditors = CalculateBalances()
        .Select((balance, index) => new Account(index, balance))
        .Where(account => account.Balance > 0)
        .OrderByDescending(account => account.Balance)
        .ToList();

    if (debtors.Count == 0 && creditors.Count == 0)
    {
        Console.WriteLine("Все рассчитались.\n");
        return;
    }

    Console.WriteLine("Рекомендуемые переводы:");
    var d = 0;
    var c = 0;
    while (d < debtors.Count && c < creditors.Count)
    {
        var amount = Math.Min(-debtors[d].Balance, creditors[c].Balance);
        Console.WriteLine($"{members[debtors[d].MemberIndex]} → {members[creditors[c].MemberIndex]}: {FormatMoney(amount)}");
        debtors[d] = debtors[d] with { Balance = debtors[d].Balance + amount };
        creditors[c] = creditors[c] with { Balance = creditors[c].Balance - amount };
        if (debtors[d].Balance == 0) d++;
        if (creditors[c].Balance == 0) c++;
    }
    Console.WriteLine();
}

long[] CalculateBalances()
{
    var balances = new long[members.Count];
    foreach (var expense in expenses)
    {
        balances[expense.PayerIndex] += expense.AmountCents;
        for (var i = 0; i < expense.Shares.Length; i++)
            balances[i] -= expense.Shares[i];
    }
    return balances;
}

void PrintMembers()
{
    for (var i = 0; i < members.Count; i++)
        Console.WriteLine($"{i + 1} — {members[i]}");
}

static long[] Split(long amountCents, int count)
{
    var baseShare = amountCents / count;
    var remainder = amountCents % count;
    return Enumerable.Range(0, count)
        .Select(index => baseShare + (index < remainder ? 1 : 0))
        .ToArray();
}

static bool TryReadAmount(string? input, out long cents)
{
    cents = 0;
    var styles = NumberStyles.Number;
    var parsed = decimal.TryParse(input, styles, CultureInfo.GetCultureInfo("ru-RU"), out var amount)
        || decimal.TryParse(input, styles, CultureInfo.InvariantCulture, out amount);
    if (!parsed || amount <= 0 || amount > (decimal)long.MaxValue / 100 || amount != decimal.Round(amount, 2))
        return false;
    cents = decimal.ToInt64(amount * 100);
    return true;
}

static string FormatMoney(long cents) => $"{cents / 100m:N2} BYN";

record Expense(string Description, int PayerIndex, long AmountCents, long[] Shares);
record Account(int MemberIndex, long Balance);
