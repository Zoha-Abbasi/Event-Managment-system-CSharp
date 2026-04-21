using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form7 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form7()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtOwnerName.Text) ||
                string.IsNullOrWhiteSpace(txtBusinessName.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string businessType = Session.BusinessType;

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();

                    string query = @"
                        INSERT INTO BusinessInfo
                        (OrganizationID, BusinessType, OwnerName, BusinessName, Contact, City, Address, CNIC)
                        VALUES
                        (@OrgID, @Type, @Owner, @BName, @Contact, @City, @Address, @CNIC)
                        ON DUPLICATE KEY UPDATE
                        BusinessType=@Type,
                        OwnerName=@Owner,
                        BusinessName=@BName,
                        Contact=@Contact,
                        City=@City,
                        Address=@Address,
                        CNIC=@CNIC";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@OrgID", Session.OrganizationID);
                    cmd.Parameters.AddWithValue("@Type", businessType);
                    cmd.Parameters.AddWithValue("@Owner", txtOwnerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@BName", txtBusinessName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@CNIC", txtID.Text.Trim());
                    cmd.ExecuteNonQuery();

                    // Setting profile completed
                    string updateProfile = "UPDATE Organizations SET ProfileCompleted=1 WHERE OrganizationID=@OrgID";
                    MySqlCommand cmd2 = new MySqlCommand(updateProfile, con);
                    cmd2.Parameters.AddWithValue("@OrgID", Session.OrganizationID);
                    cmd2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Open next form based on selection
            switch (businessType)
            {
                case "Hotel":
                    new Form6().Show(); // Hotel Info
                    break;
                case "Marquee":
                    new Form4().Show(); // Marquee Info
                    break;
                case "Hotel & Marquee":
                    new Form5().Show(); // Both Info
                    break;
            }

            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }
    }
}
