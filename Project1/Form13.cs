using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form13 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form13()
        {
            InitializeComponent();
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadBookingData();
        }

        // SET COLUMNS 
        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;

           
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

        // Fetching DATA
        private void LoadBookingData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    string query = "SELECT booking_id, customer_name, cnic, contact, email, room_type, floor, room_no, guests, day_or_night, days_nights, extra_charges, discount, total FROM booking_details_hotel";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
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

    
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int bookingId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["booking_id"].Value);

                try
                {
                    using (MySqlConnection con = new MySqlConnection(conString))
                    {
                        con.Open();
                        string query = "DELETE FROM booking_details_hotel WHERE booking_id=@id";
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
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

     
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int bookingId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["booking_id"].Value);

                // Get values from selected row
                string customerName = dataGridView1.SelectedRows[0].Cells["customer_name"].Value.ToString();
                string cnic = dataGridView1.SelectedRows[0].Cells["cnic"].Value.ToString();
                string contact = dataGridView1.SelectedRows[0].Cells["contact"].Value.ToString();
                string email = dataGridView1.SelectedRows[0].Cells["email"].Value.ToString();
                string roomType = dataGridView1.SelectedRows[0].Cells["room_type"].Value.ToString();
                string floor = dataGridView1.SelectedRows[0].Cells["floor"].Value.ToString();
                string roomNo = dataGridView1.SelectedRows[0].Cells["room_no"].Value.ToString();
                int guests = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["guests"].Value);
                string dayOrNight = dataGridView1.SelectedRows[0].Cells["day_or_night"].Value.ToString();
                int daysNights = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["days_nights"].Value);
                decimal extraCharges = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["extra_charges"].Value);
                decimal discount = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["discount"].Value);
                decimal total = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["total"].Value);

                try
                {
                    using (MySqlConnection con = new MySqlConnection(conString))
                    {
                        con.Open();
                        string query = @"UPDATE booking_details_hotel 
                                         SET customer_name=@name, cnic=@cnic, contact=@contact, email=@email,
                                             room_type=@roomType, floor=@floor, room_no=@roomNo, guests=@guests,
                                             day_or_night=@dayOrNight, days_nights=@daysNights, extra_charges=@extraCharges,
                                             discount=@discount, total=@total
                                         WHERE booking_id=@id";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", customerName);
                        cmd.Parameters.AddWithValue("@cnic", cnic);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@roomType", roomType);
                        cmd.Parameters.AddWithValue("@floor", floor);
                        cmd.Parameters.AddWithValue("@roomNo", roomNo);
                        cmd.Parameters.AddWithValue("@guests", guests);
                        cmd.Parameters.AddWithValue("@dayOrNight", dayOrNight);
                        cmd.Parameters.AddWithValue("@daysNights", daysNights);
                        cmd.Parameters.AddWithValue("@extraCharges", extraCharges);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@total", total);
                        cmd.Parameters.AddWithValue("@id", bookingId);

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
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }
    }
}
