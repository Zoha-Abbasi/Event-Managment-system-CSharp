using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form11 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form11()
        {
            InitializeComponent();
           
        }

        private void BtnHotelBooking_Click(object sender, EventArgs e)
        {
            Form8 hotelBookingForm = new Form8();
            hotelBookingForm.Show();
        }

        private void BtnAllBookings_Click(object sender, EventArgs e)
        {
            Form13 allBookingsForm = new Form13();
            allBookingsForm.Show();
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            Form15 reportForm = new Form15();
            reportForm.Show();

          
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            LoadHotelBookings();
        }

        private void LoadHotelBookings()
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
                            room_type AS 'Room Type',
                            floor AS 'Floor',
                            room_no AS 'Room No',
                            guests AS 'No of Guests',
                            day_or_night AS 'Price Day/Night',
                            days_nights AS 'No of days/nights',
                            extra_charges AS 'Extra Charges',
                            discount AS 'Discount',
                            total AS 'Total Amount',
                            booking_date AS 'Booking Date'
                        FROM booking_details_hotel
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
