using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form1 : Form
    {
        string conString = "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public Form1()
        {
            InitializeComponent();

            btnRegister.Click += btnRegister_Click;
            lblLink.LinkClicked += lblLink_LinkClicked; 
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string organizationName = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirm.Text;

            if (string.IsNullOrEmpty(organizationName) || 
                string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password) || 
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtConfirm.Clear();
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    con.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Organizations WHERE Email=@Email";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, con))
                    {
                        cmdCheck.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Email/Username already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO Organizations (OrganizationName, Email, Password) " +
                                         "VALUES (@OrganizationName, @Email, @Password)";

                    using (MySqlCommand cmdInsert = new MySqlCommand(insertQuery, con))
                    {
                        cmdInsert.Parameters.AddWithValue("@OrganizationName", organizationName);
                        cmdInsert.Parameters.AddWithValue("@Email", email);
                        cmdInsert.Parameters.AddWithValue("@Password", password); 
                        cmdInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtName.Clear();
                    txtEmail.Clear();
                    txtPassword.Clear();
                    txtConfirm.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 loginForm = new Form2();
            loginForm.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}