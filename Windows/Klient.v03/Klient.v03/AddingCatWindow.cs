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
        public AddingCatWindow()
        {
            InitializeComponent();
        }

        public bool InsertNewCategory(string _nazwa)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Category CategoryToInsert = new Category()
                    {
                        Nazwa=_nazwa
                    };

                    db.Category.Add(CategoryToInsert);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (InsertNewCategory(textBox1.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Wystąpił problem z dodawaniem produktu");
            }
        }
    }
}
