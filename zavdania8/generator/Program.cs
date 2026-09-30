using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PasswordGenerator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PasswordForm());
        }
    }

    public class PasswordForm : Form
    {
        // Елементи інтерфейсу
        private Label lblLength;
        private NumericUpDown nudLength;
        
        private GroupBox gbOptions;
        private CheckBox chkLowercase;
        private CheckBox chkUppercase;
        private CheckBox chkNumbers;
        private CheckBox chkSymbols;
        
        private Button btnGenerate;
        private TextBox txtResult;
        private Button btnCopy;

        public PasswordForm()
        {
            // Налаштування вікна
            this.Text = "Генератор паролів";
            this.Size = new Size(350, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- ДОВЖИНА ПАРОЛЯ ---
            lblLength = new Label() { Text = "Довжина пароля:", Location = new Point(20, 20), AutoSize = true };
            nudLength = new NumericUpDown() { Location = new Point(20, 45), Width = 120, Minimum = 4, Maximum = 128, Value = 12 };

            // --- ОПЦІЇ (ЧЕКБОКСИ) ---
            gbOptions = new GroupBox() { Text = "Налаштування символів", Location = new Point(20, 80), Size = new Size(290, 140) };
            
            chkLowercase = new CheckBox() { Text = "Малі літери (a-z)", Location = new Point(15, 25), AutoSize = true, Checked = true };
            chkUppercase = new CheckBox() { Text = "Великі літери (A-Z)", Location = new Point(15, 50), AutoSize = true, Checked = true };
            chkNumbers = new CheckBox() { Text = "Цифри (0-9)", Location = new Point(15, 75), AutoSize = true, Checked = true };
            chkSymbols = new CheckBox() { Text = "Спецсимволи (!@#$%)", Location = new Point(15, 100), AutoSize = true };

            gbOptions.Controls.Add(chkLowercase);
            gbOptions.Controls.Add(chkUppercase);
            gbOptions.Controls.Add(chkNumbers);
            gbOptions.Controls.Add(chkSymbols);

            // --- КНОПКА ГЕНЕРАЦІЇ ---
            btnGenerate = new Button() { Text = "Згенерувати пароль", Location = new Point(20, 230), Size = new Size(290, 40), BackColor = Color.LightSeaGreen, ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnGenerate.Click += BtnGenerate_Click;

            // --- ПОЛЕ З РЕЗУЛЬТАТОМ ---
            txtResult = new TextBox() { Location = new Point(20, 285), Size = new Size(290, 30), Font = new Font("Consolas", 14), ReadOnly = true, TextAlign = HorizontalAlignment.Center };
            
            btnCopy = new Button() { Text = "Копіювати", Location = new Point(20, 325), Size = new Size(290, 30) };
            btnCopy.Click += BtnCopy_Click;

            // Додаємо всі елементи на форму
            this.Controls.Add(lblLength);
            this.Controls.Add(nudLength);
            this.Controls.Add(gbOptions);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(txtResult);
            this.Controls.Add(btnCopy);
        }

        // Логіка генерації пароля
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            // Створюємо "базу" символів залежно від вибраних галочок
            string characterPool = "";

            if (chkLowercase.Checked) characterPool += "abcdefghijklmnopqrstuvwxyz";
            if (chkUppercase.Checked) characterPool += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (chkNumbers.Checked)   characterPool += "0123456789";
            if (chkSymbols.Checked)   characterPool += "!@#$%^&*()_+-=[]{}|;:,.<>?";

            // Перевірка: чи вибрав користувач хоча б один тип символів?
            if (string.IsNullOrEmpty(characterPool))
            {
                MessageBox.Show("Оберіть хоча б один тип символів для генерації!", "Помилка");
                return;
            }

            // Генеруємо пароль
            int passwordLength = (int)nudLength.Value;
            Random rnd = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < passwordLength; i++)
            {
                // Обираємо випадковий індекс символу з нашої "бази"
                int randomIndex = rnd.Next(0, characterPool.Length);
                // Додаємо цей символ до пароля
                password.Append(characterPool[randomIndex]);
            }

            // Виводимо результат
            txtResult.Text = password.ToString();
        }

        // Логіка кнопки "Копіювати"
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResult.Text))
            {
                Clipboard.SetText(txtResult.Text);
                MessageBox.Show("Пароль скопійовано в буфер обміну!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}