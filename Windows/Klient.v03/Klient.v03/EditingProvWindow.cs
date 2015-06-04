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
    public partial class EditingProvWindow : Form
    {
        private ShopContext database;

        private DataGridViewRow row;
        public EditingProvWindow(DataGridViewRow _row)
        {
            row = _row;
            InitializeComponent();
            textBox1.Text = (string)row.Cells[1].Value;
            textBox2.Text = (string)row.Cells[2].Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool EditProv(int _id, string _nazwa, string _adres)
        {
            try
            {
                Provider prov = database.Provider.First(x => x.Id == _id);
                if (!prov.Nazwa.Equals(_nazwa))
                {
                    prov.Nazwa = _nazwa;
                }
                if (!prov.Adres.Equals(_adres))
                {
                    prov.Adres = _adres;
                }
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
                if (EditProv((int)row.Cells[0].Value, textBox1.Text, textBox2.Text))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z edytowaniem dostawcy");
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
