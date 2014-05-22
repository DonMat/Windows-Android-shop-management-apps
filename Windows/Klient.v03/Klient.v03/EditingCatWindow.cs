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
        DataGridViewRow row;
        public EditingCatWindow(DataGridViewRow _row)
        {
            row = _row;
            InitializeComponent();
            textBox1.Text = (string)row.Cells[1].Value;
        }

        public bool EditCat(int _id, string _nazwa)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Category cat = db.Category.First(x => x.Id == _id);
                    if (!cat.Nazwa.Equals(_nazwa))
                    {
                        cat.Nazwa = _nazwa;
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
            if(EditCat((int)row.Cells[0].Value, textBox1.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Wystąpił problem z edytowaniem kategorii");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
