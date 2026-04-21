using System;
using System.Windows.Forms;

namespace Project1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e) // NEXT button
        {
            if (radioButton4.Checked)
                Session.BusinessType = "Hotel";
            else if (radioButton5.Checked)
                Session.BusinessType = "Marquee";
            else if (radioButton6.Checked)
                Session.BusinessType = "Hotel & Marquee";
            else
            {
                MessageBox.Show("Please select an option to continue.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open Form7 (Business Info)
            Form7 f7 = new Form7();
            f7.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e) // CANCEL button
        {
            new Form2().Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
