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
    public partial class Form4 : Form
    {
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

        public Form4()
        {
            InitializeComponent();
            FillCategories();
        }

        public int InsertNewProduct(string _Nazwa, int _Kategoria_id)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Products ProductToInsert = new Products()
                    {
                        Nazwa = _Nazwa,
                        Kategoria_id = _Kategoria_id
                    };

                    db.Products.Add(ProductToInsert);
                    db.SaveChanges();
                    return ProductToInsert.Id;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public bool InsertNewPrice(int _Id_produktu, DateTime _Od, DateTime _Do, double _Cena)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Prices PriceToInsert = new Prices()
                    {
                        Id_produktu = _Id_produktu,
                        Od = _Od,
                        Do = _Do,
                        Cena = _Cena
                    };

                    db.Prices.Add(PriceToInsert);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool InsertNewPrice(int _Id_produktu, DateTime _Od, double _Cena)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Prices PriceToInsert = new Prices()
                    {
                        Id_produktu = _Id_produktu,
                        Od = _Od,
                        Cena = _Cena
                    };

                    db.Prices.Add(PriceToInsert);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool InsertNewVat(int _Id_produktu, DateTime _Od, DateTime _Do, int _Wartosc_vat)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Vat VatToInsert = new Vat()
                    {
                        Id_produktu = _Id_produktu,
                        Od = _Od,
                        Do = _Do,
                        Wartosc_vat = _Wartosc_vat
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

        public bool InsertNewVat(int _Id_produktu, DateTime _Od, int _Wartosc_vat)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Vat VatToInsert = new Vat()
                    {
                        Id_produktu = _Id_produktu,
                        Od = _Od,
                        Wartosc_vat = _Wartosc_vat
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

        public bool InsertNewStore(int _Produkt_id, int _Ilosc_dostepnych, int _Ilosc_zamowionych)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Store StoreToInsert = new Store()
                    {
                        Produkt_id = _Produkt_id,
                        Ilosc_dostepnych = _Ilosc_dostepnych,
                        Ilosc_zamowionych = _Ilosc_zamowionych
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
            int productId=InsertNewProduct(textBox1.Text, (int)dataGridView1.CurrentRow.Cells[0].Value);
            if (productId > 0)
            {
                double price=Convert.ToDouble(textBox2.Text);
                InsertNewPrice(productId, DateTime.Now, price);
                int vat = Convert.ToInt32(textBox3.Text);
                InsertNewVat(productId, DateTime.Now, vat);
                int availble = Convert.ToInt32(textBox4.Text);
                InsertNewStore(productId, availble, 0);
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
