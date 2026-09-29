using idealDay.Controllers;
using idealDay.Models;

namespace idealDay
{
    public partial class Form1 : Form
    {
        private readonly DayController controller;

        public Form1()
        {
            InitializeComponent();

            controller = new DayController(new IdealDayModel());
            controller.Model.PropertyChanged += Model_PropertyChanged;

            btnEnterData.Click += BtnEnterData_Click;
            btnReset.Click += BtnReset_Click;

            UpdateView();
        }

        private void BtnEnterData_Click(object? sender, EventArgs e)
        {
            using var inputForm = new InputForm(controller.Model);

            if (inputForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (!controller.TryApplyInput(
                    inputForm.WakeUpTime,
                    inputForm.SleepHours,
                    inputForm.WorkHours,
                    out var errorMessage))
            {
                MessageBox.Show(errorMessage, "Проверьте данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnReset_Click(object? sender, EventArgs e)
        {
            controller.Reset();
        }

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var model = controller.Model;

            lblWakeUpValue.Text = model.WakeUpText;
            lblSleepValue.Text = FormatDuration(model.SleepHours);
            lblWorkValue.Text = FormatDuration(model.WorkHours);
            lblRestValue.Text = FormatDuration(model.RestHours);

            lblScore.Text = model.IdealScore.ToString();
            progressIdeal.Value = Math.Clamp(model.IdealScore, 0, 100);
            lblAssessment.Text = model.Assessment;
            lblComment.Text = $"Сон {FormatDuration(model.SleepHours)}   Работа {FormatDuration(model.WorkHours)}   Отдых {FormatDuration(model.RestHours)}";
            lblAlternative.Text = model.AlternativeSchedule;
            lblLastUpdated.Text = $"Последний расчёт: {DateTime.Now:HH:mm:ss}";
        }

        private static string FormatDuration(double hours)
        {
            return TimeSpan.FromHours(hours).ToString(@"hh\:mm");
        }
    }
}
