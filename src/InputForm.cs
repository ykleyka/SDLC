using System.Globalization;
using idealDay.Models;

namespace idealDay
{
    public partial class InputForm : Form
    {
        public TimeSpan WakeUpTime { get; private set; }
        public double SleepHours { get; private set; }
        public double WorkHours { get; private set; }

        public InputForm(IdealDayModel model)
        {
            InitializeComponent();

            txtWakeUp.Text = model.WakeUpText;
            txtSleep.Text = FormatDuration(model.SleepHours);
            txtWork.Text = FormatDuration(model.WorkHours);
            UpdateRestPreview();

            txtSleep.TextChanged += (_, _) => UpdateRestPreview();
            txtWork.TextChanged += (_, _) => UpdateRestPreview();
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;
            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!DateTime.TryParseExact(
                    txtWakeUp.Text,
                    "HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedWakeUp))
            {
                ShowValidationMessage("Введите время подъёма в формате ЧЧ:ММ.", txtWakeUp);
                return;
            }

            if (!TryReadDuration(txtSleep, "сна", 1, 14, out var sleepHours))
            {
                return;
            }

            if (!TryReadDuration(txtWork, "работы", 0, 16, out var workHours))
            {
                return;
            }

            if (sleepHours + workHours > 24)
            {
                ShowValidationMessage("Сон и работа вместе не могут занимать больше 24 часов.", txtWork);
                return;
            }

            WakeUpTime = parsedWakeUp.TimeOfDay;
            SleepHours = sleepHours;
            WorkHours = workHours;
            DialogResult = DialogResult.OK;
        }

        private void UpdateRestPreview()
        {
            if (TryParseDuration(txtSleep.Text, out var sleep)
                && TryParseDuration(txtWork.Text, out var work)
                && sleep.TotalHours + work.TotalHours <= 24)
            {
                var rest = TimeSpan.FromHours(24 - sleep.TotalHours - work.TotalHours);
                lblRestValue.Text = rest.ToString(@"hh\:mm");
                return;
            }

            lblRestValue.Text = "--:--";
        }

        private bool TryReadDuration(MaskedTextBox input, string name, double minHours, double maxHours, out double hours)
        {
            hours = 0;
            if (!TryParseDuration(input.Text, out var duration))
            {
                ShowValidationMessage($"Введите время {name} в формате ЧЧ:ММ.", input);
                return false;
            }

            hours = duration.TotalHours;
            if (hours < minHours || hours > maxHours)
            {
                ShowValidationMessage($"Время {name} должно быть от {minHours:0.#} до {maxHours:0.#} часов.", input);
                return false;
            }

            return true;
        }

        private static bool TryParseDuration(string text, out TimeSpan duration)
        {
            return TimeSpan.TryParseExact(
                text,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                out duration);
        }

        private static string FormatDuration(double hours)
        {
            return TimeSpan.FromHours(hours).ToString(@"hh\:mm");
        }

        private static void ShowValidationMessage(string message, Control control)
        {
            MessageBox.Show(message, "Некорректные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }
    }
}
