using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form8 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        decimal floorPrice = 0;
        decimal roomExtra = 0;
        decimal dayNightMultiplier = 1;

        public Form8()
        {
            InitializeComponent();
            Load += Form8_Load;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            // ComboBox setup
            comboFloor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRoomNo.DropDownStyle = ComboBoxStyle.DropDownList;
            comboDayorNight.DropDownStyle = ComboBoxStyle.DropDownList;

            comboFloor.Items.AddRange(new string[] { "FIRST", "SECOND", "THIRD" });
            comboRoomNo.Items.AddRange(new string[] { "FIRST", "SECOND", "THIRD" });
            comboDayorNight.Items.AddRange(new string[] { "Day", "Night" });

            txtTotal.ReadOnly = true;

            // Attach events
            comboFloor.SelectedIndexChanged += UpdateTotal;
            comboRoomNo.SelectedIndexChanged += UpdateTotal;
            comboDayorNight.SelectedIndexChanged += UpdateTotal;
            txtNumGuest.TextChanged += UpdateTotal;
            txtNumDN.TextChanged += UpdateTotal;
            txtExtraChar.TextChanged += UpdateTotal;
            txtDiscount.TextChanged += UpdateTotal;
        }

        // ===== TOTAL CALCULATION =====
        private void UpdateTotal(object sender, EventArgs e)
        {
            // FLOOR
            switch (comboFloor.Text)
            {
                case "FIRST": floorPrice = 5000; break;
                case "SECOND": floorPrice = 3500; break;
                case "THIRD": floorPrice = 2500; break;
                default: floorPrice = 0; break;
            }

            // ROOM
            switch (comboRoomNo.Text)
            {
                case "FIRST": roomExtra = 1000; break;
                case "SECOND": roomExtra = 700; break;
                case "THIRD": roomExtra = 400; break;
                default: roomExtra = 0; break;
            }

            // DAY/NIGHT
            if (comboDayorNight.Text == "Night")
                dayNightMultiplier = 1.2m;
            else
                dayNightMultiplier = 1m;

            int guests = int.TryParse(txtNumGuest.Text, out int g) ? g : 1;
            int days = int.TryParse(txtNumDN.Text, out int d) ? d : 1;
            decimal extra = decimal.TryParse(txtExtraChar.Text, out decimal ex) ? ex : 0;
            decimal discount = decimal.TryParse(txtDiscount.Text, out decimal dis) ? dis : 0;

            decimal total = ((floorPrice + roomExtra) * dayNightMultiplier) * guests * days + extra - discount;
            if (total < 0) total = 0;

            txtTotal.Text = total.ToString("0");
        }

        // ===== SAVE TO DATABASE =====
        private void btnSaveNext_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();

                    string query = @"INSERT INTO booking_details_hotel
                    (customer_name, cnic, contact, email, room_type, floor, room_no, guests, day_or_night, days_nights, extra_charges, discount, total)
                    VALUES
                    (@name,@cnic,@contact,@email,@roomType,@floor,@roomNo,@guests,@dayNight,@days,@extra,@discount,@total)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@cnic", txtCNIC.Text);
                        cmd.Parameters.AddWithValue("@contact", txtContact.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@roomType", txtRoomType.Text);
                        cmd.Parameters.AddWithValue("@floor", comboFloor.Text);
                        cmd.Parameters.AddWithValue("@roomNo", comboRoomNo.Text);
                        cmd.Parameters.AddWithValue("@guests", int.TryParse(txtNumGuest.Text, out int g) ? g : 1);
                        cmd.Parameters.AddWithValue("@dayNight", comboDayorNight.Text);
                        cmd.Parameters.AddWithValue("@days", int.TryParse(txtNumDN.Text, out int d) ? d : 1);
                        cmd.Parameters.AddWithValue("@extra", decimal.TryParse(txtExtraChar.Text, out decimal ex) ? ex : 0);
                        cmd.Parameters.AddWithValue("@discount", decimal.TryParse(txtDiscount.Text, out decimal dis) ? dis : 0);
                        cmd.Parameters.AddWithValue("@total", decimal.TryParse(txtTotal.Text, out decimal t) ? t : 0);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Booking Saved Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ===== CANCEL =====
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // or open previous form if needed
        }

        // ===== PRINT =====
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = pd,
                Width = 800,
                Height = 600
            };
            preview.ShowDialog();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 50, y = 50, line = 30;

            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 12);

            g.DrawString("HOTEL BOOKING RECEIPT", titleFont, Brushes.Black, x + 150, y);
            y += line * 2;

            g.DrawLine(Pens.Black, x, y, 700, y);
            y += line;

            g.DrawString("Customer Name:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtName.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("CNIC:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtCNIC.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Contact:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtContact.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Room Type:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtRoomType.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Floor:", headerFont, Brushes.Black, x, y);
            g.DrawString(comboFloor.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Room No:", headerFont, Brushes.Black, x, y);
            g.DrawString(comboRoomNo.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Guests:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtNumGuest.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Days/Nights:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtNumDN.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Extra Charges:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtExtraChar.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawString("Discount:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtDiscount.Text, bodyFont, Brushes.Black, x + 200, y);
            y += line;

            g.DrawLine(Pens.Black, x, y, 700, y);
            y += line;

            g.DrawString("TOTAL AMOUNT:", headerFont, Brushes.Black, x, y);
            g.DrawString(txtTotal.Text, bodyFont, Brushes.Black, x + 200, y);
        }
    }
}
