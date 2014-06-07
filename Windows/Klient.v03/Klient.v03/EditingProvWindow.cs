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
            using (var db = new ShopContext())
            {
                try
                {
                    Provider prov = db.Provider.First(x => x.Id == _id);
                    if (!prov.Nazwa.Equals(_nazwa))
                    {
                        prov.Nazwa = _nazwa;
                        if (!prov.Adres.Equals(_adres))
                        {
                            prov.Adres = _adres;
                        }
                        db.SaveChanges();
                    }
                    if (!prov.Adres.Equals(_adres))
                    {
                        prov.Adres = _adres;
                        db.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EditProv((int)row.Cells[0].Value, textBox1.Text, textBox2.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Wystąpił problem z edytowaniem dostawcy");
            }
        }
    }
}
