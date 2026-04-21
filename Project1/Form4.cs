using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form4 : Form
    {
        string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form4()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtHall.Text) ||
                string.IsNullOrWhiteSpace(txtCapacity.Text) ||
                string.IsNullOrWhiteSpace(txtDCharges.Text) ||
                string.IsNullOrWhiteSpace(txtHCharges.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!int.TryParse(txtCapacity.Text, out int capacity))
            {
                MessageBox.Show("Capacity must be numeric.");
                return;
            }

            if (!decimal.TryParse(txtDCharges.Text, out decimal daily))
            {
                MessageBox.Show("Daily Charges must be numeric.");
                return;
            }

            if (!decimal.TryParse(txtHCharges.Text, out decimal hourly))
            {
                MessageBox.Show("Hourly Charges must be numeric.");
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();

                    string query = @"
                        INSERT INTO MarqueeInfo
                        (OrganizationID, HallName, Capacity, DailyCharges, HourlyCharges)
                        VALUES
                        (@OrgID, @Hall, @Capacity, @Daily, @Hourly)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OrgID", Session.OrganizationID);
                        cmd.Parameters.AddWithValue("@Hall", txtHall.Text.Trim());
                        cmd.Parameters.AddWithValue("@Capacity", capacity);
                        cmd.Parameters.AddWithValue("@Daily", daily);
                        cmd.Parameters.AddWithValue("@Hourly", hourly);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Marquee information saved successfully!");

                //  NEXT FORM 
                Form12 dashboard = new Form12();
                dashboard.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        //  CANCEL 
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
