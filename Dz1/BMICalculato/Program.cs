using System;
using System.Drawing;
using System.Windows.Forms;

namespace BMICalculator
{
    // Головний клас, який запускає програму
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    // Клас нашої форми (вікна)
    public class Form1 : Form
    {
        private Label lblWeight;
        private TextBox txtWeight;
        private Label lblHeight;
        private TextBox txtHeight;
        private Button btnCalculate;
        private Label lblResult;

        public Form1()
        {
            // Налаштовуємо вікно
            this.Text = "Калькулятор ІМТ";
            this.Size = new Size(300, 350);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Створюємо елементи
            lblWeight = new Label() { Text = "Вага (кг):", Location = new Point(20, 20), AutoSize = true };
            txtWeight = new TextBox() { Location = new Point(20, 40), Width = 240 };
            
            lblHeight = new Label() { Text = "Зріст (см):", Location = new Point(20, 80), AutoSize = true };
            txtHeight = new TextBox() { Location = new Point(20, 100), Width = 240 };
            
            btnCalculate = new Button() { Text = "Обчислити ІМТ", Location = new Point(20, 150), Width = 240, Height = 40 };
            btnCalculate.Click += BtnCalculate_Click; // Прив'язуємо клік
            
            lblResult = new Label() { Text = "Результат буде тут...", Location = new Point(20, 210), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };

            // Додаємо елементи на форму
            this.Controls.Add(lblWeight);
            this.Controls.Add(txtWeight);
            this.Controls.Add(lblHeight);
            this.Controls.Add(txtHeight);
            this.Controls.Add(btnCalculate);
            this.Controls.Add(lblResult);
        }

        // Логіка обчислення
        private void BtnCalculate_Click(object? sender, EventArgs e)
        {
            try
            {
                double weight = Convert.ToDouble(txtWeight.Text);
                double heightCm = Convert.ToDouble(txtHeight.Text);
                double heightM = heightCm / 100.0;
                
                double bmi = weight / (heightM * heightM);
                
                string category = "";
                if (bmi < 18.5)
                {
                    category = "Недостатня вага";
                    lblResult.ForeColor = Color.Blue;
                }
                else if (bmi >= 18.5 && bmi <= 24.9)
                {
                    category = "Норма";
                    lblResult.ForeColor = Color.DarkGreen;
                }
                else if (bmi >= 25.0 && bmi <= 29.9)
                {
                    category = "Надлишкова вага";
                    lblResult.ForeColor = Color.Orange;
                }
                else
                {
                    category = "Ожиріння";
                    lblResult.ForeColor = Color.Red;
                }
                
                lblResult.Text = $"Ваш ІМТ: {Math.Round(bmi, 1)}\nОцінка: {category}";
            }
            catch (Exception)
            {
                MessageBox.Show("Будь ласка, введіть числові значення (наприклад, 70 та 175).", "Помилка вводу");
            }
        }
    }
}