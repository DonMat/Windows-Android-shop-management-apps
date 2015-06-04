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
    public partial class AddingCatWindow : Form
    {
        private ShopContext database;

        public AddingCatWindow()
        {
            InitializeComponent();
        }

        public bool InsertNewCategory(string _nazwa)
        {
            try
            {
                Category CategoryToInsert = new Category()
                {
                    Nazwa = _nazwa
                };

                database.Category.Add(CategoryToInsert);
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
                if (InsertNewCategory(textBox1.Text))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z dodawaniem kategorii");
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
