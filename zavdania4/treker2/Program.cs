using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FinanceTracker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Запускаємо саме FinanceForm!
            Application.Run(new FinanceForm());
        }
    }

    public class Transaction
    {
        public string Type { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
    }

    public class FinanceForm : Form
    {
        private List<Transaction> transactions = new List<Transaction>();
        private readonly string filePath = "transactions.csv";

        private Label lblType, lblCategory, lblAmount, lblCurrency, lblFilter;
        private ComboBox cbType, cbCategory, cbCurrency, cbFilter;
        private TextBox txtAmount;
        private Button btnAdd, btnSave, btnLoad;
        private DataGridView grid;
        private Label lblBalance;

        public FinanceForm()
        {
            this.Text = "Трекер фінансів (Мультивалютний)";
            this.Size = new Size(650, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Створюємо елементи
            lblType = new Label() { Text = "Тип:", Location = new Point(20, 15), AutoSize = true };
            cbType = new ComboBox() { Location = new Point(20, 35), Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
            cbType.Items.AddRange(new string[] { "Дохід", "Витрата" });
            cbType.SelectedIndex = 1;

            lblCategory = new Label() { Text = "Категорія:", Location = new Point(120, 15), AutoSize = true };
            cbCategory = new ComboBox() { Location = new Point(120, 35), Width = 120 };
            cbCategory.Items.AddRange(new string[] { "Продукти", "Транспорт", "Розваги", "Зарплата", "Подарунок", "Інше" });
            cbCategory.SelectedIndex = 0;

            lblAmount = new Label() { Text = "Сума:", Location = new Point(250, 15), AutoSize = true };
            txtAmount = new TextBox() { Location = new Point(250, 35), Width = 80 };

            lblCurrency = new Label() { Text = "Валюта:", Location = new Point(340, 15), AutoSize = true };
            cbCurrency = new ComboBox() { Location = new Point(340, 35), Width = 60, DropDownStyle = ComboBoxStyle.DropDownList };
            cbCurrency.Items.AddRange(new string[] { "UAH", "USD", "EUR" });
            cbCurrency.SelectedIndex = 0;

            btnAdd = new Button() { Text = "Додати", Location = new Point(410, 33), Width = 80, BackColor = Color.LightGreen };
            btnAdd.Click += BtnAdd_Click;

            lblFilter = new Label() { Text = "Фільтр за категорією:", Location = new Point(20, 80), AutoSize = true };
            cbFilter = new ComboBox() { Location = new Point(160, 77), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cbFilter.Items.AddRange(new string[] { "Всі категорії", "Продукти", "Транспорт", "Розваги", "Зарплата", "Подарунок", "Інше" });
            cbFilter.SelectedIndex = 0;
            cbFilter.SelectedIndexChanged += CbFilter_SelectedIndexChanged;

            grid = new DataGridView()
            {
                Location = new Point(20, 110),
                Size = new Size(590, 300),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            grid.Columns.Add("Type", "Тип");
            grid.Columns.Add("Category", "Категорія");
            grid.Columns.Add("Amount", "Сума");
            grid.Columns.Add("Currency", "Валюта");
            grid.Columns.Add("Date", "Дата");

            lblBalance = new Label() { Text = "Загальний баланс: 0.00 UAH", Location = new Point(20, 430), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };
            btnSave = new Button() { Text = "Зберегти у файл", Location = new Point(350, 430), Width = 120 };
            btnSave.Click += BtnSave_Click;

            btnLoad = new Button() { Text = "Завантажити", Location = new Point(490, 430), Width = 120 };
            btnLoad.Click += BtnLoad_Click;

            // НАДІЙНЕ ДОДАВАННЯ НА ФОРМУ
            this.Controls.Add(lblType);
            this.Controls.Add(cbType);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cbCategory);
            this.Controls.Add(lblAmount);
            this.Controls.Add(txtAmount);
            this.Controls.Add(lblCurrency);
            this.Controls.Add(cbCurrency);
            this.Controls.Add(btnAdd);
            this.Controls.Add(lblFilter);
            this.Controls.Add(cbFilter);
            this.Controls.Add(grid);
            this.Controls.Add(lblBalance);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnLoad);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtAmount.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Введіть коректну суму більше нуля.", "Помилка");
                return;
            }

            transactions.Add(new Transaction
            {
                Type = cbType.Text,
                Category = cbCategory.Text,
                Amount = amount,
                Currency = cbCurrency.Text,
                Date = DateTime.Now
            });
            
            txtAmount.Clear();
            cbFilter.SelectedIndex = 0; 
            UpdateGrid(transactions);
        }

        private void CbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0)
            {
                UpdateGrid(transactions);
            }
            else
            {
                string selectedCategory = cbFilter.Text;
                List<Transaction> filtered = new List<Transaction>();
                
                foreach (var t in transactions)
                {
                    if (t.Category == selectedCategory)
                        filtered.Add(t);
                }
                UpdateGrid(filtered);
            }
        }

        private void UpdateGrid(List<Transaction> listToShow)
        {
            grid.Rows.Clear();
            double totalBalanceUAH = 0;

            foreach (var t in listToShow)
            {
                grid.Rows.Add(t.Type, t.Category, t.Amount, t.Currency, t.Date.ToString("dd.MM.yyyy HH:mm"));

                double multiplier = 1.0;
                if (t.Currency == "USD") multiplier = 41.5;
                else if (t.Currency == "EUR") multiplier = 45.0;

                double amountInUAH = t.Amount * multiplier;

                if (t.Type == "Дохід") totalBalanceUAH += amountInUAH;
                else totalBalanceUAH -= amountInUAH;
            }

            lblBalance.Text = $"Загальний баланс: {totalBalanceUAH:F2} UAH";
            lblBalance.ForeColor = totalBalanceUAH >= 0 ? Color.DarkGreen : Color.Red;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var t in transactions)
                    {
                        writer.WriteLine($"{t.Type};{t.Category};{t.Amount};{t.Currency};{t.Date}");
                    }
                }
                MessageBox.Show("Дані збережено!", "Успіх");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл не знайдено.", "Інформація");
                return;
            }

            try
            {
                transactions.Clear();
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 5)
                    {
                        transactions.Add(new Transaction
                        {
                            Type = parts[0],
                            Category = parts[1],
                            Amount = Convert.ToDouble(parts[2]),
                            Currency = parts[3],
                            Date = Convert.ToDateTime(parts[4])
                        });
                    }
                }
                UpdateGrid(transactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження: " + ex.Message);
            }
        }
    }
}