using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project1
{
    public partial class Form22 : Form
    {
        string conString =
            "server=localhost;user id=root;password=123456;database=MarqueeMasterDB;";

        int printRowIndex;
        DataGridView printGrid;
        string printTitle;

        public Form22()
        {
            InitializeComponent();

            btnGenR1.Click += btnGenR1_Click;
            btnGenR2.Click += btnGenR2_Click;
            btnPrint1.Click += btnPrint1_Click;
            btnPrint2.Click += btnPrint2_Click;
        }

        // ================= FORM LOAD =================
        private void Form22_Load(object sender, EventArgs e)
        {
            comboMonth.Items.Clear();
            comboYear.Items.Clear();

            comboMonth.Items.AddRange(new string[]
            {
                "January","February","March","April","May","June",
                "July","August","September","October","November","December"
            });

            for (int y = 2020; y <= DateTime.Now.Year; y++)
                comboYear.Items.Add(y.ToString());
        }

        // ================= MONTHLY REPORT =================
        private void btnGenR1_Click(object sender, EventArgs e)
        {

        }

        // ================= YEARLY REPORT =================
        private void btnGenR2_Click(object sender, EventArgs e)
        {
            if (comboYear.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Year.");
                return;
            }

            int year = int.Parse(comboYear.SelectedItem.ToString());

            string query = @"
                SELECT 
                    MONTH(booking_date) AS Month,
                    COUNT(*) AS TotalBookings,
                    SUM(total) AS TotalCollection
                FROM HM_marquee_bookinginfo
                WHERE YEAR(booking_date)=@year
                GROUP BY MONTH(booking_date)
                ORDER BY MONTH(booking_date)";

            using (MySqlConnection con = new MySqlConnection(conString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@year", year);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
            }
        }

        // ================= PRINT MONTHLY =================
        private void btnPrint1_Click(object sender, EventArgs e)
        {

        }

        // ================= PRINT YEARLY =================
        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count == 0) return;

            printGrid = dataGridView2;
            printRowIndex = 0;
            printTitle = "Yearly Marquee Report";

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            new PrintPreviewDialog { Document = pd }.ShowDialog();
        }

        // ================= COMMON PRINT =================
        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 40, y = 40, h = 25;

            Font titleFont = new Font("Arial", 14, FontStyle.Bold);
            Font font = new Font("Arial", 10);

            e.Graphics.DrawString(printTitle, titleFont, Brushes.Black, x, y);
            y += 40;

            foreach (DataGridViewColumn col in printGrid.Columns)
            {
                e.Graphics.DrawString(col.HeaderText, font, Brushes.Black, x, y);
                x += 120;
            }

            x = 40;
            y += h;

            while (printRowIndex < printGrid.Rows.Count)
            {
                DataGridViewRow row = printGrid.Rows[printRowIndex];
                if (row.IsNewRow) break;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    e.Graphics.DrawString(
                        cell.Value?.ToString() ?? "",
                        font, Brushes.Black, x, y);
                    x += 120;
                }

                x = 40;
                y += h;
                printRowIndex++;

                if (y > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
            printRowIndex = 0;
        }
    }
}
