using System;
using System.Drawing;
using System.Windows.Forms;

namespace SubscriptionTracker
{
    // Головний клас для запуску
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrackerForm());
        }
    }

    // Клас нашої форми
    public class TrackerForm : Form
    {
        // Поле для зарплати
        private Label lblSalary;
        private TextBox txtSalary;

        // Елементи для додавання підписки
        private GroupBox gbAddExpense;
        private Label lblSubName;
        private TextBox txtSubName;
        private Label lblSubPrice;
        private TextBox txtSubPrice;
        private Button btnAddExpense;

        // Елементи для звіту
        private GroupBox gbSummary;
        private ListBox lstExpenses; // Список для відображення всіх доданих витрат
        private Label lblTotalExpenses;
        private Label lblRemainingSalary;

        // Змінна для зберігання загальної суми витрат
        private double totalExpenses = 0;

        public TrackerForm()
        {
            // Налаштування вікна
            this.Text = "Трекер підписок та витрат";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- БЛОК 1: Зарплата ---
            lblSalary = new Label() { Text = "Ваша зарплата (грн):", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            txtSalary = new TextBox() { Location = new Point(20, 45), Width = 340 };

            // --- БЛОК 2: Додавання витрати ---
            gbAddExpense = new GroupBox() { Text = "Додати витрату (Кава, Netflix тощо)", Location = new Point(20, 80), Size = new Size(340, 140) };
            
            lblSubName = new Label() { Text = "Назва:", Location = new Point(15, 30), AutoSize = true };
            txtSubName = new TextBox() { Location = new Point(15, 50), Width = 150 };
            
            lblSubPrice = new Label() { Text = "Ціна (грн):", Location = new Point(175, 30), AutoSize = true };
            txtSubPrice = new TextBox() { Location = new Point(175, 50), Width = 150 };
            
            btnAddExpense = new Button() { Text = "Додати до витрат", Location = new Point(15, 90), Size = new Size(310, 35), BackColor = Color.LightBlue };
            btnAddExpense.Click += BtnAddExpense_Click;

            gbAddExpense.Controls.Add(lblSubName);
            gbAddExpense.Controls.Add(txtSubName);
            gbAddExpense.Controls.Add(lblSubPrice);
            gbAddExpense.Controls.Add(txtSubPrice);
            gbAddExpense.Controls.Add(btnAddExpense);

            // --- БЛОК 3: Статистика ---
            gbSummary = new GroupBox() { Text = "Ваш звіт", Location = new Point(20, 230), Size = new Size(340, 210) };
            
            lstExpenses = new ListBox() { Location = new Point(15, 25), Size = new Size(310, 100) };
            
            lblTotalExpenses = new Label() { Text = "Всього витрат: 0 грн", Location = new Point(15, 140), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold) };
            lblRemainingSalary = new Label() { Text = "Залишок: 0 грн", Location = new Point(15, 170), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };

            gbSummary.Controls.Add(lstExpenses);
            gbSummary.Controls.Add(lblTotalExpenses);
            gbSummary.Controls.Add(lblRemainingSalary);

            // Додаємо всі блоки на форму
            this.Controls.Add(lblSalary);
            this.Controls.Add(txtSalary);
            this.Controls.Add(gbAddExpense);
            this.Controls.Add(gbSummary);
        }

        // Логіка при натисканні на кнопку "Додати"
        private void BtnAddExpense_Click(object? sender, EventArgs e)
        {
            try
            {
                // Зчитуємо зарплату та перевіряємо, чи введено число
                if (!double.TryParse(txtSalary.Text, out double salary))
                {
                    MessageBox.Show("Будь ласка, введіть коректну суму зарплати.", "Помилка");
                    return;
                }

                // Зчитуємо ціну витрати
                if (!double.TryParse(txtSubPrice.Text, out double price))
                {
                    MessageBox.Show("Будь ласка, введіть коректну ціну витрати.", "Помилка");
                    return;
                }

                string name = txtSubName.Text;
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Будь ласка, введіть назву витрати.", "Помилка");
                    return;
                }

                // Додаємо витрату до загальної суми
                totalExpenses += price;

                // Додаємо запис у візуальний список
                lstExpenses.Items.Add($"{name} : {price} грн");

                // Рахуємо залишок
                double remaining = salary - totalExpenses;

                // Оновлюємо текст на екрані
                lblTotalExpenses.Text = $"Всього витрат: {totalExpenses} грн";
                lblRemainingSalary.Text = $"Залишок: {remaining} грн";

                // Робимо залишок червоним, якщо ми пішли в мінус
                if (remaining < 0)
                {
                    lblRemainingSalary.ForeColor = Color.Red;
                }
                else
                {
                    lblRemainingSalary.ForeColor = Color.DarkGreen;
                }

                // Очищаємо поля для наступного вводу
                txtSubName.Clear();
                txtSubPrice.Clear();
                
                // Ставимо курсор назад у поле назви
                txtSubName.Focus(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка: " + ex.Message, "Помилка");
            }
        }
    }
}