using System.Drawing.Text;

namespace idealDay
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null!;

        private Panel panelHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private TableLayoutPanel layoutContent;
        private GroupBox groupInput;
        private Label lblInputHint;
        private Label lblWakeUpCaption;
        private Label lblSleepCaption;
        private Label lblWorkCaption;
        private Label lblRestCaption;
        private Label lblWakeUpValue;
        private Label lblSleepValue;
        private Label lblWorkValue;
        private Label lblRestValue;
        private Button btnEnterData;
        private Button btnReset;
        private GroupBox groupResult;
        private Label lblScoreTitle;
        private Label lblScore;
        private Label lblScoreUnit;
        private ProgressBar progressIdeal;
        private Label lblAssessment;
        private Label lblComment;
        private Label lblAlternativeTitle;
        private Label lblAlternative;
        private Panel panelFooter;
        private Label lblMvc;
        private Label lblLastUpdated;

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
            panelHeader = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            layoutContent = new TableLayoutPanel();
            groupInput = new GroupBox();
            lblInputHint = new Label();
            lblWakeUpCaption = new Label();
            lblSleepCaption = new Label();
            lblWorkCaption = new Label();
            lblRestCaption = new Label();
            lblWakeUpValue = new Label();
            lblSleepValue = new Label();
            lblWorkValue = new Label();
            lblRestValue = new Label();
            btnEnterData = new Button();
            btnReset = new Button();
            groupResult = new GroupBox();
            lblScoreTitle = new Label();
            lblScore = new Label();
            lblScoreUnit = new Label();
            progressIdeal = new ProgressBar();
            lblAssessment = new Label();
            lblComment = new Label();
            lblAlternativeTitle = new Label();
            lblAlternative = new Label();
            panelFooter = new Panel();
            lblMvc = new Label();
            lblLastUpdated = new Label();

            SuspendLayout();
            panelHeader.SuspendLayout();
            layoutContent.SuspendLayout();
            groupInput.SuspendLayout();
            groupResult.SuspendLayout();
            panelFooter.SuspendLayout();

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            BackColor = Theme.Background;
            ClientSize = new Size(980, 640);
            MinimumSize = new Size(820, 560);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Идеальный день";
            Font = Theme.RegularFont;

            panelHeader.BackColor = Theme.Header;
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 76;

            lblTitle.AutoSize = true;
            lblTitle.Font = Theme.TitleFont;
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(34, 18);
            lblTitle.Text = "Идеальный день";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = Theme.SmallFont;
            lblSubtitle.ForeColor = Theme.MutedText;
            lblSubtitle.Location = new Point(38, 66);
            lblSubtitle.Text = "Помощник для поиска баланса между делами и отдыхом";
            lblSubtitle.Visible = false;

            layoutContent.BackColor = Theme.Background;
            layoutContent.ColumnCount = 2;
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            layoutContent.Controls.Add(groupInput, 0, 0);
            layoutContent.Controls.Add(groupResult, 1, 0);
            layoutContent.Dock = DockStyle.Fill;
            layoutContent.Padding = new Padding(18, 16, 18, 10);
            layoutContent.RowCount = 1;
            layoutContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            groupInput.BackColor = Theme.Surface;
            groupInput.Dock = DockStyle.Fill;
            groupInput.Font = Theme.GroupFont;
            groupInput.ForeColor = Theme.MainText;
            groupInput.Margin = new Padding(6);
            groupInput.Padding = new Padding(14, 14, 14, 12);
            groupInput.Text = "Параметры дня";

            var inputLayout = new TableLayoutPanel
            {
                BackColor = Theme.Surface,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                RowCount = 7,
                Padding = new Padding(4, 8, 4, 4)
            };
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            lblInputHint.AutoSize = false;
            lblInputHint.Dock = DockStyle.Fill;
            lblInputHint.Font = Theme.SmallFont;
            lblInputHint.ForeColor = Theme.MutedText;
            lblInputHint.Text = "Сон и работа вводятся вручную." + Environment.NewLine + "Отдых рассчитывается автоматически.";
            lblInputHint.TextAlign = ContentAlignment.MiddleLeft;
            lblInputHint.Visible = false;
            inputLayout.Controls.Add(lblInputHint, 0, 0);
            inputLayout.SetColumnSpan(lblInputHint, 2);

            ConfigureCaption(lblWakeUpCaption, "Время подъёма");
            ConfigureCaption(lblSleepCaption, "Сон");
            ConfigureCaption(lblWorkCaption, "Работа / учёба");
            ConfigureCaption(lblRestCaption, "Отдых");
            ConfigureValue(lblWakeUpValue);
            ConfigureValue(lblSleepValue);
            ConfigureValue(lblWorkValue);
            ConfigureValue(lblRestValue);

            inputLayout.Controls.Add(lblWakeUpCaption, 0, 1);
            inputLayout.Controls.Add(lblWakeUpValue, 1, 1);
            inputLayout.Controls.Add(lblSleepCaption, 0, 2);
            inputLayout.Controls.Add(lblSleepValue, 1, 2);
            inputLayout.Controls.Add(lblWorkCaption, 0, 3);
            inputLayout.Controls.Add(lblWorkValue, 1, 3);
            inputLayout.Controls.Add(lblRestCaption, 0, 4);
            inputLayout.Controls.Add(lblRestValue, 1, 4);

            var inputButtons = new FlowLayoutPanel
            {
                AutoSize = false,
                BackColor = Theme.Surface,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 4, 0, 0),
                WrapContents = false
            };
            ConfigurePrimaryButton(btnEnterData, "Ввести данные");
            ConfigureSecondaryButton(btnReset, "По умолчанию");
            inputButtons.Controls.Add(btnEnterData);
            inputButtons.Controls.Add(btnReset);
            inputLayout.Controls.Add(inputButtons, 0, 6);
            inputLayout.SetColumnSpan(inputButtons, 2);
            groupInput.Controls.Add(inputLayout);

            groupResult.BackColor = Theme.Surface;
            groupResult.Dock = DockStyle.Fill;
            groupResult.Font = Theme.GroupFont;
            groupResult.ForeColor = Theme.MainText;
            groupResult.Margin = new Padding(6);
            groupResult.Padding = new Padding(14, 14, 14, 12);
            groupResult.Text = "Результат расчёта";

            var resultLayout = new TableLayoutPanel
            {
                BackColor = Theme.Surface,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 6,
                Padding = new Padding(4, 8, 4, 4)
            };
            resultLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            resultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ConfigureResultLabel(lblScoreTitle, "Индекс идеальности", Theme.MutedText, ContentAlignment.MiddleLeft);
            resultLayout.Controls.Add(lblScoreTitle, 0, 0);

            var scoreLayout = new TableLayoutPanel
            {
                BackColor = Theme.Surface,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                RowCount = 1
            };
            scoreLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            scoreLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            scoreLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            scoreLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ConfigureResultLabel(lblScore, "85", Theme.Accent, ContentAlignment.MiddleLeft);
            lblScore.Font = Theme.ScoreFont;
            ConfigureResultLabel(lblScoreUnit, "/ 100", Theme.MutedText, ContentAlignment.BottomLeft);
            lblScoreUnit.Padding = new Padding(0, 0, 0, 22);
            progressIdeal.Dock = DockStyle.Fill;
            progressIdeal.Margin = new Padding(8, 35, 8, 35);
            progressIdeal.Maximum = 100;
            progressIdeal.Style = ProgressBarStyle.Continuous;
            progressIdeal.Value = 85;
            scoreLayout.Controls.Add(lblScore, 0, 0);
            scoreLayout.Controls.Add(lblScoreUnit, 1, 0);
            scoreLayout.Controls.Add(progressIdeal, 2, 0);
            resultLayout.Controls.Add(scoreLayout, 0, 1);

            ConfigureResultLabel(lblAssessment, "Почти идеальный день", Theme.MainText, ContentAlignment.MiddleLeft);
            lblAssessment.Font = Theme.AssessmentFont;
            resultLayout.Controls.Add(lblAssessment, 0, 2);

            ConfigureResultLabel(lblComment, "Сон 08:00   Работа 08:00   Отдых 08:00", Theme.SecondaryText, ContentAlignment.MiddleLeft);
            lblComment.AutoEllipsis = true;
            lblComment.Visible = true;
            resultLayout.Controls.Add(lblComment, 0, 3);

            ConfigureResultLabel(lblAlternativeTitle, "Расписание для счастья", Theme.MainText, ContentAlignment.MiddleLeft);
            lblAlternativeTitle.Font = Theme.GroupFont;
            lblAlternativeTitle.Visible = true;
            resultLayout.Controls.Add(lblAlternativeTitle, 0, 4);

            ConfigureResultLabel(lblAlternative, "07:00 подъём" + Environment.NewLine + "09:00–17:00 работа / учёба" + Environment.NewLine + "17:00–21:00 отдых, 23:00 сон", Theme.SecondaryText, ContentAlignment.TopLeft);
            lblAlternative.AutoEllipsis = true;
            lblAlternative.Padding = new Padding(2, 4, 2, 0);
            lblAlternative.Visible = true;
            resultLayout.Controls.Add(lblAlternative, 0, 5);
            resultLayout.RowStyles[3] = new RowStyle(SizeType.Absolute, 34F);
            resultLayout.RowStyles[4] = new RowStyle(SizeType.Absolute, 32F);
            resultLayout.RowStyles[5] = new RowStyle(SizeType.Percent, 100F);
            groupResult.Controls.Add(resultLayout);

            panelFooter.BackColor = Theme.Footer;
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Height = 0;
            panelFooter.Visible = false;
            var footerLayout = new TableLayoutPanel
            {
                BackColor = Theme.Footer,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                RowCount = 1
            };
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            footerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            ConfigureResultLabel(lblMvc, "MVC: активная модель  •  Model пересчитывает результат сама", Theme.MutedText, ContentAlignment.MiddleLeft);
            ConfigureResultLabel(lblLastUpdated, "Последний расчёт: --:--:--", Theme.MutedText, ContentAlignment.MiddleRight);
            footerLayout.Controls.Add(lblMvc, 0, 0);
            footerLayout.Controls.Add(lblLastUpdated, 1, 0);
            panelFooter.Controls.Add(footerLayout);

            Controls.Add(layoutContent);
            Controls.Add(panelFooter);
            Controls.Add(panelHeader);

            panelFooter.ResumeLayout(false);
            groupResult.ResumeLayout(false);
            groupInput.ResumeLayout(false);
            layoutContent.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
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

        private static void ConfigureValue(Label label)
        {
            label.AutoSize = false;
            label.Dock = DockStyle.Fill;
            label.Font = Theme.ValueFont;
            label.ForeColor = Theme.MainText;
            label.Text = "--";
            label.TextAlign = ContentAlignment.MiddleRight;
        }

        private static void ConfigureResultLabel(Label label, string text, Color color, ContentAlignment alignment)
        {
            label.AutoSize = false;
            label.Dock = DockStyle.Fill;
            label.Font = Theme.RegularFont;
            label.ForeColor = color;
            label.Text = text;
            label.TextAlign = alignment;
        }

        private static void ConfigurePrimaryButton(Button button, string text)
        {
            button.BackColor = Theme.Accent;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = Theme.ButtonFont;
            button.ForeColor = Color.White;
            button.Margin = new Padding(0, 0, 8, 0);
            button.Size = new Size(150, 40);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }

        private static void ConfigureSecondaryButton(Button button, string text)
        {
            button.BackColor = Theme.Control;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = Theme.SmallFont;
            button.ForeColor = Theme.MainText;
            button.Margin = new Padding(0);
            button.Size = new Size(112, 40);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }
    }

    internal static class Theme
    {
        private static readonly PrivateFontCollection FontCollection = new();
        private static readonly FontFamily AppFontFamily = LoadFontFamily();

        public static readonly Color Background = Color.FromArgb(18, 24, 38);
        public static readonly Color Header = Color.FromArgb(15, 23, 42);
        public static readonly Color Surface = Color.FromArgb(27, 36, 54);
        public static readonly Color Footer = Color.FromArgb(22, 31, 49);
        public static readonly Color Control = Color.FromArgb(42, 54, 80);
        public static readonly Color Accent = Color.FromArgb(94, 129, 244);
        public static readonly Color MainText = Color.FromArgb(240, 244, 255);
        public static readonly Color SecondaryText = Color.FromArgb(187, 198, 222);
        public static readonly Color MutedText = Color.FromArgb(139, 153, 183);

        public static readonly Font RegularFont = CreateFont(10F);
        public static readonly Font SmallFont = CreateFont(9F);
        public static readonly Font GroupFont = CreateFont(11F, FontStyle.Bold);
        public static readonly Font ButtonFont = CreateFont(9.5F, FontStyle.Bold);
        public static readonly Font ValueFont = CreateFont(11F, FontStyle.Bold);
        public static readonly Font TitleFont = CreateFont(24F, FontStyle.Bold);
        public static readonly Font InputTitleFont = CreateFont(18F, FontStyle.Bold);
        public static readonly Font ScoreFont = CreateFont(38F, FontStyle.Bold);
        public static readonly Font AssessmentFont = CreateFont(15F, FontStyle.Bold);

        private static FontFamily LoadFontFamily()
        {
            var fontPath = Path.Combine(AppContext.BaseDirectory, "Montserrat-SemiBold.ttf");

            if (File.Exists(fontPath))
            {
                FontCollection.AddFontFile(fontPath);
                return FontCollection.Families[0];
            }

            return new FontFamily("Segoe UI");
        }

        private static Font CreateFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font(AppFontFamily, size, style, GraphicsUnit.Point);
        }
    }
}
