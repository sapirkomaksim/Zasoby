using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizApplication
{
    // Головний клас для запуску
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    // Клас нашої форми з анкетою
    public class MainForm : Form
    {
        // Оголошуємо групи для запитань
        private GroupBox gbQuestion1;
        private GroupBox gbQuestion2;
        private GroupBox gbQuestion3;

        // Варіанти відповідей (RadioButton - для однієї правильної)
        private RadioButton rbQ1_A, rbQ1_B, rbQ1_C;
        private RadioButton rbQ2_A, rbQ2_B, rbQ2_C;

        // Варіанти відповідей (CheckBox - для кількох правильних)
        private CheckBox cbQ3_A, cbQ3_B, cbQ3_C;

        // Кнопка для перевірки
        private Button btnFinish;

        public MainForm()
        {
            // 1. Налаштування вікна
            this.Text = "Тестова анкета";
            this.Size = new Size(400, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ==========================================
            // ЗАПИТАННЯ 1 (Один варіант)
            // ==========================================
            gbQuestion1 = new GroupBox() { Text = "1. Яка мова використовується для Windows Forms?", Location = new Point(20, 20), Size = new Size(340, 100) };
            
            rbQ1_A = new RadioButton() { Text = "C#", Location = new Point(20, 30), AutoSize = true }; // Правильна
            rbQ1_B = new RadioButton() { Text = "Python", Location = new Point(20, 50), AutoSize = true };
            rbQ1_C = new RadioButton() { Text = "HTML", Location = new Point(20, 70), AutoSize = true };
            
            // Додаємо кнопки саме в GroupBox 1, а не на форму
            gbQuestion1.Controls.Add(rbQ1_A);
            gbQuestion1.Controls.Add(rbQ1_B);
            gbQuestion1.Controls.Add(rbQ1_C);

            // ==========================================
            // ЗАПИТАННЯ 2 (Один варіант)
            // ==========================================
            gbQuestion2 = new GroupBox() { Text = "2. Який тип даних використовується для цілих чисел?", Location = new Point(20, 130), Size = new Size(340, 100) };
            
            rbQ2_A = new RadioButton() { Text = "string", Location = new Point(20, 30), AutoSize = true };
            rbQ2_B = new RadioButton() { Text = "double", Location = new Point(20, 50), AutoSize = true };
            rbQ2_C = new RadioButton() { Text = "int", Location = new Point(20, 70), AutoSize = true }; // Правильна
            
            gbQuestion2.Controls.Add(rbQ2_A);
            gbQuestion2.Controls.Add(rbQ2_B);
            gbQuestion2.Controls.Add(rbQ2_C);

            // ==========================================
            // ЗАПИТАННЯ 3 (Кілька варіантів - CheckBox)
            // ==========================================
            gbQuestion3 = new GroupBox() { Text = "3. Оберіть мови програмування (кілька варіантів):", Location = new Point(20, 240), Size = new Size(340, 110) };
            
            cbQ3_A = new CheckBox() { Text = "C++", Location = new Point(20, 30), AutoSize = true }; // Правильна
            cbQ3_B = new CheckBox() { Text = "Photoshop", Location = new Point(20, 50), AutoSize = true };
            cbQ3_C = new CheckBox() { Text = "Java", Location = new Point(20, 70), AutoSize = true }; // Правильна
            
            gbQuestion3.Controls.Add(cbQ3_A);
            gbQuestion3.Controls.Add(cbQ3_B);
            gbQuestion3.Controls.Add(cbQ3_C);

            // ==========================================
            // КНОПКА ЗАВЕРШЕННЯ
            // ==========================================
            btnFinish = new Button() { Text = "Завершити тест і дізнатися результат", Location = new Point(20, 370), Size = new Size(340, 45), Font = new Font("Arial", 10, FontStyle.Bold) };
            btnFinish.Click += BtnFinish_Click;

            // Додаємо всі створені Групи та Кнопку на саму форму
            this.Controls.Add(gbQuestion1);
            this.Controls.Add(gbQuestion2);
            this.Controls.Add(gbQuestion3);
            this.Controls.Add(btnFinish);
        }

        // Логіка підрахунку балів
        private void BtnFinish_Click(object? sender, EventArgs e)
        {
            int score = 0; // Початковий бал

            // Перевіряємо перше запитання (правильна C# - це rbQ1_A)
            if (rbQ1_A.Checked) 
            {
                score++;
            }

            // Перевіряємо друге запитання (правильна int - це rbQ2_C)
            if (rbQ2_C.Checked) 
            {
                score++;
            }

            // Перевіряємо третє запитання (правильно C++ та Java, і НЕ вибрано Photoshop)
            if (cbQ3_A.Checked == true && cbQ3_C.Checked == true && cbQ3_B.Checked == false)
            {
                score++;
            }

            // Формуємо повідомлення з оцінкою
            string feedback = "";
            if (score == 3) feedback = "Відмінно! Ви знавець!";
            else if (score == 2) feedback = "Добре, але є куди рости.";
            else feedback = "Варто ще трохи повчитися.";

            // Виводимо результат на екран у спливаючому віконці
            MessageBox.Show($"Ваш результат: {score} з 3 балів.\n\n{feedback}", "Результат тесту", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}