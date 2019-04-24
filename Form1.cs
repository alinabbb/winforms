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
using System.Drawing.Drawing2D;

namespace WindowsFormsApp2 { 
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
             
        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
             

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
            cnn.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = cnn;
            command.Parameters.AddWithValue("@id", textBox2.Text);
            command.Parameters.AddWithValue("@pass", textBox1.Text);
            command.CommandText = "SELECT COUNT(*) FROM workers WHERE id_w LIKE @id  AND passw LIKE @pass";
            int  c =  Convert.ToInt32(command.ExecuteScalar());
            cnn.Close();
            if (c!=0)
            { 
               int id=Convert.ToInt32(textBox2.Text);
                Form2 newForm = new Form2(this, id);
                newForm.Show();
            }
            else
            {
                label5.Text = "Неправильный ввод";
            }
               
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { textBox1.UseSystemPasswordChar = false; }
            else { textBox1.UseSystemPasswordChar = true; }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        { 

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
