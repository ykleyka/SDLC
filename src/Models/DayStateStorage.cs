using System.Globalization;
using System.Text.Json;

namespace idealDay.Models
{
    internal static class DayStateStorage
    {
        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "IdealDay");

        private static readonly string SettingsFile = Path.Combine(SettingsDirectory, "day-state.json");

        public static void Save(TimeSpan wakeUpTime, double sleepHours, double workHours)
        {
            try
            {
                Directory.CreateDirectory(SettingsDirectory);
                var state = new SavedDayState
                {
                    WakeUpTime = wakeUpTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture),
                    SleepHours = sleepHours,
                    WorkHours = workHours
                };

                var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFile, json);
            }
            catch (IOException)
            {
                // Ошибка сохранения настроек не должна мешать работе калькулятора.
            }
            catch (UnauthorizedAccessException)
            {
                // Восстановление просто будет недоступно при ограниченных правах пользователя.
            }
        }

        public static bool TryLoad(out SavedDayState state)
        {
            state = new SavedDayState();

            try
            {
                if (!File.Exists(SettingsFile))
                {
                    return false;
                }

                var json = File.ReadAllText(SettingsFile);
                var savedState = JsonSerializer.Deserialize<SavedDayState>(json);
                if (savedState == null || string.IsNullOrWhiteSpace(savedState.WakeUpTime))
                {
                    return false;
                }

                state = savedState;
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        internal sealed class SavedDayState
        {
            public string WakeUpTime { get; set; } = "07:00";
            public double SleepHours { get; set; } = 8;
            public double WorkHours { get; set; } = 8;
        }
    }
}
