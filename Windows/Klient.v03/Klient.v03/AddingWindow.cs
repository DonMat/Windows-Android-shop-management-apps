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
    public partial class AddingWindow : Form
    {
        //public DataGridView datagrid;
        private void FillCategories()
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

        public AddingWindow(/*DataGridView _datagrid*/)
        {
            //datagrid = _datagrid;
            InitializeComponent();
            FillCategories();
        }

        public int InsertNewProduct(string _Nazwa, int _KategoriaId)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Product ProductToInsert = new Product()
                    {
                        Nazwa = _Nazwa,
                        KategoriaId = _KategoriaId
                    };

                    db.Product.Add(ProductToInsert);
                    db.SaveChanges();
                    return ProductToInsert.Id;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public bool InsertNewPrice(int _ProduktId, DateTime _Od, DateTime _Do, double _Cena)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Price PriceToInsert = new Price()
                    {
                        ProduktId = _ProduktId,
                        Od = _Od,
                        Do = _Do,
                        Cena = _Cena
                    };

                    db.Price.Add(PriceToInsert);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool InsertNewPrice(int _ProduktId, DateTime _Od, double _Cena)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Price PriceToInsert = new Price()
                    {
                        ProduktId = _ProduktId,
                        Od = _Od,
                        Cena = _Cena
                    };

                    db.Price.Add(PriceToInsert);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool InsertNewVat(int _ProduktId, DateTime _Od, DateTime _Do, int _WartoscVat)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Vat VatToInsert = new Vat()
                    {
                        ProduktId = _ProduktId,
                        Od = _Od,
                        Do = _Do,
                        WartoscVat = _WartoscVat
                    };

                    db.Vat.Add(VatToInsert);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool InsertNewVat(int _ProduktId, DateTime _Od, int _WartoscVat)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Vat VatToInsert = new Vat()
                    {
                        ProduktId = _ProduktId,
                        Od = _Od,
                        WartoscVat = _WartoscVat
                    };

                    db.Vat.Add(VatToInsert);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool InsertNewStore(int _ProduktId, int _IloscDostepnych, int _IloscZamowionych)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Store StoreToInsert = new Store()
                    {
                        ProduktId = _ProduktId,
                        IloscDostepnych = _IloscDostepnych,
                        IloscZamowionych = _IloscZamowionych
                    };

                    db.Store.Add(StoreToInsert);
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
            int productId=InsertNewProduct(textBox1.Text, cat);
            if (productId > 0 && InsertNewPrice(productId, DateTime.Now, price) &&
                InsertNewVat(productId, DateTime.Now, vat) &&
                InsertNewStore(productId, availble, 0))
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
