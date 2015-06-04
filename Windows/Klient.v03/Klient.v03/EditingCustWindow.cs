using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klient.v03
{
    public partial class EditingCustWindow : Form
    {
        private ShopContext database;
        private DataGridViewRow row;

        public EditingCustWindow(DataGridViewRow _row)
        {
            row = _row;
            InitializeComponent();

            textBox1.Text = (string)row.Cells[1].Value;
            textBox2.Text = (string)row.Cells[2].Value;
            textBox3.Text = (string)row.Cells[3].Value;
            textBox4.Text = (string)row.Cells[4].Value;
            textBox5.Text = (string)row.Cells[5].Value;
            textBox6.Text = (string)row.Cells[6].Value;
            textBox7.Text = (string)row.Cells[7].Value;
            textBox8.Text = (string)row.Cells[8].Value;
            textBox9.Text = (string)row.Cells[9].Value;
            textBox10.Text = (string)row.Cells[10].Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool EditCust(int _id, string _imie, string _nazwisko, string _ulica, string _nrDomu, string _miasto, 
            string _kodPocztowy, string _mail, string _telefon, string _fax)
        {
            try
            {
                Customer cust = database.Customer.First(x => x.KontoId == _id);
                if (!cust.Imie.Equals(_imie))
                {
                    cust.Imie = _imie;
                }
                if (!cust.Nazwisko.Equals(_nazwisko))
                {
                    cust.Nazwisko = _nazwisko;
                }
                if (!cust.Ulica.Equals(_ulica))
                {
                    cust.Ulica = _ulica;
                }
                if (!cust.NrDomu.Equals(_nrDomu))
                {
                    cust.NrDomu = _nrDomu;
                }
                if (!cust.Miasto.Equals(_miasto))
                {
                    cust.Miasto = _miasto;
                }
                if (!cust.KodPocztowy.Equals(_kodPocztowy))
                {
                    cust.KodPocztowy = _kodPocztowy;
                }
                if (!cust.Mail.Equals(_mail))
                {
                    cust.Mail = _mail;
                }
                if (!cust.Telefon.Equals(_telefon))
                {
                    cust.Telefon = _telefon;
                }
                if (!cust.Fax.Equals(_fax))
                {
                    cust.Fax = _fax;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Niepoprawny klient");
                return false;
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                database = new ShopContext();
                if(EditCust((int)row.Cells[0].Value, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z edytowaniem kategorii");
                } 
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd zapisu do bazy");
                return;
            }
           
            finally
            {
                database.Dispose();
            }
        }



    }
}
