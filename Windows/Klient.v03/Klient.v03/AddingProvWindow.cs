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
        public AddingProvWindow()
        {
            InitializeComponent();
        }

        public bool InsertNewProvider(string _nazwa, string _adres)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Provider ProviderToInsert = new Provider()
                    {
                        Nazwa = _nazwa,
                        Adres = _adres
                    };

                    db.Provider.Add(ProviderToInsert);
                    db.SaveChanges();
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
            if (InsertNewProvider(textBox1.Text, textBox2.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Wystąpił problem z dodawaniem produktu");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
