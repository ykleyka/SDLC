using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace idealDay.Models
{
    // Активная модель: при изменении входных данных она сразу пересчитывает результат
    // и уведомляет представление через событие PropertyChanged.
    public sealed class IdealDayModel : INotifyPropertyChanged
    {
        private TimeSpan wakeUpTime = new(7, 0, 0);
        private double sleepHours = 8;
        private double workHours = 8;
        private double restHours;
        private int idealScore;
        private string assessment = string.Empty;
        private string alternativeSchedule = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public TimeSpan WakeUpTime => wakeUpTime;
        public double SleepHours => sleepHours;
        public double WorkHours => workHours;
        public double RestHours => restHours;
        public int IdealScore => idealScore;
        public string Assessment => assessment;
        public string AlternativeSchedule => alternativeSchedule;
        public string WakeUpText => FormatTime(wakeUpTime);

        public IdealDayModel()
        {
            if (DayStateStorage.TryLoad(out var savedState)
                && TimeSpan.TryParseExact(savedState.WakeUpTime, @"hh\:mm", CultureInfo.InvariantCulture, out var savedWakeUp)
                && IsValidInput(savedWakeUp, savedState.SleepHours, savedState.WorkHours))
            {
                wakeUpTime = savedWakeUp;
                sleepHours = savedState.SleepHours;
                workHours = savedState.WorkHours;
            }

            Recalculate();
        }

        public void Update(TimeSpan newWakeUpTime, double newSleepHours, double newWorkHours)
        {
            Validate(newWakeUpTime, newSleepHours, newWorkHours);

            wakeUpTime = newWakeUpTime;
            sleepHours = newSleepHours;
            workHours = newWorkHours;

            Recalculate();
            DayStateStorage.Save(wakeUpTime, sleepHours, workHours);
            OnPropertyChanged(string.Empty);
        }

        public void Reset()
        {
            Update(new TimeSpan(7, 0, 0), 8, 8);
        }

        private void Recalculate()
        {
            restHours = 24 - sleepHours - workHours;

            var sleepScore = Math.Max(0, 100 - Math.Abs(sleepHours - 8) * 25);
            var workScore = Math.Max(0, 100 - Math.Abs(workHours - 8) * 15);
            var restScore = Math.Max(0, 100 - Math.Abs(restHours - 8) * 8);

            idealScore = Math.Clamp(
                (int)Math.Round(sleepScore * 0.5 + workScore * 0.3 + restScore * 0.2),
                0,
                100);

            assessment = idealScore switch
            {
                >= 90 => "Идеальный баланс",
                >= 70 => "Хороший день",
                _ => "День можно улучшить"
            };

            var workStart = AddHours(wakeUpTime, 2);
            var workEnd = AddHours(workStart, 8);
            var sleepStart = AddHours(wakeUpTime, 16);

            alternativeSchedule =
                $"{FormatTime(wakeUpTime)} — подъём" + Environment.NewLine +
                $"{FormatTime(workStart)}–{FormatTime(workEnd)} — работа / учёба" + Environment.NewLine +
                $"{FormatTime(workEnd)}–{FormatTime(sleepStart)} — отдых" + Environment.NewLine +
                $"{FormatTime(sleepStart)} — сон до {FormatTime(wakeUpTime)} (8 ч)";
        }

        private static void Validate(TimeSpan wakeUpTime, double sleep, double work)
        {
            if (wakeUpTime < TimeSpan.Zero || wakeUpTime >= TimeSpan.FromDays(1))
            {
                throw new ArgumentException("Время подъёма должно быть в диапазоне от 00:00 до 23:59.");
            }

            if (double.IsNaN(sleep) || double.IsInfinity(sleep)
                || double.IsNaN(work) || double.IsInfinity(work)
                || sleep < 1 || sleep > 14 || work < 0 || work > 16)
            {
                throw new ArgumentException("Проверьте длительность сна и работы.");
            }

            if (sleep + work > 24)
            {
                throw new ArgumentException("Сон и работа вместе не могут занимать больше 24 часов.");
            }
        }

        private static bool IsValidInput(TimeSpan wakeUpTime, double sleep, double work)
        {
            return wakeUpTime >= TimeSpan.Zero
                && wakeUpTime < TimeSpan.FromDays(1)
                && !double.IsNaN(sleep)
                && !double.IsInfinity(sleep)
                && !double.IsNaN(work)
                && !double.IsInfinity(work)
                && sleep >= 1
                && sleep <= 14
                && work >= 0
                && work <= 16
                && sleep + work <= 24;
        }

        private static string FormatTime(TimeSpan time)
        {
            return time.ToString(@"hh\:mm");
        }

        private static TimeSpan AddHours(TimeSpan time, double hours)
        {
            return TimeSpan.FromHours((time.TotalHours + hours) % 24);
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
