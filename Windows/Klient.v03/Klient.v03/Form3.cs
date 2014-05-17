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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            AcceptButton = button1;
        }

        private void Form3_Closing(object sender, CancelEventArgs e)
        {
            Filling();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Filling())
            {
                using (var db = new ShopContext())
                {
                    try
                    {

                        Accounts account = new Accounts()
                        {
                            NazwaUzytkownika = this.textBox1.Text,
                            Haslo = this.textBox2.Text,
                            Uprawnienia = 3
                        };
                        db.Accounts.Add(account);
                        db.SaveChanges();


                        Customers customer = new Customers()
                        {
                            Id_konta = account.Id,
                            Imie = this.textBox4.Text,
                            Nazwisko = this.textBox5.Text,
                            Ulica = this.textBox6.Text,
                            Nr_domu = this.textBox7.Text,
                            Kod_pocztowy = this.textBox8.Text,
                            Miasto = this.textBox9.Text,
                            Mail = this.textBox10.Text,
                            Telefon = this.textBox11.Text,
                            Fax = this.textBox12.Text

                        };
                        db.Customers.Add(customer);
                        db.SaveChanges();
                    }
                    catch
                    {
                    }
                    
                }
                
            }
        }

        private bool Filling()
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Nieprawidłowa nazwa użytkownika bądź hasło");
                this.DialogResult = DialogResult.Abort;
                return false;
            }
            else if (textBox2.Text.Equals(textBox3.Text))
            {
                if (!string.IsNullOrWhiteSpace(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox6.Text)
                    && !string.IsNullOrWhiteSpace(textBox7.Text) && !string.IsNullOrWhiteSpace(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox9.Text))
               {
                    this.DialogResult = DialogResult.OK;
                    return true;
                }
                else
                {
                    MessageBox.Show("Brak danych");
                    this.DialogResult = DialogResult.Abort;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Niezgodność hasła");
                this.DialogResult = DialogResult.Abort;
                return false;
            }
        }
    }
}
