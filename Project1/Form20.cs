using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form20 : Form
    {
        string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form20()
        {
            InitializeComponent();
        }

        private void Form20_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadMarqueeBookingData();
        }

        // ================= SET DATAGRIDVIEW COLUMNS =================
        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = false;

            // Hidden ID column
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

        // ================= FETCH DATA =================
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
                                     FROM HM_marquee_bookinginfo";

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

        // ================= DELETE =================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
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
                        "DELETE FROM HM_marquee_bookinginfo WHERE booking_id=@id";
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

        // ================= UPDATE =================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to update.");
                return;
            }

            DataGridViewRow r = dataGridView1.SelectedRows[0];

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();

                    string query = @"UPDATE HM_marquee_bookinginfo SET
                                        customer_name=@name,
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

                    cmd.Parameters.AddWithValue("@name", r.Cells["customer_name"].Value);
                    cmd.Parameters.AddWithValue("@cnic", r.Cells["cnic"].Value);
                    cmd.Parameters.AddWithValue("@contact", r.Cells["contact"].Value);
                    cmd.Parameters.AddWithValue("@email", r.Cells["email"].Value);
                    cmd.Parameters.AddWithValue("@pricingType", r.Cells["pricing_type"].Value);
                    cmd.Parameters.AddWithValue("@eventType", r.Cells["event_type"].Value);
                    cmd.Parameters.AddWithValue("@headType", r.Cells["head_type"].Value);
                    cmd.Parameters.AddWithValue("@dayNight", r.Cells["day_night"].Value);
                    cmd.Parameters.AddWithValue("@numGuests", r.Cells["num_guests"].Value);
                    cmd.Parameters.AddWithValue("@extraCharges", r.Cells["extra_charges"].Value);
                    cmd.Parameters.AddWithValue("@discount", r.Cells["discount"].Value);
                    cmd.Parameters.AddWithValue("@total", r.Cells["total"].Value);
                    cmd.Parameters.AddWithValue("@id", r.Cells["booking_id"].Value);

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
    }
}
