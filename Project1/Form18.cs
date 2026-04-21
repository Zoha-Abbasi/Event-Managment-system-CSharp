using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form18 : Form
    {
        string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        decimal hallPrice = 150000;
        decimal perHeadPrice = 0;

        public Form18()
        {
            InitializeComponent();

            radioPerHead.CheckedChanged -= radioPerHead_CheckedChanged;
            radioPerHead.CheckedChanged += radioPerHead_CheckedChanged;

            radioFixedPrice.CheckedChanged -= radioFixedPrice_CheckedChanged;
            radioFixedPrice.CheckedChanged += radioFixedPrice_CheckedChanged;

            comboHeadType.SelectedIndexChanged -= comboHeadType_SelectedIndexChanged;
            comboHeadType.SelectedIndexChanged += comboHeadType_SelectedIndexChanged;

            txtNumGuests.KeyPress -= txtNumGuests_KeyPress;
            txtNumGuests.KeyPress += txtNumGuests_KeyPress;
            txtNumGuests.TextChanged -= TxtNumGuests_TextChanged;
            txtNumGuests.TextChanged += TxtNumGuests_TextChanged;

            txtExtraCharges.KeyPress -= txtExtraCharges_KeyPress;
            txtExtraCharges.KeyPress += txtExtraCharges_KeyPress;
            txtExtraCharges.TextChanged -= TxtExtraCharges_TextChanged;
            txtExtraCharges.TextChanged += TxtExtraCharges_TextChanged;

            txtDiscount.KeyPress -= txtDiscount_KeyPress;
            txtDiscount.KeyPress += txtDiscount_KeyPress;
            txtDiscount.TextChanged -= TxtDiscount_TextChanged;
            txtDiscount.TextChanged += TxtDiscount_TextChanged;

            btnSaveNext.Click -= btnSaveNext_Click;
            btnSaveNext.Click += btnSaveNext_Click;

            btnPrint.Click -= btnPrint_Click;
            btnPrint.Click += btnPrint_Click;

            this.Load += Form18_Load;
        }

        private void Form18_Load(object sender, EventArgs e)
        {
            comboEventType.Items.AddRange(
                new string[] { "Wedding", "Mehndi", "Birthday", "Corporate" });

            comboHeadType.Items.AddRange(
                new string[] { "Standard", "Premium", "Royal" });

            comboDN.Items.AddRange(new string[] { "Day", "Night" });

            comboHeadType.SelectedIndex = 0;

            txtTotal.ReadOnly = true;
            comboHeadType.Enabled = false;
            txtNumGuests.Enabled = false;

            radioPerHead.Checked = false;
            radioFixedPrice.Checked = false;
            txtNumGuests.Text = "0";
        }

        private void radioPerHead_CheckedChanged(object sender, EventArgs e)
        {
            comboHeadType.Enabled = radioPerHead.Checked;
            txtNumGuests.Enabled = radioPerHead.Checked;

            if (radioPerHead.Checked && txtNumGuests.Text == "0")
                txtNumGuests.Text = "1";

            CalculateTotal();
        }

        private void radioFixedPrice_CheckedChanged(object sender, EventArgs e)
        {
            comboHeadType.Enabled = false;
            txtNumGuests.Enabled = false;

            if (radioFixedPrice.Checked)
                txtNumGuests.Text = "0";

            CalculateTotal();
        }

        private void comboHeadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboHeadType.Text)
            {
                case "Standard": perHeadPrice = 2500; break;
                case "Premium": perHeadPrice = 3500; break;
                case "Royal": perHeadPrice = 5000; break;
                default: perHeadPrice = 0; break;
            }
            CalculateTotal();
        }

        private void TxtNumGuests_TextChanged(object sender, EventArgs e) => CalculateTotal();
        private void TxtExtraCharges_TextChanged(object sender, EventArgs e) => CalculateTotal();
        private void TxtDiscount_TextChanged(object sender, EventArgs e) => CalculateTotal();

        private void txtNumGuests_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtExtraCharges_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
                e.Handled = true;

            if (e.KeyChar == '.' && txtExtraCharges.Text.Contains("."))
                e.Handled = true;
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
                e.Handled = true;

            if (e.KeyChar == '.' && txtDiscount.Text.Contains("."))
                e.Handled = true;
        }

        private void CalculateTotal()
        {
            int guests = int.TryParse(txtNumGuests.Text, out guests) ? guests : 0;
            decimal extra = decimal.TryParse(txtExtraCharges.Text, out extra) ? extra : 0;
            decimal discount = decimal.TryParse(txtDiscount.Text, out discount) ? discount : 0;

            decimal total =
                radioPerHead.Checked ? guests * perHeadPrice :
                radioFixedPrice.Checked ? hallPrice : 0;

            total = total + extra - discount;
            if (total < 0) total = 0;

            txtTotal.Text = total.ToString("0");
        }

        // ===== SAVE TO DATABASE (TABLE CHANGED) =====
        private void btnSaveNext_Click(object sender, EventArgs e)
        {
            try
            {
                string pricingType =
                    radioPerHead.Checked ? "PerHead" :
                    radioFixedPrice.Checked ? "Fixed" : "";

                int guests =
                    radioPerHead.Checked ? int.Parse(txtNumGuests.Text) : 0;

                decimal extra =
                    decimal.Parse(string.IsNullOrEmpty(txtExtraCharges.Text) ? "0" : txtExtraCharges.Text);

                decimal discount =
                    decimal.Parse(string.IsNullOrEmpty(txtDiscount.Text) ? "0" : txtDiscount.Text);

                decimal total =
                    decimal.Parse(string.IsNullOrEmpty(txtTotal.Text) ? "0" : txtTotal.Text);

                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    string query = @"INSERT INTO HM_marquee_bookinginfo
                    (customer_name, cnic, contact, email,
                     pricing_type, event_type, head_type, day_night,
                     num_guests, extra_charges, discount, total)
                    VALUES
                    (@name,@cnic,@contact,@email,
                     @pricing,@event,@head,@dn,
                     @guests,@extra,@discount,@total)";

                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@cnic", txtCNIC.Text);
                    cmd.Parameters.AddWithValue("@contact", txtContact.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@pricing", pricingType);
                    cmd.Parameters.AddWithValue("@event", comboEventType.Text);
                    cmd.Parameters.AddWithValue("@head", comboHeadType.Text);
                    cmd.Parameters.AddWithValue("@dn", comboDN.Text);
                    cmd.Parameters.AddWithValue("@guests", guests);
                    cmd.Parameters.AddWithValue("@extra", extra);
                    cmd.Parameters.AddWithValue("@discount", discount);
                    cmd.Parameters.AddWithValue("@total", total);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Marquee Booking Saved Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DB Error");
            }
        }

        // ===== PRINT =====
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintDocument_PrintPage;

            PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = pd,
                Width = 900,
                Height = 600
            };
            preview.ShowDialog();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 50, y = 50, line = 30;

            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font headFont = new Font("Arial", 12, FontStyle.Bold);
            Font normalFont = new Font("Arial", 12);

            g.DrawString("Marquee Booking Receipt", titleFont, Brushes.Black, x, y);
            y += line * 2;

            g.DrawLine(Pens.Black, x, y, x + 500, y);
            y += line;

            string bookingType =
                radioPerHead.Checked ? "Per Head" : "Fixed Price";

            void Draw(string label, string value)
            {
                g.DrawString(label, headFont, Brushes.Black, x, y);
                g.DrawString(value, normalFont, Brushes.Black, x + 200, y);
                y += line;
            }

            Draw("Customer Name:", txtName.Text);
            Draw("Event Type:", comboEventType.Text);
            Draw("Pricing Type:", bookingType);
            Draw("Head Type:", comboHeadType.Text);
            Draw("No of Guests:", txtNumGuests.Text);
            Draw("Extra Charges:", txtExtraCharges.Text);
            Draw("Discount:", txtDiscount.Text);

            g.DrawLine(Pens.Black, x, y, x + 500, y);
            y += line;

            Draw("TOTAL AMOUNT:", txtTotal.Text);

            y += line * 2;
            g.DrawString("Thank you for booking with us!",
                headFont, Brushes.Black, x, y);
        }

        private void bnCancel_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
            this.Hide();
        }
    }
}
