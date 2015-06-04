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
    public partial class EditingCatWindow : Form
    {
        private ShopContext database;

        DataGridViewRow row;
        public EditingCatWindow(DataGridViewRow _row)
        {
            row = _row;
            InitializeComponent();
            textBox1.Text = (string)row.Cells[1].Value;
        }

        public bool EditCat(int _id, string _nazwa)
        {
            try
            {
                Category cat = database.Category.First(x => x.Id == _id);
                if (!cat.Nazwa.Equals(_nazwa))
                {
                    cat.Nazwa = _nazwa;
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
                if (EditCat((int)row.Cells[0].Value, textBox1.Text))
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
