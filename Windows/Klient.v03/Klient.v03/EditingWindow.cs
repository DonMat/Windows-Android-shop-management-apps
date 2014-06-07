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
    public partial class EditingWindow : Form
    {
        //DataGridView datagrid;
        private ShopContext database;
        DataGridViewRow row;
        int kategoriaId;
        private void FillCategories()
        {
            try
            {
                using (var db = new ShopContext())
                {
                    var data1 = db.Category;
                    dataGridView1.Rows.Clear();
                    foreach (var cat in data1)
                    {
                        dataGridView1.Rows.Add(cat.Id, cat.Nazwa);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
        }
        public EditingWindow(/*DataGridView _datagrid, */DataGridViewRow _row)
        {
            //datagrid = _datagrid;
            row = _row;
            InitializeComponent();
            kategoriaId = (int)row.Cells[7].Value;
            textBox1.Text = (string)row.Cells[1].Value;
            textBox2.Text = ((double)row.Cells[3].Value).ToString();
            textBox3.Text = ((int)row.Cells[4].Value).ToString();
            textBox4.Text = ((int)row.Cells[5].Value).ToString();
            FillCategories();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if ((int)row.Cells[0].Value==kategoriaId)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        public bool EditProduct(int _id, int _kategoriaId, string _nazwa)
        {
            try
            {
                Product Prod = database.Product.First(x => x.Id == _id);
                if (!Prod.Nazwa.Equals(_nazwa))
                {
                    Prod.Nazwa = _nazwa;
                }
                if (Prod.KategoriaId != _kategoriaId)
                {
                    Prod.KategoriaId = _kategoriaId;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditPrice(int _IdCeny, int _ProduktId, DateTime _Od, double _Cena)
        {
            try
            {
                Price pri = database.Price.First(x => x.CenaId == _IdCeny);
                if (pri.Cena != _Cena)
                {
                    pri.Do = _Od;
                    Price PriceToInsert = new Price()
                    {
                        ProduktId = _ProduktId,
                        Od = _Od,
                        Cena = _Cena
                    };
                    database.Price.Add(PriceToInsert);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditVat(int _IdVat, int _ProduktId, DateTime _Od, int _WartoscVat)
        {
            try
            {
                Vat vat = db.Vat.First(x => x.Id == _IdVat);
                if (vat.WartoscVat != _WartoscVat)
                {
                    vat.Do = _Od;
                    Vat VatToInsert = new Vat()
                    {
                        ProduktId = _ProduktId,
                        Od = _Od,
                        WartoscVat = _WartoscVat
                    };

                    db.Vat.Add(VatToInsert);
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditAvail(int _ProduktId, int _ile)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Store pr = db.Store.First(x => x.ProduktId == _ProduktId);
                    if (pr.IloscDostepnych != _ile)
                    {
                        pr.IloscDostepnych = _ile;
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
            int cat;
            double price;
            int vat;
            int availble;
            try
            {
                cat = (int)dataGridView1.CurrentRow.Cells[0].Value;
            }
            catch
            {
                MessageBox.Show("Niepoprawna kategoria");
                return;
            }
            try
            {
                price = Convert.ToDouble(textBox2.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(textBox2.Text+" nie jest poprawną ceną");
                return;
            }
            try
            {
                vat = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(textBox3.Text + " nie jest poprawną stawką vat");
                return;
            }
            try
            {
                availble = Convert.ToInt32(textBox4.Text);
            }
            catch
            {
                MessageBox.Show(textBox4.Text + " nie jest poprawną liczbą");
                return;
            }
            if(availble<(int)row.Cells[6].Value)
            {
                MessageBox.Show(textBox4.Text + " to mniej niż w tym momencie jest zamówionych");
                return;
            }
            if (
                EditProduct((int)row.Cells[0].Value, cat, textBox1.Text) &&
                EditPrice((int)row.Cells[8].Value, (int)row.Cells[0].Value, DateTime.Now, price) &&
                EditVat((int)row.Cells[9].Value, (int)row.Cells[0].Value, DateTime.Now, vat) &&
                EditAvail((int)row.Cells[0].Value, availble))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Wystąpił problem z edytowaniem produktu");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
