using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Klient.v02
{
    public partial class Form2 : Form
    {
        //Zmienna ktora jest true jesli logowanie sie uda
        private Boolean isCorrect = false;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            AcceptButton = button1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection myConn = new MySqlConnection(Form1.myConnection);
            MySqlCommand SelectCommand = new MySqlCommand("select * from sql332256.Accounts where username = '" + this.textBox1.Text + "' and password = '" + this.textBox2.Text + "' and privileges = 3", myConn);
            MySqlDataReader myReader;
            myConn.Open();
            myReader = SelectCommand.ExecuteReader();
            while (myReader.Read())
            {
                isCorrect = true;
            }
            if (isCorrect == false)
            {
                MessageBox.Show("Zła nazwa użytkownika lub hasło");
            }
            else
            {
                this.Close();
            }
            myConn.Close();
        }
        
        //Chamskie zamykanie
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(isCorrect==false)
                Environment.Exit(0);
        }

       

        
    }
}
