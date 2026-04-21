using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Project1
{
    public static class DashboardHelper
    {
        static string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        public static void OpenDashboard(Form currentForm)
        {
            string businessType = "";

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                con.Open();
                string q = "SELECT BusinessType FROM BusinessInfo WHERE OrganizationID=@OrgID";
                MySqlCommand cmd = new MySqlCommand(q, con);
                cmd.Parameters.AddWithValue("@OrgID", Session.OrganizationID);

                object result = cmd.ExecuteScalar();
                if (result != null)
                    businessType = result.ToString();
            }

            Form dashboard;

            if (businessType == "Hotel")
                dashboard = new Form11();
            else if (businessType == "Hotel & Marquee")
                dashboard = new Form12();
            else if (businessType == "Marquee")
                dashboard = new Form10();
            else
            {
                MessageBox.Show("Business type not found.");
                return;
            }

            dashboard.Show();
            currentForm.Hide();
        }
    }
}
