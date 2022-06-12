using MessagingApp.Models;
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
    public partial class Messages : Form
    {
        public Messages()
        {
            InitializeComponent();
            connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=base_db.accdb");
            timer2.Interval = 3000;
            timer2.Stop();
        }
        List<Models.Credentials> ConnectedUsers = new List<Credentials>();
        OleDbConnection connection;
        int SessionKey;

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectedUsers.Clear();
            ConnectedUsers.Add(new Credentials() { userName = txtMe.Text });
            ConnectedUsers.Add(new Credentials() { userName = txtSup.Text });
            connection.Open();
            using (OleDbCommand cmd = new OleDbCommand("Select * From user_db", connection))
            {
                OleDbDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    if (ConnectedUsers[0].Id == 0 || ConnectedUsers[1].Id == 0)
                    {
                        if (dataReader[1].ToString() == ConnectedUsers[0].userName)
                        {
                            ConnectedUsers[0].Id = Convert.ToInt32(dataReader[0]);
                        }
                        if (dataReader[1].ToString() == ConnectedUsers[1].userName)
                        {
                            ConnectedUsers[1].Id = Convert.ToInt32(dataReader[0]);
                        }
                    }
                }
                SessionKey = (ConnectedUsers[0].Id + ConnectedUsers[0].userName.Length)
                    * (ConnectedUsers[1].Id + ConnectedUsers[1].userName.Length);
            }
            connection.Close();

            timer2.Start();

        }

        private void RefreshTextBox()
        {
            using (OleDbCommand cmd = new OleDbCommand(@"
                                        SELECT user_db.username,message_db.text
                                        FROM user_db INNER JOIN message_db ON user_db.id = message_db.user_id
                                        WHERE(((message_db.session_id) = " + SessionKey + @"))
                                        ORDER BY message_db.Date;
                                                            ", connection))
            {
                OleDbDataReader dataReader = cmd.ExecuteReader();
                richTextBox1.Text = "";
                while (dataReader.Read())
                {
                    richTextBox1.Text += dataReader[0].ToString() + ": " + dataReader[1].ToString() + "\n";
                    Log(dataReader[0].ToString() + ": " + dataReader[1].ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Open();
            using (OleDbCommand cmd = new OleDbCommand(@"
                                        INSERT INTO message_db([user_id],[session_id],[text],[date])
                                        VALUES(@userId,@sessionId,@text,@date)", connection))
            {
                cmd.Parameters.Add("@userId", OleDbType.Integer).Value = ConnectedUsers[0].Id;
                cmd.Parameters.Add("@sessionId", OleDbType.Integer).Value = SessionKey;
                cmd.Parameters.Add("@text", OleDbType.VarChar).Value = messagetxt.Text;
                cmd.Parameters.Add("@date", OleDbType.Date).Value = DateTime.Now;
                int dataSender = cmd.ExecuteNonQuery();
                if (dataSender > 0)
                {
                    RefreshTextBox();
                }
                else
                {
                    MessageBox.Show("Xatolik aniqlandi");
                }
            }
            connection.Close();
        }
        private void Log(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            connection.Open();
            RefreshTextBox();
            connection.Close();
        }

        private void Messages_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer2.Stop();
            connection.Dispose();
        }
    }
}
