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
namespace WindowsFormsApp2
{
    public partial  class OrderFinalize : Form
    {
        double summa;
        int cust_id=-1;
        int id_w;
        bool is_applied;
        DataTable cart_f = new DataTable();
        DataTable order_grid = new DataTable();
        bool newCust = false;
        public OrderFinalize( DataTable cartGrid, int summ, int id)
        {
            InitializeComponent();
            cart_f = cartGrid;
            id_w = id;
            summa = Convert.ToDouble(summ);
        }
        private void OrderFinalize_Load(object sender, EventArgs e)
        {
            maskedTextBox1.MaskInputRejected += new MaskInputRejectedEventHandler(maskedTextBox1_MaskInputRejected);
            dataGridView1.DataSource = cart_f;
            label9.Text = Convert.ToString(summa);
            label7.Text = Convert.ToString(summa);
            is_applied = false;
            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                if (groupBox1.Controls[i] is RadioButton)
                {
                    ((RadioButton)groupBox1.Controls[i]).CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
                }
            }
           
            radioButton2.Checked = true;
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked==true)
            {
                label1.Visible = true;
                label2.Visible = true;
                label4.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true; 
                maskedTextBox1.Visible = true;
                button1.Visible = true;
                newCust = true;
            }
            else if(radioButton2.Checked==true)
            {
                label1.Visible = false;
                label2.Visible = false; 
                label4.Visible = true;
                label3.Visible = false;
                textBox1.Visible = true;
                textBox2.Visible = false; 
                maskedTextBox1.Visible = false;
                button1.Visible = true;
            }
            else if (radioButton1.Checked)
            {
                label1.Visible = false;
                label2.Visible = false; 
                label4.Visible = false;
                label3.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false; 
                maskedTextBox1.Visible = false;
                button1.Visible = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd1 = new MySqlCommand("Insert into order_detail values(@,@id_tov,@quant);");
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
            cnn.Open();
            if (radioButton2.Checked)
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = cnn;
                command.Parameters.AddWithValue("@code", textBox1.Text); 
                command.CommandText = "SELECT id_c FROM customers WHERE discount LIKE @code";
                cust_id = Convert.ToInt32(command.ExecuteScalar());
                if (cust_id==0)
                {
                    MessageBox.Show("Такого клиента нет! Повторите попытку ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(!is_applied)
                {
                    is_applied = true;
                    int discount =Convert.ToInt32( Math.Round(summa * 0.05));
                    summa -= discount;
                    label5.Text = Convert.ToString(discount);
                    label9.Text= Convert.ToString(summa);
                }
                else
                {
                    MessageBox.Show("Скидка уже применена ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if(radioButton3.Checked)
            {
                MySqlCommand command = new MySqlCommand("Select count(*) from customers where discount=@card1;",cnn);
                command.Parameters.AddWithValue("@card1", textBox1.Text);
                if (Convert.ToInt32(command.ExecuteScalar()) != 0)
                {
                    MessageBox.Show("Эта карта уже выдана! Повторите попытку ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MySqlCommand command1 = new MySqlCommand("Insert into customers(name_c,phone_c,discount) values(@name,@phone,@card)",cnn);
                    command1.Parameters.AddWithValue("@name", textBox2.Text);
                    command1.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                    command1.Parameters.AddWithValue("@card", textBox1.Text);
                    
                    try
                    {
                        command1.ExecuteNonQuery();
                        MessageBox.Show("Покупатель добавлен", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MySqlCommand command0 = new MySqlCommand("SELECT max(id_c) FROM customers ;", cnn);
                        cust_id = Convert.ToInt32(command0.ExecuteScalar());
                    }
                    catch
                    {
                        MessageBox.Show("возникла ошибка", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                 
                }
            }
            
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                cust_id = 9;
            }
            MySqlCommand cmd1 = new MySqlCommand();
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
            cmd1.Connection = cnn;
            cnn.Open();
                cmd1.CommandText = "Insert into orders(date_o,id_w,id_c,adres) values (@today,@id_w,@id_c,@adress);";
                cmd1.Parameters.AddWithValue("@today", DateTime.Today);
                cmd1.Parameters.AddWithValue("@id_w", id_w);
                cmd1.Parameters.AddWithValue("@id_c", cust_id);
                cmd1.Parameters.AddWithValue("@adress", textBox4.Text);
                cmd1.ExecuteNonQuery();
                cmd1.CommandText = "Select max(id_o) from orders;";
                int id_order = Convert.ToInt32(cmd1.ExecuteScalar());
                 try
                 {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        MySqlCommand cmd2 = new MySqlCommand("call insert_update_cust(@id_ord ,@kolvo,@id_good);", cnn);
                        cmd2.Parameters.AddWithValue("@id_ord", id_order);
                        cmd2.Parameters.AddWithValue("@id_good", Convert.ToInt32(row.Cells[0].Value));
                        cmd2.Parameters.AddWithValue("@kolvo", Convert.ToInt32(row.Cells[3].Value));
                    cmd2.ExecuteNonQuery();
                    }
                MessageBox.Show("Операция успешна", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
              }
                catch
                 {
                   MessageBox.Show("возникла ошибка", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);

               }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
