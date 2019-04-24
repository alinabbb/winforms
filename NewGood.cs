using System; 
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp2
{
    public partial class NewGood : Form
    {
        int provider;
        bool succes;
        public NewGood(int c, int prov)
        {
            InitializeComponent();
            label3.Text = Convert.ToString(c);
            provider = prov;
        }
        public bool if_succes( )
        {
            if (succes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void NewGood_Load(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedIndex == 0) || (comboBox1.SelectedIndex == 2))
            {
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label10.Text = "Процессор";
                label11.Text = "Диагональ экрана";
                label12.Text = "Объем ОП";
                textBox6.Visible = true;
                textBox7.Visible = true;
                textBox8.Visible = true;
                textBox9.Visible = true;

            }
            else if(comboBox1.SelectedIndex==1)
            {
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = false;
                label10.Text = "Цветов";
                label11.Text = "Тип устройства";
                label12.Text = "Тип печати";
                textBox6.Visible = true;
                textBox7.Visible = true;
                textBox8.Visible = true;
                textBox9.Visible = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection cnn = new MySqlConnection(@"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;");

                cnn.Open();
                MySqlCommand cmd1 = new MySqlCommand("call new_good_add(@id_gr,@model,@brand,@price,@kolvo,@h1,@h2,@h3,@h4);", cnn);
                cmd1.Parameters.AddWithValue("@id_gr", Convert.ToString(comboBox1.SelectedIndex + 1));
                cmd1.Parameters.AddWithValue("@model", Convert.ToString(textBox4.Text));
                cmd1.Parameters.AddWithValue("@brand", Convert.ToString(textBox2.Text));
                double price1 = Convert.ToDouble(textBox3.Text);
                int price = Convert.ToInt32(Math.Round(price1));
                cmd1.Parameters.AddWithValue("@price", price);
                cmd1.Parameters.AddWithValue("@kolvo", Convert.ToInt32(textBox5.Text));
                cmd1.Parameters.AddWithValue("@h1", Convert.ToString(textBox6.Text));
                cmd1.Parameters.AddWithValue("@h2", Convert.ToString(textBox7.Text));
                cmd1.Parameters.AddWithValue("@h3", Convert.ToString(textBox8.Text));
                cmd1.Parameters.AddWithValue("@h4", Convert.ToString(textBox9.Text));
                cmd1.ExecuteNonQuery();
                MySqlCommand cmd3 = new MySqlCommand("select max(id_g) from goods;", cnn);
                int id_good = Convert.ToInt32(cmd3.ExecuteScalar());
                MySqlCommand cmd2 = new MySqlCommand("call  provide_update(@id_provide,@id_good,@kolvo,@price);", cnn);
                cmd2.Parameters.AddWithValue("@id_provide", provider);
                cmd2.Parameters.AddWithValue("@id_good", id_good);
                cmd2.Parameters.AddWithValue("@kolvo", Convert.ToInt32(textBox5.Text));
                cmd2.Parameters.AddWithValue("@price", Convert.ToInt32(textBox3.Text));
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Готово");
              succes = true;
                cnn.Close();
            }
            catch
            {
                succes = false;
                MessageBox.Show("Ошибка");
            }
            this.Close();
        }
    }
}
