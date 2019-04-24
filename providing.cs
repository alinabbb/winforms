using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp2
{
    public partial class providing : UserControl
    {
        bool acess;
        bool check=false;
        int p_id;
        int id_w;
        NewGood new1;
        public providing(bool priv, int id)
        {

            InitializeComponent();
            acess = priv;
            id_w = id;
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged; 
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.AllowUserToAddRows = true;
            if (acess) { checkBox1.Visible = true; }
        }
        public void cell_change(int cell)
        {
            int x;
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            if (dataGridView1.Rows[rowIndex].Cells[cell].Value != null)
            {

                if (Int32.TryParse(dataGridView1.Rows[rowIndex].Cells[cell].Value.ToString(), out x))
                {
                    if ((x < 1))
                    {
                        MessageBox.Show("Не может быть значение меньше 1 ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.CurrentRow.Cells[cell].Value = 1;
                    }
                }
                else
                {
                    MessageBox.Show("Введите только цифры! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.CurrentRow.Cells[cell].Value = 1;
                }
            }
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                int clIndex = dataGridView1.CurrentCell.ColumnIndex;
                if (clIndex == 0 | clIndex == 1 | clIndex == 2)
                {
                    cell_change(clIndex);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (check)
            {
                dataGridView1.AllowUserToAddRows = false;

                MySqlCommand cmd1 = new MySqlCommand();
                MySqlConnection cnn = new MySqlConnection();
                cnn.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
                cmd1.Connection = cnn;
                cnn.Open();
                cmd1.CommandText = "Insert into provide(date_pr,id_w,id_p) values (@today,@id_w,@id_p);";
                cmd1.Parameters.AddWithValue("@today", DateTime.Today);
                cmd1.Parameters.AddWithValue("@id_w", id_w);
                cmd1.Parameters.AddWithValue("@id_p", p_id);
                cmd1.ExecuteNonQuery();
                cmd1.CommandText = "Select max(id_pr) from provide;";
                int id_provide = Convert.ToInt32(cmd1.ExecuteScalar());
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    MySqlCommand check = new MySqlCommand("Select count(*) from goods where id_g=@id_good;", cnn);
                    check.Parameters.AddWithValue("@id_good", Convert.ToInt32(row.Cells[0].Value));
                    int check_exist = Convert.ToInt32(check.ExecuteScalar());
                    if (check_exist != 0)
                    {
                        MySqlCommand cmd2 = new MySqlCommand("call  provide_update(@id_provide,@id_good,@kolvo,@price);", cnn);
                        cmd2.Parameters.AddWithValue("@id_provide", id_provide);
                        cmd2.Parameters.AddWithValue("@id_good", Convert.ToInt32(row.Cells[0].Value));
                        cmd2.Parameters.AddWithValue("@kolvo", Convert.ToInt32(row.Cells[2].Value));
                        cmd2.Parameters.AddWithValue("@price", Convert.ToInt32(row.Cells[1].Value));
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                          new1 = new NewGood(Convert.ToInt32(row.Cells[0].Value), id_provide);
                        new1.Show();
                       
              }
                }
                if (new1.IsDisposed)
                {
                    MessageBox.Show("Операция успешна", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                dataGridView1.Rows.Clear();
                dataGridView1.AllowUserToAddRows = true;

            }
            else
            {
                MessageBox.Show("Укажите поставщика", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    
 

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBox1.Checked))
            { 
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    maskedTextBox1.Visible = true;
                   button1.Text = "Добавить";
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                textBox1.Visible = false;
                maskedTextBox1.Visible = false;
                button1.Text = "Применить";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check = true;
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
            cnn.Open();
            if (checkBox1.Checked)
            {
                //check it for new provider filling 
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                   
                    
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = cnn;
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    command.CommandText = "SELECT id_p FROM provider WHERE name_p LIKE @name";
                    p_id = Convert.ToInt32(command.ExecuteScalar());
                    
                        if (p_id != 0)
                        {
                            MessageBox.Show("Этот поставщик уже в базе! Повторите попытку ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MySqlCommand command1 = new MySqlCommand("Insert into provider(name_p,phone_p,man_p) values(@name,@phone,@man)", cnn);
                            command1.Parameters.AddWithValue("@name", textBox2.Text);
                            command1.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                            command1.Parameters.AddWithValue("@man", textBox1.Text);
                            try
                            {
                                command1.ExecuteNonQuery();
                                MySqlCommand command3 = new MySqlCommand("SELECT id_p FROM provider WHERE name_p LIKE @name;", cnn);

                                command.Parameters.AddWithValue("@name", textBox2.Text);

                                p_id = Convert.ToInt32(command.ExecuteScalar());
                                MessageBox.Show("Поставщик добавлен", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch
                            {
                                MessageBox.Show("возникла ошибка", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                else
                {
                    MessageBox.Show("Заполните поля!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                 
                    MySqlCommand command3 = new MySqlCommand("SELECT id_p FROM provider WHERE name_p LIKE @name;", cnn);
                    command3.Parameters.AddWithValue("@name", textBox2.Text);
                    p_id = Convert.ToInt32(command3.ExecuteScalar());
                if (p_id == 0)
                {
                    MessageBox.Show("Такого поставщика нет. Проверьте ввод!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("нельзя ввести пустую строку!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(r.Index);
            }
        }

        private void providing_Load(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
        
    }
             
    

