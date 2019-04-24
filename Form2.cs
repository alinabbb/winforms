using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
namespace WindowsFormsApp2
{

    public partial class Form2 : Form
    {
        int id_w;
        bool priviliges=false;
        public Form2(Form1 form, int id1)
        {
            InitializeComponent();
            form.Hide();
            id_w = id1;
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
            cnn.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = cnn;
            command.Parameters.AddWithValue("@id", id1);
            command.CommandText = "SELECT name_w FROM workers WHERE id_w LIKE @id";
            label5.Text =Convert.ToString(command.ExecuteScalar());
            command.CommandText = "SELECT post FROM workers WHERE  id_w LIKE @id";
            label6.Text = Convert.ToString(command.ExecuteScalar());
            if(label6.Text=="администратор")
            {
                priviliges = true;
            }
            
            cnn.Close();
        }
 
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
                    }
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            orders orders_list = new orders(priviliges, label5.Text);
            panel5.Controls.Add(orders_list);
        }
          
        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            UserControl1 cart = new UserControl1(id_w);
            panel5.Controls.Add(cart);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            providing cart = new providing(priviliges, id_w);
            panel5.Controls.Add(cart);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            workers empl = new workers(priviliges);
            panel5.Controls.Add(empl);
        }
    }
}
