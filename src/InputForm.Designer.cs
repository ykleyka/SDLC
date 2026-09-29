namespace idealDay
{
    partial class InputForm
    {
        private System.ComponentModel.IContainer components = null!;
        private Label lblTitle;
        private Label lblWakeUp;
        private Label lblSleep;
        private Label lblWork;
        private Label lblRest;
        private Label lblRestValue;
        private MaskedTextBox txtWakeUp;
        private MaskedTextBox txtSleep;
        private MaskedTextBox txtWork;
        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            lblWakeUp = new Label();
            lblSleep = new Label();
            lblWork = new Label();
            lblRest = new Label();
            lblRestValue = new Label();
            txtWakeUp = new MaskedTextBox();
            txtSleep = new MaskedTextBox();
            txtWork = new MaskedTextBox();
            btnSave = new Button();
            btnCancel = new Button();

            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            BackColor = Theme.Background;
            ClientSize = new Size(470, 380);
            MinimumSize = new Size(430, 360);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ввод данных";
            Font = Theme.RegularFont;

            var titlePanel = new Panel
            {
                BackColor = Theme.Background,
                Dock = DockStyle.Top,
                Height = 72,
                Padding = new Padding(24, 12, 24, 8)
            };

            lblTitle.AutoSize = false;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = Theme.InputTitleFont;
            lblTitle.ForeColor = Theme.MainText;
            lblTitle.Text = "Настройте свой день";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            titlePanel.Controls.Add(lblTitle);

            var inputLayout = new TableLayoutPanel
            {
                BackColor = Theme.Background,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 8, 24, 18),
                RowCount = 6
            };
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            ConfigureCaption(lblWakeUp, "Время подъёма");
            ConfigureCaption(lblSleep, "Время сна");
            ConfigureCaption(lblWork, "Время работы / учёбы");
            ConfigureCaption(lblRest, "Время отдыха");
            inputLayout.Controls.Add(lblWakeUp, 0, 0);
            inputLayout.Controls.Add(lblSleep, 0, 1);
            inputLayout.Controls.Add(lblWork, 0, 2);
            inputLayout.Controls.Add(lblRest, 0, 3);

            ConfigureTimeInput(txtWakeUp, "00:00");
            ConfigureTimeInput(txtSleep, "00:00");
            ConfigureTimeInput(txtWork, "00:00");
            ConfigurePreview(lblRestValue);
            inputLayout.Controls.Add(txtWakeUp, 1, 0);
            inputLayout.Controls.Add(txtSleep, 1, 1);
            inputLayout.Controls.Add(txtWork, 1, 2);
            inputLayout.Controls.Add(lblRestValue, 1, 3);

            var buttons = new FlowLayoutPanel
            {
                AutoSize = false,
                BackColor = Theme.Background,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 6, 0, 0),
                WrapContents = false
            };
            ConfigurePrimaryButton(btnSave, "Рассчитать");
            ConfigureSecondaryButton(btnCancel, "Отмена");
            buttons.Controls.Add(btnSave);
            buttons.Controls.Add(btnCancel);
            inputLayout.Controls.Add(buttons, 0, 5);
            inputLayout.SetColumnSpan(buttons, 2);

            Controls.Add(inputLayout);
            Controls.Add(titlePanel);
            ResumeLayout(false);
        }

        private static void ConfigureCaption(Label label, string text)
        {
            label.AutoSize = false;
            label.Dock = DockStyle.Fill;
            label.Font = Theme.RegularFont;
            label.ForeColor = Theme.SecondaryText;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        private static void ConfigureTimeInput(MaskedTextBox input, string mask)
        {
            input.BackColor = Theme.Control;
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Dock = DockStyle.Fill;
            input.Font = Theme.ValueFont;
            input.ForeColor = Theme.MainText;
            input.Mask = mask;
            input.Margin = new Padding(4, 5, 4, 5);
            input.PromptChar = '_';
            input.TextAlign = HorizontalAlignment.Center;
        }

        private static void ConfigurePreview(Label label)
        {
            label.AutoSize = false;
            label.BackColor = Theme.Control;
            label.Dock = DockStyle.Fill;
            label.Font = Theme.ValueFont;
            label.ForeColor = Theme.Accent;
            label.Margin = new Padding(4, 5, 4, 5);
            label.Text = "--:--";
            label.TextAlign = ContentAlignment.MiddleCenter;
        }

        private static void ConfigurePrimaryButton(Button button, string text)
        {
            button.BackColor = Theme.Accent;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = Theme.ButtonFont;
            button.ForeColor = Color.White;
            button.Margin = new Padding(0, 0, 10, 0);
            button.Size = new Size(170, 40);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }

        private static void ConfigureSecondaryButton(Button button, string text)
        {
            button.BackColor = Theme.Control;
            button.DialogResult = DialogResult.Cancel;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = Theme.SmallFont;
            button.ForeColor = Theme.MainText;
            button.Margin = new Padding(0);
            button.Size = new Size(170, 40);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }
    }
}
