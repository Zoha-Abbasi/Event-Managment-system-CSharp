using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form2 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form2()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter Email and Password");
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();
                    string query = @"SELECT OrganizationID, OrganizationName, ProfileCompleted FROM Organizations WHERE Email=@Email AND Password=@Password";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Session.OrganizationID = dr.GetInt32("OrganizationID");
                                Session.OrganizationName = dr.GetString("OrganizationName");
                                bool profileCompleted = dr.GetInt32("ProfileCompleted") == 1;
                                dr.Close();

                                if (profileCompleted)
                                {
                                    string typeQuery = "SELECT BusinessType FROM BusinessInfo WHERE OrganizationID=@OrgID";
                                    using (MySqlCommand cmdType = new MySqlCommand(typeQuery, con))
                                    {
                                        cmdType.Parameters.AddWithValue("@OrgID", Session.OrganizationID);
                                        object result = cmdType.ExecuteScalar();
                                        Session.BusinessType = result != null ? result.ToString() : "";

                                        switch (Session.BusinessType)
                                        {
                                            case "Hotel": new Form11().Show(); break;
                                            case "Marquee": new Form12().Show(); break;
                                            case "Hotel & Marquee": new Form10().Show(); break;
                                            default: new Form3().Show(); break;
                                        }
                                        this.Hide();
                                    }
                                }
                                else
                                {
                                    new Form3().Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Email or Password");
                                txtPassword.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
