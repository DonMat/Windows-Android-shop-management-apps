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
   

    public partial class AddingCustWindow : Form
    {
        private ShopContext database;

        public AddingCustWindow()
        {
            InitializeComponent();
            label1.Text = "Id Użytkownika";
        }


        
        public bool InsertNewCustomer(int _idkonta, string _imie, string _nazwisko, string _ulica, string _nrDomu, string _miasto, string _kod, string _mail, string _telefon, string _fax )
        {
            try
            {
                Customer CustomerToInsert = new Customer()
                {
                    KontoId = _idkonta,
                    Imie = _imie,
                    Nazwisko = _nazwisko,
                    Ulica = _ulica,
                    NrDomu = _nrDomu,
                    KodPocztowy = _kod,
                    Miasto = _miasto,
                    Mail = _mail,
                    Telefon = _telefon,
                    Fax = _fax
                };
                

                database.Customer.Add(CustomerToInsert);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int _id = Convert.ToInt32(textBox1.Text);
            try
            {
                database = new ShopContext();
                Account acc = database.Account.First(x => x.Id == _id);
               // Customer cust = database.Customer.First(x => x.KontoId == _id);
                if (acc != null )//&& cust == null)
                {
                    if (InsertNewCustomer(_id, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text))
                    {
                        database.SaveChanges();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }

                    else
                    {
                        MessageBox.Show("Wystąpił problem z dodawaniem konta");
                    }
                }
                else
                {
                    MessageBox.Show("Podaj poprawne ID użytkownika");
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
