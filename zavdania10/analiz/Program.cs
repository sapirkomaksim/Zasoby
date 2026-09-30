using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalorieTracker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CalorieTrackerForm());
        }
    }

    public class CalorieTrackerForm : Form
    {
        // Змінна для збереження сумарних калорій
        private int totalCalories = 0;

        // Елементи інтерфейсу
        private Label lblGoal;
        private NumericUpDown nudGoal;

        private GroupBox gbAddMeal;
        private Label lblProductName;
        private TextBox txtProductName;
        private Label lblCalories;
        private NumericUpDown nudCalories;
        private Button btnAdd;

        private GroupBox gbSummary;
        private ListBox lstMeals;
        private Label lblTotal;
        private ProgressBar pbProgress;
        private Label lblStatus;

        public CalorieTrackerForm()
        {
            // Налаштування вікна
            this.Text = "Аналізатор здорового харчування";
            this.Size = new Size(420, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // --- ДЕННА НОРМА ---
            lblGoal = new Label() { Text = "Ваша денна норма калорій (ккал):", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            nudGoal = new NumericUpDown() 
            { 
                Location = new Point(280, 18), 
                Width = 100, 
                Minimum = 500, 
                Maximum = 10000, 
                Value = 2000, // За замовчуванням 2000 ккал
                Font = new Font("Arial", 10)
            };
            // Якщо норма змінюється, відразу перераховуємо прогрес
            nudGoal.ValueChanged += UpdateProgress;

            // --- БЛОК ДОДАВАННЯ ПРИЙОМУ ЇЖІ ---
            gbAddMeal = new GroupBox() { Text = "Додати прийом їжі", Location = new Point(20, 60), Size = new Size(360, 140) };
            
            lblProductName = new Label() { Text = "Що ви з'їли?", Location = new Point(15, 30), AutoSize = true };
            txtProductName = new TextBox() { Location = new Point(15, 50), Width = 200 };
            
            lblCalories = new Label() { Text = "Калорії (ккал):", Location = new Point(230, 30), AutoSize = true };
            nudCalories = new NumericUpDown() { Location = new Point(230, 50), Width = 115, Minimum = 1, Maximum = 5000, Value = 100 };

            btnAdd = new Button() 
            { 
                Text = "Додати у список", 
                Location = new Point(15, 90), 
                Size = new Size(330, 35), 
                BackColor = Color.MediumSeaGreen, 
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += BtnAdd_Click;

            gbAddMeal.Controls.Add(lblProductName);
            gbAddMeal.Controls.Add(txtProductName);
            gbAddMeal.Controls.Add(lblCalories);
            gbAddMeal.Controls.Add(nudCalories);
            gbAddMeal.Controls.Add(btnAdd);

            // --- БЛОК СТАТИСТИКИ (СПИСОК ТА ПРОГРЕС-БАР) ---
            gbSummary = new GroupBox() { Text = "Ваш прогрес за день", Location = new Point(20, 220), Size = new Size(360, 300) };
            
            lstMeals = new ListBox() { Location = new Point(15, 30), Size = new Size(330, 150) };
            
            lblTotal = new Label() { Text = "Спожито: 0 / 2000 ккал", Location = new Point(15, 195), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            
            // Смуга прогресу
            pbProgress = new ProgressBar() { Location = new Point(15, 220), Size = new Size(330, 25), Style = ProgressBarStyle.Continuous };
            
            lblStatus = new Label() { Text = "Можна ще їсти!", Location = new Point(15, 255), AutoSize = true, ForeColor = Color.DarkGreen, Font = new Font("Arial", 10, FontStyle.Italic) };

            gbSummary.Controls.Add(lstMeals);
            gbSummary.Controls.Add(lblTotal);
            gbSummary.Controls.Add(pbProgress);
            gbSummary.Controls.Add(lblStatus);

            // Додаємо всі елементи на головну форму
            this.Controls.Add(lblGoal);
            this.Controls.Add(nudGoal);
            this.Controls.Add(gbAddMeal);
            this.Controls.Add(gbSummary);

            // Оновлюємо прогрес при запуску (щоб налаштувати максимум для ProgressBar)
            UpdateProgress(null, null);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string product = txtProductName.Text.Trim();
            int calories = (int)nudCalories.Value;

            if (string.IsNullOrEmpty(product))
            {
                MessageBox.Show("Будь ласка, введіть назву продукту!", "Увага");
                return;
            }

            // Додаємо запис у список
            lstMeals.Items.Add($"{product} — {calories} ккал");
            
            // Збільшуємо загальну кількість калорій
            totalCalories += calories;

            // Очищаємо поле вводу для наступного продукту
            txtProductName.Clear();
            txtProductName.Focus();

            // Оновлюємо смугу прогресу
            UpdateProgress(null, null);
        }

        // Метод, який перераховує візуальний прогрес
        private void UpdateProgress(object sender, EventArgs e)
        {
            int goal = (int)nudGoal.Value;

            // Оновлюємо текст
            lblTotal.Text = $"Спожито: {totalCalories} / {goal} ккал";

            // Налаштовуємо ProgressBar
            pbProgress.Maximum = goal; // Максимум смуги дорівнює нашій нормі

            // Якщо ми з'їли менше або рівно нормі
            if (totalCalories <= goal)
            {
                pbProgress.Value = totalCalories;
                lblStatus.Text = $"У вас в запасі ще {goal - totalCalories} ккал.";
                lblStatus.ForeColor = Color.DarkGreen;
            }
            // Якщо переїли (перевищили норму)
            else
            {
                // Ставимо смугу на 100% (її максимум), щоб програма не видала помилку
                pbProgress.Value = goal; 
                lblStatus.Text = $"Обережно! Ви перевищили норму на {totalCalories - goal} ккал!";
                lblStatus.ForeColor = Color.Red;
            }
        }
    }
}