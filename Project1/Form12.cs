using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form12 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form12()
        {
            InitializeComponent();
           
        }

        private void BtnMarqueeBooking_Click(object sender, EventArgs e)
        {
            Form9 marqueeBookingForm = new Form9();
            marqueeBookingForm.Show();
        }

        private void BtnAllBookings_Click(object sender, EventArgs e)
        {
            Form14 allBookingsForm = new Form14();
            allBookingsForm.Show();
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            Form16 reportForm = new Form16();
            reportForm.Show();

           
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            LoadMarqueeBookings();
        }

        private void LoadMarqueeBookings()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(conString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            customer_name AS 'Name',
                            cnic AS 'CNIC',
                            contact AS 'Contact Number',
                            email AS 'Email',
                            pricing_type AS 'Pricing Type',     -- Per Head / Fixed
                            event_type AS 'Event Type',
                            head_type AS 'Per Head Type',
                            day_night AS 'Day/Night',
                            num_guests AS 'No of Guests',
                            extra_charges AS 'Extra Charges',
                            discount AS 'Discount',
                            total AS 'Total Amount',
                            booking_date AS 'Booking Date'
                        FROM booking_details_marquee
                        WHERE booking_date BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL 2 DAY)
                        ORDER BY booking_date ASC;
                    ";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message);
            }
        }
    }
}
