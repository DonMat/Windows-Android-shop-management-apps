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
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
            AcceptButton = button2;
            DialogResult = DialogResult.Cancel; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 registration = new Form3();
            registration.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var db = new ShopContext())
            {
                foreach (var entry in db.Accounts)
                {
                    if (entry.NazwaUzytkownika == textBox1.Text && entry.Haslo == maskedTextBox1.Text && entry.Uprawnienia == 3)
                        this.DialogResult = DialogResult.OK;
                }

            }
            this.Close();
        }
    }
}
