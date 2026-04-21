using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form5 : Form
    {
        string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form5()
        {
            InitializeComponent();

            btnNext.Click += btnNext_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // comboType with room types
            comboType.Items.Clear();
            comboType.Items.Add("Single");
            comboType.Items.Add("Double");
            comboType.Items.Add("Suite");
            comboType.Items.Add("Conference");
            comboType.SelectedIndex = 0; 
        }

        // nxt button
        private void btnNext_Click(object sender, EventArgs e)
        {
         
            if (string.IsNullOrWhiteSpace(txtFloors.Text) ||
                string.IsNullOrWhiteSpace(txtFloorName.Text) ||
                string.IsNullOrWhiteSpace(txtRooms.Text) ||
                comboType.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtHall.Text) ||
                string.IsNullOrWhiteSpace(txtCap.Text) ||
                string.IsNullOrWhiteSpace(txtDayCharges.Text))
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parse numeric values
            if (!int.TryParse(txtFloors.Text, out int floors))
            {
                MessageBox.Show("Floors must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtRooms.Text, out int rooms))
            {
                MessageBox.Show("Rooms must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCap.Text, out int capacity))
            {
                MessageBox.Show("Capacity must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtDayCharges.Text, out decimal dailyCharges))
            {
                MessageBox.Show("Daily Charges must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roomType = comboType.SelectedItem.ToString();
            string floorName = txtFloorName.Text.Trim();
            string hallName = txtHall.Text.Trim();

            // Inserting into DB
            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();

                    string query = @"
                        INSERT INTO HotelMarqueeInfo
                        (OrganizationID, Floors, FloorName, Rooms, RoomType, HallName, Capacity, DailyCharges)
                        VALUES
                        (@OrgID, @Floors, @FloorName, @Rooms, @RoomType, @Hall, @Capacity, @DailyCharges)";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@OrgID", Session.OrganizationID);
                    cmd.Parameters.AddWithValue("@Floors", floors);
                    cmd.Parameters.AddWithValue("@FloorName", floorName);
                    cmd.Parameters.AddWithValue("@Rooms", rooms);
                    cmd.Parameters.AddWithValue("@RoomType", roomType);
                    cmd.Parameters.AddWithValue("@Hall", hallName);
                    cmd.Parameters.AddWithValue("@Capacity", capacity);
                    cmd.Parameters.AddWithValue("@DailyCharges", dailyCharges);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Hotel & Marquee information saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form10 dashboard = new Form10();
                dashboard.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  CANCEL BUTTON
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form2 loginForm = new Form2();
            loginForm.Show();
            this.Hide();
        }
    }
}
