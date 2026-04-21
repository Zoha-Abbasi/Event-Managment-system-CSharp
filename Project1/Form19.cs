using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form19 : Form
    {
        string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form19()
        {
            InitializeComponent();
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadBookingData();
        }

        // ================== SET COLUMNS ==================
        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;

            // Hidden ID column
            DataGridViewTextBoxColumn idCol = new DataGridViewTextBoxColumn();
            idCol.Name = "booking_id";
            idCol.HeaderText = "ID";
            idCol.Visible = false;
            dataGridView1.Columns.Add(idCol);

            dataGridView1.Columns.Add("customer_name", "Name");
            dataGridView1.Columns.Add("cnic", "CNIC");
            dataGridView1.Columns.Add("contact", "Contact Number");
            dataGridView1.Columns.Add("email", "Email");

            dataGridView1.Columns.Add("room_type", "Room Type");
            dataGridView1.Columns.Add("floor", "Floor");
            dataGridView1.Columns.Add("room_no", "Room No");
            dataGridView1.Columns.Add("guests", "No of Guests");

            dataGridView1.Columns.Add("day_or_night", "Day / Night");
            dataGridView1.Columns.Add("days_nights", "No of Days/Nights");

            dataGridView1.Columns.Add("extra_charges", "Extra Charges");
            dataGridView1.Columns.Add("discount", "Discount");
            dataGridView1.Columns.Add("total", "Total Amount");
        }

        // ================== FETCH DATA ==================
        private void LoadBookingData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    string query = @"SELECT booking_id, customer_name, cnic, contact, email,
                                            room_type, floor, room_no, guests,
                                            day_or_night, days_nights,
                                            extra_charges, discount, total
                                     FROM HM_hotel_bookinginfo";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.Rows.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            row["booking_id"],
                            row["customer_name"],
                            row["cnic"],
                            row["contact"],
                            row["email"],
                            row["room_type"],
                            row["floor"],
                            row["room_no"],
                            row["guests"],
                            row["day_or_night"],
                            row["days_nights"],
                            row["extra_charges"],
                            row["discount"],
                            row["total"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        // ================== DELETE ==================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row first.");
                return;
            }

            int bookingId =
                Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["booking_id"].Value);

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();
                    string query =
                        "DELETE FROM HM_hotel_bookinginfo WHERE booking_id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", bookingId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Booking deleted successfully!");
                LoadBookingData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting booking: " + ex.Message);
            }
        }

        // ================== UPDATE ==================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row first.");
                return;
            }

            DataGridViewRow r = dataGridView1.SelectedRows[0];

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();

                    string query = @"UPDATE HM_hotel_bookinginfo SET
                                        customer_name=@name,
                                        cnic=@cnic,
                                        contact=@contact,
                                        email=@email,
                                        room_type=@roomType,
                                        floor=@floor,
                                        room_no=@roomNo,
                                        guests=@guests,
                                        day_or_night=@dayOrNight,
                                        days_nights=@daysNights,
                                        extra_charges=@extraCharges,
                                        discount=@discount,
                                        total=@total
                                     WHERE booking_id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", r.Cells["customer_name"].Value);
                    cmd.Parameters.AddWithValue("@cnic", r.Cells["cnic"].Value);
                    cmd.Parameters.AddWithValue("@contact", r.Cells["contact"].Value);
                    cmd.Parameters.AddWithValue("@email", r.Cells["email"].Value);
                    cmd.Parameters.AddWithValue("@roomType", r.Cells["room_type"].Value);
                    cmd.Parameters.AddWithValue("@floor", r.Cells["floor"].Value);
                    cmd.Parameters.AddWithValue("@roomNo", r.Cells["room_no"].Value);
                    cmd.Parameters.AddWithValue("@guests", r.Cells["guests"].Value);
                    cmd.Parameters.AddWithValue("@dayOrNight", r.Cells["day_or_night"].Value);
                    cmd.Parameters.AddWithValue("@daysNights", r.Cells["days_nights"].Value);
                    cmd.Parameters.AddWithValue("@extraCharges", r.Cells["extra_charges"].Value);
                    cmd.Parameters.AddWithValue("@discount", r.Cells["discount"].Value);
                    cmd.Parameters.AddWithValue("@total", r.Cells["total"].Value);
                    cmd.Parameters.AddWithValue("@id", r.Cells["booking_id"].Value);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Booking updated successfully!");
                LoadBookingData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating booking: " + ex.Message);
            }
        }
    }
}
