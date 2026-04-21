using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form10 : Form
    {
        string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            LoadHotelBookings();
            LoadMarqueeBookings();
        }

        private void BtnHotelBooking_Click(object sender, EventArgs e)
        {
            Form17 hotelForm = new Form17();   
            hotelForm.Show();
        }

        private void BtnMarqueeBooking_Click(object sender, EventArgs e)
        {
            Form18 marqueeForm = new Form18(); 
            marqueeForm.Show();
        }

        private void BtnAllBookings_Click(object sender, EventArgs e)
        {
            Form19 allHotelBookings = new Form19();
            Form20 allMarqueeBookings = new Form20();

            allHotelBookings.Show();
            allMarqueeBookings.Show();
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            Form21 hotelReport = new Form21();   
            Form22 marqueeReport = new Form22(); 

            hotelReport.FormClosed += (s, args) => this.Show();
            marqueeReport.FormClosed += (s, args) => this.Show();

            hotelReport.Show();
            marqueeReport.Show();

            this.Hide();
        }

        //  HOTEL BOOKINGS 
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
                        FROM HM_hotel_bookinginfo
                        WHERE booking_date BETWEEN CURDATE()
                        AND DATE_ADD(CURDATE(), INTERVAL 2 DAY)
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
                MessageBox.Show("Error loading hotel bookings: " + ex.Message);
            }
        }

        // MARQUEE BOOKINGS 
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
                            pricing_type AS 'Pricing Type',
                            event_type AS 'Event Type',
                            head_type AS 'Per Head Type',
                            day_night AS 'Day/Night',
                            num_guests AS 'No of Guests',
                            extra_charges AS 'Extra Charges',
                            discount AS 'Discount',
                            total AS 'Total Amount',
                            booking_date AS 'Booking Date'
                        FROM HM_marquee_bookinginfo
                        WHERE booking_date BETWEEN CURDATE()
                        AND DATE_ADD(CURDATE(), INTERVAL 2 DAY)
                        ORDER BY booking_date ASC;
                    ";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading marquee bookings: " + ex.Message);
            }
        }
    }
}
