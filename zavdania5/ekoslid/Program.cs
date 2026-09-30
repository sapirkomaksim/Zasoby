using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoTracker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EcoForm());
        }
    }

    public class EcoForm : Form
    {
        // Елементи інтерфейсу
        private Label lblCarTrips;
        private TextBox txtCarTrips;
        
        private Label lblElectricity;
        private TextBox txtElectricity;
        
        private Button btnCalculate;
        
        private GroupBox gbResult;
        private Label lblTotalCO2;
        private Label lblAdvice;

        // Приблизні коефіцієнти викидів (у кг CO2)
        private readonly double CO2_PER_TRIP = 2.5; // кг CO2 за одну середню поїздку
        private readonly double CO2_PER_HOUR_ELEC = 0.4; // кг CO2 за 1 годину роботи техніки (1 кВт·год)

        public EcoForm()
        {
            // Налаштування вікна
            this.Text = "Калькулятор екосліду (CO₂)";
            this.Size = new Size(380, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // --- БЛОК ВВОДУ ---
            lblCarTrips = new Label() { Text = "Скільки поїздок на авто ви робите за тиждень?", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 9) };
            txtCarTrips = new TextBox() { Location = new Point(20, 45), Width = 320 };

            lblElectricity = new Label() { Text = "Скільки годин на день активно працює техніка\n(ПК, ТВ, пральна машина тощо)?", Location = new Point(20, 85), AutoSize = true, Font = new Font("Arial", 9) };
            txtElectricity = new TextBox() { Location = new Point(20, 125), Width = 320 };

            btnCalculate = new Button() { Text = "Оцінити мій екослід", Location = new Point(20, 170), Size = new Size(320, 45), BackColor = Color.MediumSeaGreen, ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnCalculate.Click += BtnCalculate_Click;

            // --- БЛОК РЕЗУЛЬТАТІВ ---
            gbResult = new GroupBox() { Text = "Ваш результат за МІСЯЦЬ", Location = new Point(20, 230), Size = new Size(320, 130), BackColor = Color.White };
            
            lblTotalCO2 = new Label() { Text = "Викиди CO₂: 0 кг", Location = new Point(15, 30), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };
            lblAdvice = new Label() { Text = "Тут буде порада щодо вашого стилю життя.", Location = new Point(15, 65), Size = new Size(290, 50), Font = new Font("Arial", 9, FontStyle.Italic), ForeColor = Color.DimGray };

            gbResult.Controls.Add(lblTotalCO2);
            gbResult.Controls.Add(lblAdvice);

            // Надійне додавання елементів на форму
            this.Controls.Add(lblCarTrips);
            this.Controls.Add(txtCarTrips);
            this.Controls.Add(lblElectricity);
            this.Controls.Add(txtElectricity);
            this.Controls.Add(btnCalculate);
            this.Controls.Add(gbResult);
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // Перевірка вводу
                if (!double.TryParse(txtCarTrips.Text, out double tripsPerWeek) || tripsPerWeek < 0)
                {
                    MessageBox.Show("Введіть коректну кількість поїздок на авто (число).", "Помилка");
                    return;
                }

                if (!double.TryParse(txtElectricity.Text, out double hoursPerDay) || hoursPerDay < 0)
                {
                    MessageBox.Show("Введіть коректну кількість годин роботи техніки (число).", "Помилка");
                    return;
                }

                // Математика: рахуємо за місяць (приблизно 4 тижні та 30 днів)
                double tripsPerMonth = tripsPerWeek * 4;
                double hoursPerMonth = hoursPerDay * 30;

                double carEmissions = tripsPerMonth * CO2_PER_TRIP;
                double elecEmissions = hoursPerMonth * CO2_PER_HOUR_ELEC;
                
                double totalEmissions = carEmissions + elecEmissions;

                // Вивід результату
                lblTotalCO2.Text = $"Викиди CO₂: {Math.Round(totalEmissions, 1)} кг";

                // Логіка порад
                if (totalEmissions < 50)
                {
                    lblTotalCO2.ForeColor = Color.DarkGreen;
                    lblAdvice.Text = "Чудово! Ваш екослід мінімальний. Ви справжній друг природи 🌳";
                }
                else if (totalEmissions <= 150)
                {
                    lblTotalCO2.ForeColor = Color.DarkOrange;
                    lblAdvice.Text = "Середній рівень. Можна спробувати частіше ходити пішки або вимикати прилади з розеток на ніч 🚶";
                }
                else
                {
                    lblTotalCO2.ForeColor = Color.Red;
                    lblAdvice.Text = "Високий рівень викидів! Варто подумати про громадський транспорт та енергозберігаючі лампи 🌍";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка: " + ex.Message, "Помилка");
            }
        }
    }
}