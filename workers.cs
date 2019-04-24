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
    public partial class workers : UserControl
    {
        DataTable DS = new DataTable();
        bool acess = false;
        public workers(bool priv)
        {
            InitializeComponent();
            acess = priv;

            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = @"Data Source=localhost;" + "Initial Catalog=shop;" + "User ID=root;" + "Password=12345;";
               
            MySqlDataAdapter mySqlDataAdapter3 = new MySqlDataAdapter("SELECT g.id_g as 'Номер', gr.name_group as 'Тип' , g.model_g as 'Модель', b.name_b as 'Бренд', g.price as 'Цена'," +
       " g.quantity as 'Количество' FROM (goods g natural join group_g gr) natural join brand b where g.quantity>0 order by gr.name_group;",
                        connection);
            mySqlDataAdapter3.Fill(DS);
            //dataGridView2.CellValueChanged -= dataGridView2_CellValueChanged;//line added after solution given
           // dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
            dataGridView1.DataSource = DS;
            dataGridView1.Columns[0].Visible = false;
            //initcart();
            connection.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
