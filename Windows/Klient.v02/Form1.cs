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
    public partial class Form1 : Form
    {
        //Nasza baza danych
        public static string myConnection = "datasource=sql3.freesqldatabase.com;port=3306;username=sql332256;password=pB1*gE7%;Database=sql332256";
        //Tabela przechowujaca nazwy tabel z bazy ktore sa pokazywane na kolejnych zakladkach
        public static string[] names = { "Products", "Accounts" };
        //DataObject da;


        //Bierze odpowiednia tabele z bazy i wypelnia DataGrida ktory jej odpowiada
        private void fill(DataGridView dataGridView, string table)
        {
            string connectionString = Form1.myConnection;
            string query = "Select * from " + table;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView.DataSource = ds.Tables[0];
                }
            }
        }

        //Usuwanie z wybranej tabeli wskazywanego wiersza
        private void delete(DataGridView dataGridView, string table)
        {
            int id = (int)dataGridView.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value;
            MySqlConnection myConn = new MySqlConnection(Form1.myConnection);
            MySqlCommand SelectCommand = new MySqlCommand("delete from sql332256." + table + " where " + dataGridView.Columns[0].HeaderText + " = " + id, myConn);
            MySqlDataReader myReader;
            myConn.Open();
            myReader = SelectCommand.ExecuteReader();
            myConn.Close();
        }

        public Form1()
        {
            InitializeComponent();
            fill(dataGridView1, names[0]);

            /*
            this.da = new DataObject();
            this.da.Updated +=
                new EventHandler<DataObjectEventArgs>(da_Updated);
            this.Load += new EventHandler(Form1_Load);  */
        }
        /*
        void da_Updated(object sender, DataObjectEventArgs e)
        {
            string[] data = (string[])e.Obj;
            //DODAWANIE
            return;
        } 
         */

        //Ekran logowania
        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 login = new Form2();
            login.ShowDialog();
        }

        //Kasowanie i uaktualnianie tabeli Products
        private void button2_Click(object sender, EventArgs e)
        {
            delete(dataGridView1, names[0]);
            fill(dataGridView1, names[0]);
        }

       
        //Wyswietla okienko proszoce o potwierdzenie zamkniecia glownego okna
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form3 reallyClose = new Form3();
            DialogResult result = reallyClose.ShowDialog();
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Form4 adding = new Form4();
            adding.Text = "Dodaj nowy wiersz";
            
        }


        
       
    }
}
