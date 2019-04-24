using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp2 
{
    
    public partial class UserControl1 : UserControl
    {
        int id1;
        public UserControl1(int id)
        {
            InitializeComponent();
            id1 = id;
        }
        DataTable cart = new DataTable();
        
        DataTable DS = new DataTable();
        DataTable tmDT = new DataTable();
        int sum=0;
        public void initcart()
        {
            cart.Columns.Add("id_g",typeof(int));
            cart.Columns.Add("model_g", typeof(string));
            cart.Columns.Add("price", typeof(int));
            cart.Columns.Add("quant", typeof(int));

        }
        public void updDGV()
        {
           
            dataGridView1.DataSource = DS;

        }
        private void UserControl1_Load(object sender, EventArgs e)
        { 
            MySqlConnection connection = new MySqlConnection( );
            connection.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
          
            updDGV();
            MySqlDataAdapter mySqlDataAdapter2 = new MySqlDataAdapter("select name_b as tm1  from brand ; ", connection);
           
            mySqlDataAdapter2.Fill(tmDT);
            
            MySqlDataAdapter mySqlDataAdapter3 = new MySqlDataAdapter("SELECT g.id_g as 'Номер', gr.name_group as 'Тип' , g.model_g as 'Модель', b.name_b as 'Бренд', g.price as 'Цена'," +
       " g.quantity as 'Количество' FROM (goods g natural join group_g gr) natural join brand b where g.quantity>0 order by gr.name_group;",
                        connection);
            mySqlDataAdapter3.Fill(DS);
            dataGridView2.CellValueChanged -= dataGridView2_CellValueChanged;//line added after solution given
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
            dataGridView1.DataSource = DS;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 111;
            dataGridView1.Columns[2].Width = 111;
            dataGridView1.Columns[3].Width = 111;
            dataGridView1.Columns[4].Width = 111;
            initcart();
            connection.Close();
        
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            cart.Clear();
             
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    DataRow buff = cart.NewRow();
                    buff["id_g"] = Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value);
                    buff["model_g"] = Convert.ToString(dataGridView2.Rows[i].Cells[1].Value);
                    buff["price"] = Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
                    buff["quant"] = Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value);

                    cart.Rows.Add(buff);
                }
                
            if (cart.Rows.Count!=0)
                {
                OrderFinalize OF = new OrderFinalize(cart, Convert.ToInt32(label3.Text), id1);
                OF.Show();
            }
            else
            {
                MessageBox.Show("Заполните корзину ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                int index = dataGridView2.Rows.Add();
                dataGridView2.Rows[index].Cells[0].Value = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                dataGridView2.Rows[index].Cells[1].Value = dataGridView1.SelectedRows[i].Cells[2].Value.ToString();
                dataGridView2.Rows[index].Cells[2].Value = dataGridView1.SelectedRows[i].Cells[4].Value.ToString();
                dataGridView2.Rows[index].Cells[3].Value = dataGridView1.SelectedRows[i].Cells[5].Value.ToString();
                dataGridView2.Rows[index].Cells[4].Value = 1; 
            }
            SumUpd();
            }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int currow = dataGridView2.CurrentCell.RowIndex;
            int x;
            if (dataGridView2.Rows[currow].Cells[4].Value != null)
            {
                if (Int32.TryParse(dataGridView2.Rows[currow].Cells[4].Value.ToString(), out x))
                {
                    if (x < 1)
                    {
                        MessageBox.Show("Не может быть количество меньше 1 ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView2.CurrentRow.Cells[4].Value = 1;
                    }
                    if (x > Convert.ToInt32(dataGridView2.Rows[currow].Cells[3].Value))
                    {
                        MessageBox.Show("Количество больше, чем на складе!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView2.CurrentRow.Cells[4].Value = 1;
                    }
                }
                else
                {
                    MessageBox.Show("Введите только цифры! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView2.CurrentRow.Cells[4].Value = 1;
                }
            }
            SumUpd();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
            foreach (DataGridViewRow r in dataGridView2.SelectedRows)
            {
               int z =Convert.ToInt32( r.Cells[3].Value.ToString());
                z += 1;
                 r.Cells[3].Value = z.ToString();
              }
            SumUpd();
        }
      

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
        
        private void button5_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView2.SelectedRows)
            { 
                dataGridView2.Rows.RemoveAt(r.Index);
            }
            SumUpd();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView2.SelectedRows)
            {
                int z = Convert.ToInt32(r.Cells[3].Value.ToString());
                    z -= 1;
                    r.Cells[3].Value = z.ToString();
            }
            SumUpd();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DS.DefaultView.RowFilter = "Тип LIKE '%" + textBox1.Text + "%' OR Бренд LIKE '%" + textBox1.Text + "%' OR Модель LIKE '%" + textBox1.Text + "%'";
        }
        private void SumUpd()
        {
                int counter;
                sum = 0;
            // Iterate through the rows, counting sum
            for (counter = 0; counter < (dataGridView2.Rows.Count); counter++)
            {
                if (dataGridView2.Rows[counter].Cells["kolvo"].Value != null)
                {
                    sum += Convert.ToInt32(dataGridView2.Rows[counter].Cells["price"].Value)* Convert.ToInt32(dataGridView2.Rows[counter].Cells["kolvo"].Value);
                }
            }
                label3.Text = Convert.ToString(sum);
                                      
         }
 

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
