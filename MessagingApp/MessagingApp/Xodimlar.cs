using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessagingApp
{
    public partial class Xodimlar : Form
    {
        public Xodimlar()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=base_db.accdb");
            conn.Open();
            InitializeComponent();
        }
        OleDbConnection conn;
        private void Xodimlar_Load(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }
        int rowindex;
        int columnindex;
        string dataId;

        private void RefreshDataGrid()
        {
            using (OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM user_db",conn))
            {
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable employees = new DataTable();
                da.Fill(employees);
                dataGridView1.DataSource = employees;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO user_db(username) VALUES(@userNameValue)", conn))
                {
                    cmd.Parameters.Add("@userNameValue", OleDbType.VarChar).Value = textBox1.Text;
                    int dataSender = cmd.ExecuteNonQuery();
                    if (dataSender > 0)
                    {
                        RefreshDataGrid();
                        textBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Xatolik aniqlandi");
                    }
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            using(OleDbCommand cmd = new OleDbCommand("UPDATE user_db SET username=@username WHERE id = @dataId", conn))
            {
                cmd.Parameters.Add("@username", OleDbType.VarChar).Value = (textBox1.Text);
                cmd.Parameters.Add("@dataId", OleDbType.Integer).Value = dataId;
                int updatedUserCount = cmd.ExecuteNonQuery();
                if (updatedUserCount > 0)
                {
                    RefreshDataGrid();
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("Xatolik aniqlandi");
                }
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            int columnindex = 0;
            string deletedId = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();


            using (OleDbCommand cmd = new OleDbCommand("DELETE * FROM user_db WHERE id = @dataId", conn))
            {
                cmd.Parameters.Add("@dataId", OleDbType.Integer).Value = dataId;
                int updatedUserCount = cmd.ExecuteNonQuery();
                if (updatedUserCount > 0)
                {
                    RefreshDataGrid();
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("Xatolik aniqlandi");
                }
            }
        }

        private void Xodimlar_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            conn.Dispose();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rowindex = dataGridView1.CurrentCell.RowIndex;
            columnindex = 0;
            dataId = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();
            textBox1.Text = dataGridView1.Rows[rowindex].Cells[columnindex + 1].Value.ToString();
        }

    }
}
