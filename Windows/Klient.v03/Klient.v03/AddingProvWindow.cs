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
    public partial class AddingProvWindow : Form
    {
        private ShopContext database;

        public AddingProvWindow()
        {
            InitializeComponent();
        }

        public bool InsertNewProvider(string _nazwa, string _adres)
        {
            try
            {
                Provider ProviderToInsert = new Provider()
                {
                    Nazwa = _nazwa,
                    Adres = _adres
                };

                database.Provider.Add(ProviderToInsert);
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
            try
            {
                database = new ShopContext();
                if (InsertNewProvider(textBox1.Text, textBox2.Text))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z dodawaniem dostawcy");
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
