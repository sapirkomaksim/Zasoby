using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaBooking
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CinemaForm());
        }
    }

    public class CinemaForm : Form
    {
        // Налаштування залу
        private const int ROWS = 5;
        private const int COLS = 6;
        private const int TICKET_PRICE = 150; // Ціна одного квитка

        private int selectedSeatsCount = 0;

        // Елементи форми
        private Label lblScreen;
        private Label lblTotalCost;
        private Button btnBuy;

        public CinemaForm()
        {
            this.Text = "Бронювання місць";
            this.Size = new Size(420, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- ЕКРАН ---
            lblScreen = new Label() 
            { 
                Text = "ЕКРАН", 
                Location = new Point(40, 20), 
                Size = new Size(320, 30), 
                BackColor = Color.Gray, 
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            this.Controls.Add(lblScreen);

            // --- ГЕНЕРАЦІЯ СІТКИ МІСЦЬ ---
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    Button btnSeat = new Button();
                    btnSeat.Size = new Size(45, 45);
                    btnSeat.Location = new Point(40 + (col * 55), 70 + (row * 55));
                    btnSeat.Text = $"{row + 1}-{col + 1}"; 
                    btnSeat.BackColor = Color.LightGreen; // Зелений - вільно
                    btnSeat.Cursor = Cursors.Hand;
                    
                    // Прив'язуємо клік по кнопці до одного методу
                    btnSeat.Click += Seat_Click;
                    
                    this.Controls.Add(btnSeat);
                }
            }

            // --- ТЕКСТ ВАРТОСТІ ---
            lblTotalCost = new Label() 
            { 
                Text = "Загальна вартість: 0 грн", 
                Location = new Point(40, 370), 
                AutoSize = true, 
                Font = new Font("Arial", 11, FontStyle.Bold) 
            };
            this.Controls.Add(lblTotalCost);

            // --- КНОПКА ПОКУПКИ ---
            btnBuy = new Button() 
            { 
                Text = "Оформити бронювання", 
                Location = new Point(40, 410), 
                Size = new Size(320, 40), 
                BackColor = Color.DodgerBlue, 
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnBuy.Click += BtnBuy_Click;
            this.Controls.Add(btnBuy);
        }

        // Логіка кліку по місцю
        private void Seat_Click(object sender, EventArgs e)
        {
            Button clickedSeat = sender as Button;

            // Якщо місце вільне -> бронюємо (червоний колір)
            if (clickedSeat.BackColor == Color.LightGreen)
            {
                clickedSeat.BackColor = Color.Tomato;
                selectedSeatsCount++;
            }
            // Якщо місце вже вибране нами -> скасовуємо вибір (зелений колір)
            else if (clickedSeat.BackColor == Color.Tomato)
            {
                clickedSeat.BackColor = Color.LightGreen;
                selectedSeatsCount--;
            }

            // Оновлюємо ціну
            lblTotalCost.Text = $"Загальна вартість: {selectedSeatsCount * TICKET_PRICE} грн";
        }

        // Логіка кнопки "Оформити"
        private void BtnBuy_Click(object sender, EventArgs e)
        {
            if (selectedSeatsCount == 0)
            {
                MessageBox.Show("Оберіть хоча б одне місце!", "Увага");
                return;
            }

            int total = selectedSeatsCount * TICKET_PRICE;
            MessageBox.Show($"Ви успішно забронювали {selectedSeatsCount} місць на суму {total} грн.", "Успіх");
            
            // Скидаємо лічильник після покупки
            selectedSeatsCount = 0;
            lblTotalCost.Text = "Загальна вартість: 0 грн";
            
            // Робимо куплені місця сірими та неактивними
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn.BackColor == Color.Tomato)
                {
                    btn.BackColor = Color.LightGray;
                    btn.Enabled = false; 
                }
            }
        }
    }
}