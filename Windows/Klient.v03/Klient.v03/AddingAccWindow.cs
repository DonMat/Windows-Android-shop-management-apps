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
    public partial class AddingAccWindow : Form
    {
        private ShopContext database;

        public AddingAccWindow()
        {
            InitializeComponent();
        }

        public string CalculateMD5Hash(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public bool InsertNewAccount(string _nazwa, string _haslo, int _uprawnienia)
        {
            try
            {
                Account AccountToInsert = new Account()
                {
                    NazwaUzytkownika = _nazwa,
                    Haslo = CalculateMD5Hash(_haslo).ToLower(),
                    Uprawnienia = _uprawnienia
                };

                database.Account.Add(AccountToInsert);
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
            int priv;
            try
            {
                priv = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox2.Text + " nie jest poprawną liczbą z zakresu 0 do 3");
                return;
            }
            if (priv > 3 || priv < 0)
            {
                MessageBox.Show(textBox2.Text + " nie jest liczbą z zakresu 0 do 3");
                return;
            }
            try
            {
                database = new ShopContext();
                if (!maskedTextBox1.Text.Equals(maskedTextBox2.Text))
                {
                    MessageBox.Show("Hasło niepoprawnie powtórzone");
                    return;
                }
                if (InsertNewAccount(textBox1.Text, maskedTextBox1.Text, priv))
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
    }
}
