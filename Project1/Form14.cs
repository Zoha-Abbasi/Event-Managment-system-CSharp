using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form14 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form14()
        {
            InitializeComponent();
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadMarqueeBookingData();
        }

        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = false; 

           
            DataGridViewTextBoxColumn idCol = new DataGridViewTextBoxColumn();
            idCol.Name = "booking_id";
            idCol.HeaderText = "ID";
            idCol.Visible = false;
            idCol.ReadOnly = true;
            dataGridView1.Columns.Add(idCol);

            dataGridView1.Columns.Add("customer_name", "Name");
            dataGridView1.Columns.Add("cnic", "CNIC");
            dataGridView1.Columns.Add("contact", "Contact Number");
            dataGridView1.Columns.Add("email", "Email");

            dataGridView1.Columns.Add("pricing_type", "Pricing Type");
            dataGridView1.Columns.Add("event_type", "Event Type");
            dataGridView1.Columns.Add("head_type", "Per Head Type");
            dataGridView1.Columns.Add("day_night", "Day / Night");

            dataGridView1.Columns.Add("num_guests", "No of Guests");
            dataGridView1.Columns.Add("extra_charges", "Extra Charges");
            dataGridView1.Columns.Add("discount", "Discount");
            dataGridView1.Columns.Add("total", "Total Amount");
        }

        // fetching data
        private void LoadMarqueeBookingData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    string query = @"SELECT booking_id,
                                            customer_name,
                                            cnic,
                                            contact,
                                            email,
                                            pricing_type,
                                            event_type,
                                            head_type,
                                            day_night,
                                            num_guests,
                                            extra_charges,
                                            discount,
                                            total
                                     FROM booking_details_marquee";

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
                            row["pricing_type"],
                            row["event_type"],
                            row["head_type"],
                            row["day_night"],
                            row["num_guests"],
                            row["extra_charges"],
                            row["discount"],
                            row["total"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading marquee data: " + ex.Message);
            }
        }

        //DELETE SELECTED ROW 
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
                        string query = "DELETE FROM booking_details_marquee WHERE booking_id=@id";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", bookingId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Booking deleted successfully!");
                    LoadMarqueeBookingData(); 
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

        // UPDATE SELECTED ROW 
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int bookingId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["booking_id"].Value);

                string customerName = dataGridView1.SelectedRows[0].Cells["customer_name"].Value.ToString();
                string cnic = dataGridView1.SelectedRows[0].Cells["cnic"].Value.ToString();
                string contact = dataGridView1.SelectedRows[0].Cells["contact"].Value.ToString();
                string email = dataGridView1.SelectedRows[0].Cells["email"].Value.ToString();
                string pricingType = dataGridView1.SelectedRows[0].Cells["pricing_type"].Value.ToString();
                string eventType = dataGridView1.SelectedRows[0].Cells["event_type"].Value.ToString();
                string headType = dataGridView1.SelectedRows[0].Cells["head_type"].Value.ToString();
                string dayNight = dataGridView1.SelectedRows[0].Cells["day_night"].Value.ToString();
                int numGuests = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["num_guests"].Value);
                decimal extraCharges = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["extra_charges"].Value);
                decimal discount = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["discount"].Value);
                decimal total = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["total"].Value);

                try
                {
                    using (MySqlConnection con = new MySqlConnection(conString))
                    {
                        con.Open();
                        string query = @"UPDATE booking_details_marquee 
                                         SET customer_name=@name,
                                             cnic=@cnic,
                                             contact=@contact,
                                             email=@email,
                                             pricing_type=@pricingType,
                                             event_type=@eventType,
                                             head_type=@headType,
                                             day_night=@dayNight,
                                             num_guests=@numGuests,
                                             extra_charges=@extraCharges,
                                             discount=@discount,
                                             total=@total
                                         WHERE booking_id=@id";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", customerName);
                        cmd.Parameters.AddWithValue("@cnic", cnic);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@pricingType", pricingType);
                        cmd.Parameters.AddWithValue("@eventType", eventType);
                        cmd.Parameters.AddWithValue("@headType", headType);
                        cmd.Parameters.AddWithValue("@dayNight", dayNight);
                        cmd.Parameters.AddWithValue("@numGuests", numGuests);
                        cmd.Parameters.AddWithValue("@extraCharges", extraCharges);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@total", total);
                        cmd.Parameters.AddWithValue("@id", bookingId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Booking updated successfully!");
                    LoadMarqueeBookingData(); 
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
