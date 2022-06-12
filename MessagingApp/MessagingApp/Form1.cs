using MessagingApp.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MessagingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessagingApp.Messages msForm = new Messages();
            msForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Xodimlar xForm = new Xodimlar();
            xForm.ShowDialog();
        }
    }
}
