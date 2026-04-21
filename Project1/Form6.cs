using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form6 : Form
    {
        
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form6()
        {
            InitializeComponent();
        }


        private void Form6_Load(object sender, EventArgs e)
        {
           
        }

      
        private void btnSaveNext_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtFloors.Text) ||
                string.IsNullOrWhiteSpace(txtFName.Text) ||
                string.IsNullOrWhiteSpace(txtRooms.Text) ||
                string.IsNullOrWhiteSpace(comboType.Text))
            {
                MessageBox.Show(
                    "Please fill all fields",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    string query = @"INSERT INTO HotelInfo
                                    (Floors, FloorName, RoomsPerFloor, HallRoomType)
                                    VALUES
                                    (@floors, @fname, @rooms, @type)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@floors", txtFloors.Text);
                        cmd.Parameters.AddWithValue("@fname", txtFName.Text);
                        cmd.Parameters.AddWithValue("@rooms", txtRooms.Text);
                        cmd.Parameters.AddWithValue("@type", comboType.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                MessageBox.Show(
                    "Data saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                Form11 dashboard = new Form11();
                dashboard.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
            this.Close();
        }
    }
}
