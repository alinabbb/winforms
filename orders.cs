using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp2
{
    public partial class orders : UserControl
    {
        List<string> valuesList = new List<string>();
        DataTable DS=new DataTable();
        bool acess;
        int acess_sort; int sum = 0;
        string name_w1;
        public orders(bool priv, string name_w)
        {
            InitializeComponent();
            name_w1 = name_w;
            acess = priv;
            radioButton1.Checked = true;
            if (acess)
            {
                comboBox1.Visible = true;
                label1.Visible = true;
                MySqlConnection connection1 = new MySqlConnection();
                connection1.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
                MySqlCommand command = new MySqlCommand("Select name_w from workers;", connection1);
                connection1.Open();
                MySqlDataReader dataReader = command.ExecuteReader();
                valuesList.Add("Все");
                acess_sort = 2;
                while (dataReader.Read())
                {
                    valuesList.Add(Convert.ToString(dataReader[0]));
                }
                comboBox1.DataSource = valuesList;
                comboBox1.SelectedIndex = 0;
                update_dgv();
            }
            else
            {
                acess_sort = 1;
                update_dgv();
                DS.DefaultView.RowFilter = "Сотрудник LIKE '%" + name_w1 + "%'"; 
            }
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            for (int j = 0; j < groupBox1.Controls.Count; j++)
            {
                if (groupBox1.Controls[j] is RadioButton)
                {
                    ((RadioButton)groupBox1.Controls[j]).CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
                }
            }
            SumUpd();
        }
        public void update_dgv()
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
            connection.Open();
            DS.Rows.Clear();
            MySqlDataAdapter mySqlDataAdapter2 = new MySqlDataAdapter();
            if (radioButton2.Checked)
              {
                      mySqlDataAdapter2.SelectCommand =new MySqlCommand( "SELECT o.id_o as 'Номер заказа', o.date_o as 'Дата заказа' , o.date_done as 'Дата выполнения', w.name_w as 'Сотрудник', c.name_c as 'Покупатель'," +
                 " o.adres as 'Адрес доставки', sum(od.quantity*g.price) as 'Сумма' from(( (orders o natural join workers w) natural join customers c) natural join order_detail od )join goods g on g.id_g=od.id_g  where o.date_done is null group by 1 order by o.date_o desc;"
                                 , connection);
              }
              if (radioButton1.Checked)
              { 
                      mySqlDataAdapter2.SelectCommand =new MySqlCommand ("SELECT o.id_o as 'Номер заказа', o.date_o as 'Дата заказа' , o.date_done as 'Дата выполнения', w.name_w as 'Сотрудник', c.name_c as 'Покупатель'," +
             " o.adres as 'Адрес доставки', sum(od.quantity*g.price) as 'Сумма' from(( (orders o natural join workers w) natural join customers c) natural join order_detail od )join goods g on g.id_g=od.id_g group by 1  order by date_o desc;",
                              connection); 
              } 
            mySqlDataAdapter2.Fill(DS);
            dataGridView1.DataSource = DS;
            SumUpd();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            update_dgv();
            SumUpd();
            }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
            cnn.Open();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                MySqlCommand cmd1 = new MySqlCommand("Update orders set date_done=@today where id_o=@id_ord;",cnn); 
                cmd1.Parameters.AddWithValue("@today", DateTime.Today);
                cmd1.Parameters.AddWithValue("@id_ord",Convert.ToInt32( row.Cells[0].Value));
                cmd1.ExecuteNonQuery(); 
            }
            update_dgv();
            MessageBox.Show("Операция успешна", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                DS.DefaultView.RowFilter = "Сотрудник LIKE '%" + comboBox1.SelectedItem + "%'";

            }
            else
            {
                DS.DefaultView.RowFilter = null;
            }
            SumUpd();
        }
        private void SumUpd()
        {
            int counter;
            sum = 0;
            // Iterate through the rows, counting sum
            for (counter = 0; counter < (dataGridView1.Rows.Count); counter++)
            {
                if (dataGridView1.Rows[counter].Cells[6].Value != null)
                {
                    sum += Convert.ToInt32(dataGridView1.Rows[counter].Cells[6].Value);
                }
            }
            label4.Text = Convert.ToString(sum);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MySqlConnection cnn1 = new MySqlConnection(@"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;");
            cnn1.Open();
            MySqlDataAdapter adapt = new MySqlDataAdapter();
            if (radioButton4.Checked)
            {
                adapt.SelectCommand = new MySqlCommand( " call date_filter_man(@check,@name,@acess);",cnn1);
                adapt.SelectCommand.Parameters.AddWithValue("@check",1);
                adapt.SelectCommand.Parameters.AddWithValue("@name", name_w1);
                adapt.SelectCommand.Parameters.AddWithValue("@acess", acess_sort);
            }
            if (radioButton3.Checked)
            {
                adapt.SelectCommand = new MySqlCommand(" call date_filter_man(@check,@name,@acess);", cnn1);
                adapt.SelectCommand.Parameters.AddWithValue("@check", 2);
                adapt.SelectCommand.Parameters.AddWithValue("@name", name_w1);
                adapt.SelectCommand.Parameters.AddWithValue("@acess", acess_sort);
            }
            if (radioButton5.Checked)
            {
                adapt.SelectCommand = new MySqlCommand(" call date_filter_man(@check,@name,@acess);", cnn1);
                adapt.SelectCommand.Parameters.AddWithValue("@check", 3);
                adapt.SelectCommand.Parameters.AddWithValue("@name", name_w1);
                adapt.SelectCommand.Parameters.AddWithValue("@acess", acess_sort);
            }
            if (radioButton6.Checked)
            {
                adapt.SelectCommand = new MySqlCommand(" call date_filter_man(@check,@name,@acess);", cnn1);
                adapt.SelectCommand.Parameters.AddWithValue("@check", 4);
                adapt.SelectCommand.Parameters.AddWithValue("@name", name_w1);
                adapt.SelectCommand.Parameters.AddWithValue("@acess", acess_sort);
            }
            if (radioButton7.Checked)
            {
                adapt.SelectCommand = new MySqlCommand(" call date_filter_man(@check,@name,@acess);", cnn1);
                adapt.SelectCommand.Parameters.AddWithValue("@check", 5);
                adapt.SelectCommand.Parameters.AddWithValue("@name", name_w1);
                adapt.SelectCommand.Parameters.AddWithValue("@acess", acess_sort);
            }
            DS.Clear();
            adapt.Fill(DS);
            dataGridView1.DataSource = DS;
            SumUpd();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
