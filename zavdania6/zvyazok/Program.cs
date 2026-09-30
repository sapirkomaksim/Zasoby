using System;
using System.Drawing;
using System.Windows.Forms;

namespace TariffSelector
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TariffForm());
        }
    }

    public class TariffForm : Form
    {
        // Елементи для вибору (NumericUpDown)
        private Label lblInternet;
        private NumericUpDown nudInternet;
        
        private Label lblMinutes;
        private NumericUpDown nudMinutes;
        
        private Label lblSMS;
        private NumericUpDown nudSMS;
        
        private Button btnFindTariff;
        
        // Елементи для виводу результату
        private GroupBox gbResult;
        private Label lblTariffName;
        private Label lblPrice;
        private Label lblDescription;

        public TariffForm()
        {
            // Налаштування вікна
            this.Text = "Підбір тарифу мобільного зв'язку";
            this.Size = new Size(380, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- БЛОК ВВОДУ ---
            lblInternet = new Label() { Text = "Скільки Інтернету вам потрібно (ГБ):", Location = new Point(20, 20), AutoSize = true };
            nudInternet = new NumericUpDown() { Location = new Point(20, 45), Width = 320, Maximum = 1000, Minimum = 0, Value = 10 };

            lblMinutes = new Label() { Text = "Скільки хвилин на інші мережі:", Location = new Point(20, 85), AutoSize = true };
            nudMinutes = new NumericUpDown() { Location = new Point(20, 110), Width = 320, Maximum = 5000, Minimum = 0, Value = 150 };

            lblSMS = new Label() { Text = "Скільки SMS повідомлень:", Location = new Point(20, 150), AutoSize = true };
            nudSMS = new NumericUpDown() { Location = new Point(20, 175), Width = 320, Maximum = 1000, Minimum = 0, Value = 0 };

            btnFindTariff = new Button() { Text = "Підібрати тариф", Location = new Point(20, 220), Size = new Size(320, 40), BackColor = Color.LightSkyBlue, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnFindTariff.Click += BtnFindTariff_Click;

            // --- БЛОК РЕЗУЛЬТАТІВ ---
            gbResult = new GroupBox() { Text = "Рекомендований тариф", Location = new Point(20, 280), Size = new Size(320, 110) };
            
            lblTariffName = new Label() { Text = "Назва тарифу", Location = new Point(15, 25), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.DarkBlue };
            lblPrice = new Label() { Text = "Ціна: 0 грн/міс", Location = new Point(15, 50), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold), ForeColor = Color.DarkRed };
            lblDescription = new Label() { Text = "Опис тарифу...", Location = new Point(15, 75), Size = new Size(290, 30), ForeColor = Color.DimGray };

            gbResult.Controls.Add(lblTariffName);
            gbResult.Controls.Add(lblPrice);
            gbResult.Controls.Add(lblDescription);

            // Додаємо все на форму
            this.Controls.Add(lblInternet);
            this.Controls.Add(nudInternet);
            this.Controls.Add(lblMinutes);
            this.Controls.Add(nudMinutes);
            this.Controls.Add(lblSMS);
            this.Controls.Add(nudSMS);
            this.Controls.Add(btnFindTariff);
            this.Controls.Add(gbResult);
        }

        private void BtnFindTariff_Click(object sender, EventArgs e)
        {
            // Зчитуємо значення з NumericUpDown
            // Оскільки Value має тип decimal, перетворюємо (cast) його у ціле число (int)
            int gb = (int)nudInternet.Value;
            int mins = (int)nudMinutes.Value;
            int sms = (int)nudSMS.Value;

            string tariffName = "";
            int price = 0;
            string description = "";

            // Логіка підбору (if / else if / else)
            if (gb <= 5 && mins <= 100 && sms <= 50)
            {
                tariffName = "Тариф «Економ»";
                price = 100;
                description = "5 ГБ, 100 хв, 50 SMS. Ідеально для базових потреб.";
            }
            else if (gb <= 20 && mins <= 300 && sms <= 200)
            {
                tariffName = "Тариф «Оптимальний»";
                price = 200;
                description = "20 ГБ, 300 хв, 200 SMS. Золота середина для смартфона.";
            }
            else
            {
                tariffName = "Тариф «Максимальний» (Безлім)";
                price = 350;
                description = "Безлімітний інтернет, 1000 хв, 500 SMS. Ні в чому собі не відмовляйте!";
            }

            // Виводимо результати на екран
            lblTariffName.Text = tariffName;
            lblPrice.Text = $"Ціна: {price} грн/міс";
            lblDescription.Text = description;
        }
    }
}