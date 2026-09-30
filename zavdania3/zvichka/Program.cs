using System;
using System.Drawing;
using System.Windows.Forms;

namespace HabitCalculator
{
    // Головний клас для запуску
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HabitForm());
        }
    }

    // Клас нашої форми
    public class HabitForm : Form
    {
        // Елементи вводу
        private Label lblHabitName;
        private TextBox txtHabitName;
        private Label lblDailyCost;
        private TextBox txtDailyCost;
        private Button btnCalculate;

        // Елементи виводу результатів
        private GroupBox gbResults;
        private Label lblMonth;
        private Label lblYear;
        private Label lblFiveYears;
        private Label lblAlternativeTitle;
        private Label lblAlternativeText;

        public HabitForm()
        {
            // 1. Налаштування вікна
            this.Text = "Калькулятор звичок";
            this.Size = new Size(420, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- БЛОК ВВОДУ ---
            lblHabitName = new Label() { Text = "Назва звички (кава, таксі, тощо):", Location = new Point(20, 20), AutoSize = true };
            txtHabitName = new TextBox() { Location = new Point(20, 40), Width = 360 };

            lblDailyCost = new Label() { Text = "Скільки витрачаєте на це в день (грн):", Location = new Point(20, 80), AutoSize = true };
            txtDailyCost = new TextBox() { Location = new Point(20, 100), Width = 360 };

            btnCalculate = new Button() { Text = "Порахувати витрати", Location = new Point(20, 140), Size = new Size(360, 40), BackColor = Color.LightCoral, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnCalculate.Click += BtnCalculate_Click;

            // --- БЛОК РЕЗУЛЬТАТІВ ---
            gbResults = new GroupBox() { Text = "Ваші витрати", Location = new Point(20, 200), Size = new Size(360, 230) };
            
            lblMonth = new Label() { Text = "За місяць (30 днів): 0 грн", Location = new Point(15, 30), AutoSize = true, Font = new Font("Arial", 10) };
            lblYear = new Label() { Text = "За рік (365 днів): 0 грн", Location = new Point(15, 60), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            lblFiveYears = new Label() { Text = "За 5 років: 0 грн", Location = new Point(15, 90), AutoSize = true, Font = new Font("Arial", 10) };
            
            lblAlternativeTitle = new Label() { Text = "За річну суму можна було б купити:", Location = new Point(15, 140), AutoSize = true, ForeColor = Color.Gray };
            lblAlternativeText = new Label() { Text = "...", Location = new Point(15, 165), Size = new Size(330, 50), Font = new Font("Arial", 10, FontStyle.Italic), ForeColor = Color.DarkBlue };

            // Додаємо елементи в GroupBox
            gbResults.Controls.Add(lblMonth);
            gbResults.Controls.Add(lblYear);
            gbResults.Controls.Add(lblFiveYears);
            gbResults.Controls.Add(lblAlternativeTitle);
            gbResults.Controls.Add(lblAlternativeText);

            // Додаємо все на форму
            this.Controls.Add(lblHabitName);
            this.Controls.Add(txtHabitName);
            this.Controls.Add(lblDailyCost);
            this.Controls.Add(txtDailyCost);
            this.Controls.Add(btnCalculate);
            this.Controls.Add(gbResults);
        }

        // Логіка підрахунку
        private void BtnCalculate_Click(object? sender, EventArgs e)
        {
            try
            {
                // Перевіряємо ввід на правильність
                if (!double.TryParse(txtDailyCost.Text, out double dailyCost))
                {
                    MessageBox.Show("Введіть коректну суму витрат цифрами (наприклад: 50 або 75,50).", "Помилка");
                    return;
                }

                // Математика
                double costMonth = dailyCost * 30;
                double costYear = dailyCost * 365;
                double cost5Years = dailyCost * 1825;

                // Виведення сум
                lblMonth.Text = $"За місяць (30 днів): {costMonth} грн";
                lblYear.Text = $"За рік (365 днів): {costYear} грн";
                lblFiveYears.Text = $"За 5 років: {cost5Years} грн";

                // Логіка підбору "Що можна купити" (орієнтуємось на річну суму)
                string alternative = "";
                
                if (costYear == 0)
                {
                    alternative = "Ви нічого не витрачаєте! Так тримати!";
                }
                else if (costYear < 3000)
                {
                    alternative = "Кілька хороших книг, похід у ресторан або підписку на Netflix на рік.";
                }
                else if (costYear < 10000)
                {
                    alternative = "Нові смарт-годинник, бездротові навушники або гарне взуття.";
                }
                else if (costYear < 25000)
                {
                    alternative = "Бюджетний смартфон, крутий велосипед або електросамокат.";
                }
                else if (costYear < 50000)
                {
                    alternative = "Флагманський телефон (iPhone/Samsung), ігрову консоль PS5 або крутий телевізор.";
                }
                else if (costYear < 100000)
                {
                    alternative = "Потужний ігровий ноутбук або повноцінну відпустку на двох на морі!";
                }
                else
                {
                    alternative = "Вживаний автомобіль, топовий комп'ютер або перший внесок за житло!";
                }

                // Виводимо підібрану альтернативу
                lblAlternativeText.Text = alternative;
            }
            catch (Exception)
            {
                MessageBox.Show("Сталася непередбачена помилка.", "Помилка");
            }
        }
    }
}