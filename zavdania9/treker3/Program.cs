using System;
using System.Drawing;
using System.Windows.Forms;

namespace SleepTracker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SleepForm());
        }
    }

    public class SleepForm : Form
    {
        // Елементи інтерфейсу
        private Label lblAge;
        private ComboBox cbAge;

        private Label lblBedtime;
        private DateTimePicker dtpBedtime;

        private Label lblWakeup;
        private DateTimePicker dtpWakeup;

        private Button btnCalculate;

        // Елементи результату
        private GroupBox gbResult;
        private Label lblDuration;
        private Label lblStatus;
        private Label lblAdvice;

        public SleepForm()
        {
            // Налаштування вікна
            this.Text = "Трекер сну";
            this.Size = new Size(380, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // --- КАТЕГОРІЯ ВІКУ ---
            lblAge = new Label() { Text = "Оберіть вашу вікову категорію:", Location = new Point(20, 20), AutoSize = true };
            cbAge = new ComboBox() { Location = new Point(20, 40), Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };
            cbAge.Items.AddRange(new string[] 
            { 
                "Діти (6-13 років)", 
                "Підлітки (14-17 років)", 
                "Дорослі (18-64 роки)", 
                "Літні люди (65+ років)" 
            });
            cbAge.SelectedIndex = 2; // За замовчуванням "Дорослі"

            // --- ЧАС СНУ ---
            lblBedtime = new Label() { Text = "О котрій ви лягли спати?", Location = new Point(20, 80), AutoSize = true };
            dtpBedtime = new DateTimePicker() 
            { 
                Location = new Point(20, 100), 
                Width = 150, 
                Format = DateTimePickerFormat.Custom, 
                CustomFormat = "HH:mm", 
                ShowUpDown = true, // Вмикає стрілочки замість календаря
                Font = new Font("Arial", 12)
            };
            // Встановлюємо 23:00 за замовчуванням
            dtpBedtime.Value = DateTime.Today.AddHours(23);

            // --- ЧАС ПРОБУДЖЕННЯ ---
            lblWakeup = new Label() { Text = "О котрій ви прокинулися?", Location = new Point(20, 150), AutoSize = true };
            dtpWakeup = new DateTimePicker() 
            { 
                Location = new Point(20, 170), 
                Width = 150, 
                Format = DateTimePickerFormat.Custom, 
                CustomFormat = "HH:mm", 
                ShowUpDown = true, 
                Font = new Font("Arial", 12)
            };
            // Встановлюємо 07:00 за замовчуванням
            dtpWakeup.Value = DateTime.Today.AddHours(7);

            // --- КНОПКА РОЗРАХУНКУ ---
            btnCalculate = new Button() 
            { 
                Text = "Оцінити сон", 
                Location = new Point(20, 220), 
                Size = new Size(320, 45), 
                BackColor = Color.SlateBlue, 
                ForeColor = Color.White, 
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCalculate.Click += BtnCalculate_Click;

            // --- БЛОК РЕЗУЛЬТАТІВ ---
            gbResult = new GroupBox() { Text = "Результат", Location = new Point(20, 280), Size = new Size(320, 130), BackColor = Color.White };
            
            lblDuration = new Label() { Text = "Тривалість сну: 0 год 0 хв", Location = new Point(15, 30), AutoSize = true, Font = new Font("Arial", 11, FontStyle.Bold) };
            lblStatus = new Label() { Text = "Статус", Location = new Point(15, 60), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };
            lblAdvice = new Label() { Text = "...", Location = new Point(15, 90), Size = new Size(290, 40), Font = new Font("Arial", 9, FontStyle.Italic), ForeColor = Color.DimGray };

            gbResult.Controls.Add(lblDuration);
            gbResult.Controls.Add(lblStatus);
            gbResult.Controls.Add(lblAdvice);

            // Додаємо все на форму
            this.Controls.Add(lblAge);
            this.Controls.Add(cbAge);
            this.Controls.Add(lblBedtime);
            this.Controls.Add(dtpBedtime);
            this.Controls.Add(lblWakeup);
            this.Controls.Add(dtpWakeup);
            this.Controls.Add(btnCalculate);
            this.Controls.Add(gbResult);
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            // Отримуємо тільки час (без дати) з DateTimePicker
            TimeSpan bedTime = dtpBedtime.Value.TimeOfDay;
            TimeSpan wakeTime = dtpWakeup.Value.TimeOfDay;

            // Рахуємо тривалість сну
            TimeSpan duration = wakeTime - bedTime;

            // Якщо час пробудження менший за час відходу до сну (наприклад, ліг о 23:00, встав о 07:00),
            // це означає, що пройшла ніч, тому додаємо 24 години (1 день).
            if (duration.TotalHours < 0)
            {
                duration = duration.Add(TimeSpan.FromDays(1));
            }

            double totalHours = duration.TotalHours;

            // Виводимо тривалість на екран
            lblDuration.Text = $"Тривалість сну: {duration.Hours} год {duration.Minutes} хв";

            // Визначаємо норму сну залежно від віку
            double minNorm = 0;
            double maxNorm = 0;

            if (cbAge.SelectedIndex == 0) // Діти
            {
                minNorm = 9; maxNorm = 11;
            }
            else if (cbAge.SelectedIndex == 1) // Підлітки
            {
                minNorm = 8; maxNorm = 10;
            }
            else if (cbAge.SelectedIndex == 2) // Дорослі
            {
                minNorm = 7; maxNorm = 9;
            }
            else // Літні люди
            {
                minNorm = 7; maxNorm = 8;
            }

            // Порівнюємо результат з нормою
            if (totalHours < minNorm)
            {
                lblStatus.Text = "Статус: Недосип 😴";
                lblStatus.ForeColor = Color.Red;
                lblAdvice.Text = $"Ваша норма становить {minNorm}-{maxNorm} годин. Постарайтеся лягати раніше, недосип шкодить здоров'ю.";
            }
            else if (totalHours > maxNorm)
            {
                lblStatus.Text = "Статус: Пересип 🦥";
                lblStatus.ForeColor = Color.Orange;
                lblAdvice.Text = $"Ваша норма становить {minNorm}-{maxNorm} годин. Занадто довгий сон може викликати відчуття млявості.";
            }
            else
            {
                lblStatus.Text = "Статус: Норма! ✨";
                lblStatus.ForeColor = Color.MediumSeaGreen;
                lblAdvice.Text = $"Чудово! Ви спите ідеальні {minNorm}-{maxNorm} годин. Так тримати!";
            }
        }
    }
}