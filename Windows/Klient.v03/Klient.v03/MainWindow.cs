﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity; 

namespace Klient.v03
{
    public partial class MainWindow : Form
    {
        public AccountInfo info;
        public DataGridView products;
        private void FillCustomers()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Customer.Join(db.Account,
                       cus => cus.KontoId,
                       acc => acc.Id,
                       (cus, acc) => new { Id = cus.Id, Imie = cus.Imie, Nazwisko = cus.Nazwisko, Ulica = cus.Ulica, NrDomu = cus.NrDomu, Miasto = cus.Miasto, KodPocztowy = cus.KodPocztowy, Mail = cus.Mail, Telefon = cus.Telefon, Fax = cus.Fax, Login = acc.NazwaUzytkownika });
                dataGridView6.Rows.Clear();
                foreach (var cus in data1)
                {
                    dataGridView6.Rows.Add(cus.Id, cus.Login, cus.Imie, cus.Nazwisko, cus.Ulica, cus.NrDomu, cus.Miasto, cus.KodPocztowy, cus.Mail, cus.Telefon, cus.Fax);
                }
            }
            dataGridView6.Sort(dataGridView6.Columns[3], ListSortDirection.Ascending);
            if (dataGridView6.RowCount > 0)
            {
                dataGridView6.Rows[0].Selected = true;
            }
        }

        private void FillDeliveries()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Delivery.Join(db.Provider,
                       del => del.DostawcaId,
                       prov => prov.Id,
                       (del, prov) => new { Id = del.DostawcaId, Dostawca = prov.Nazwa, del.Data });
                dataGridView7.Rows.Clear();
                foreach (var del in data1)
                {
                    dataGridView7.Rows.Add(del.Id, del.Dostawca, del.Data);
                }
            }
            dataGridView7.Sort(dataGridView7.Columns[2], ListSortDirection.Descending);
            if (dataGridView7.RowCount > 0)
            {
                dataGridView7.Rows[0].Selected = true;
            }
        }

        private void FillAccounts()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Account;
                dataGridView4.Rows.Clear();
                foreach (var acc in data1)
                {
                    dataGridView4.Rows.Add(acc.Id, acc.NazwaUzytkownika, acc.Haslo, acc.Uprawnienia);
                }
            }
            dataGridView4.Sort(dataGridView4.Columns[1], ListSortDirection.Ascending);
            if (dataGridView4.RowCount > 0)
            {
                dataGridView4.Rows[0].Selected = true;
            }
        }

        private void FillProviders()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Provider;
                dataGridView5.Rows.Clear();
                foreach (var prov in data1)
                {
                    dataGridView5.Rows.Add(prov.Id, prov.Nazwa, prov.Adres);
                }
            }
            dataGridView5.Sort(dataGridView5.Columns[1], ListSortDirection.Ascending);
            if (dataGridView5.RowCount > 0)
            {
                dataGridView5.Rows[0].Selected = true;
            }
        }
        
        private void FillOrders()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Order.Join(db.OrderDetail,
                       ord => ord.ZamowienieId,
                       det => det.ZamowienieId,
                       (ord, det) => new { Id = ord.ZamowienieId, DataZamowienia = ord.DataZamowienia, DataZrealizowania = ord.DataZrealizowania, KlientId = ord.KlientId, ProduktId = det.ProduktId, Ilosc = det.Ilosc, MiastoDostawy = det.MiastoDostawy, KodPocztowyDostawy = det.KodPocztowyDostawy, AdresDostawy = det.AdresDostawy })
                       .Join(db.Customer,
                       ord => ord.KlientId,
                       cus => cus.Id,
                       (ord, cus) => new { Id = ord.Id, DataZamowienia = ord.DataZamowienia, DataZrealizowania = ord.DataZrealizowania, KlientId = ord.KlientId, ProduktId = ord.ProduktId, Ilosc = ord.Ilosc, MiastoDostawy = ord.MiastoDostawy, KodPocztowyDostawy = ord.KodPocztowyDostawy, AdresDostawy = ord.AdresDostawy, Imie = cus.Imie, Nazwisko = cus.Nazwisko })
                       .Join(db.Product,
                       ord => ord.ProduktId,
                       pro => pro.Id,
                       (ord, pro) => new { Id = ord.Id, DataZamowienia = ord.DataZamowienia, DataZrealizowania = ord.DataZrealizowania, KlientId = ord.KlientId, ProduktId = ord.ProduktId, Ilosc = ord.Ilosc, MiastoDostawy = ord.MiastoDostawy, KodPocztowyDostawy = ord.KodPocztowyDostawy, AdresDostawy = ord.AdresDostawy, Imie = ord.Imie, Nazwisko = ord.Nazwisko, NazwaProduktu = pro.Nazwa });
                dataGridView3.Rows.Clear();
                foreach (var ord in data1)
                {
                    dataGridView3.Rows.Add(ord.Id, ord.NazwaProduktu, ord.Ilosc, ord.DataZamowienia, ord.DataZrealizowania, ord.Imie + " " + ord.Nazwisko, ord.AdresDostawy + ", " + ord.KodPocztowyDostawy + " " + ord.MiastoDostawy);
                }
            }
            dataGridView3.Sort(dataGridView3.Columns[3], ListSortDirection.Descending);
            if (dataGridView3.RowCount > 0)
            {
                dataGridView3.Rows[0].Selected = true;
            }
        }


        private void FillCategory()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Category;
                dataGridView2.Rows.Clear();
                dataGridView2.Rows.Add(0, "<wszystko>");
                foreach (var cat in data1)
                {
                    dataGridView2.Rows.Add(cat.Id, cat.Nazwa);
                }
            }
            dataGridView2.Sort(dataGridView2.Columns[1], ListSortDirection.Ascending);
            if (dataGridView2.RowCount > 0)
            {
                dataGridView2.Rows[0].Selected = true;
            }
        }
        private void FillProduct()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Product.Join(db.Price.Where(x => x.Do == null),
                       pro => pro.Id,
                       pri => pri.ProduktId,
                       (pro, pri) => new { Id = pro.Id, IdKategorii = pro.KategoriaId, Product = pro.Nazwa, Price = pri.Cena, IdCeny = pri.CenaId })
                       .Join(db.Category,
                       pro => pro.IdKategorii,
                       cat => cat.Id,
                       (pro, cat) => new { Id = pro.Id, Product = pro.Product, Kategoria = cat.Nazwa, Price = pro.Price, IdCeny = pro.IdCeny, IdKategorii = pro.IdKategorii })
                       .Join(db.Vat.Where(z => z.Do == null),
                       pro => pro.Id,
                       v => v.ProduktId,
                       (pro, v) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = v.WartoscVat, IdCeny = pro.IdCeny, IdKategorii = pro.IdKategorii, IdVat = v.Id })
                       .Join(db.Store,
                       pro => pro.Id,
                       s => s.ProduktId,
                       (pro, s) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = pro.Vat, Dostępnych = s.IloscDostepnych, Zamówionych = s.IloscZamowionych, IdKategorii = pro.IdKategorii, IdCeny = pro.IdCeny, IdVat = pro.IdVat });
                dataGridView1.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView1.Rows.Add(prod.Id, prod.Product, prod.Kategoria, prod.Price, prod.Vat, prod.Dostępnych, prod.Zamówionych, prod.IdKategorii, prod.IdCeny, prod.IdVat);
                }
            }
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void FillProductCat(int idCat)
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Product.Join(db.Price.Where(x => x.Do == null),
                       pro => pro.Id,
                       pri => pri.ProduktId,
                       (pro, pri) => new { Id = pro.Id, IdKategorii = pro.KategoriaId, Product = pro.Nazwa, Price = pri.Cena })
                       .Join(db.Category.Where(x => x.Id==idCat),
                       pro => pro.IdKategorii,
                       cat => cat.Id,
                       (pro, cat) => new { Id = pro.Id, Product = pro.Product, Kategoria = cat.Nazwa, Price = pro.Price })
                       .Join(db.Vat.Where(z => z.Do == null),
                       pro => pro.Id,
                       v => v.ProduktId,
                       (pro, v) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = v.WartoscVat })
                       .Join(db.Store,
                       pro => pro.Id,
                       s => s.ProduktId,
                       (pro, s) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = pro.Vat, Dostępnych = s.IloscDostepnych, Zamówionych = s.IloscZamowionych });
                dataGridView1.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView1.Rows.Add(prod.Id, prod.Product, prod.Kategoria, prod.Price, prod.Vat, prod.Dostępnych, prod.Zamówionych);
                }
            }
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        public MainWindow()
        {
            info = new AccountInfo();
            this.Visible = false;
            LoginWindow logging = new LoginWindow(info);
            logging.ShowDialog();
            if (logging.DialogResult != DialogResult.OK)
                Environment.Exit(0);

            InitializeComponent();
            FillProduct();
            FillCategory();
            FillOrders();
            FillAccounts();
            FillCustomers();
            FillProviders();
            FillDeliveries();
            MessageBox.Show("Zalogowano jako \"" + info.login + "\", poziom uprawnień:" + info.privilages);
        } 

        public bool InsertNewCategory(string _Nazwa)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Category ReviewToInsert = new Category()
                    {
                        Nazwa = _Nazwa
                    };

                    db.Category.Add(ReviewToInsert);
                    db.SaveChanges();
                    return true;
                }

                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteCategory(int _Id)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    var cat = db.Category.First(x => x.Id == _Id);
                    db.Category.Remove(cat);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteProduct(int _Id)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    var pro = db.Product.First(x => x.Id==_Id);
                    db.Product.Remove(pro);
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
            //FillProductCat("S");
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            AddingWindow adding = new AddingWindow(/*dataGridView1*/);
            adding.ShowDialog();
            if (adding.DialogResult == DialogResult.OK)
            {
                FilterProducts();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                EditingWindow editing = new EditingWindow(/*dataGridView1, */dataGridView1.CurrentRow);
                editing.ShowDialog();
                if (editing.DialogResult == DialogResult.OK)
                {
                    FilterProducts();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczony produkt?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView1.CurrentRow!=null&&DeleteProduct((int)dataGridView1.CurrentRow.Cells[0].Value))
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem produktu");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddingCatWindow adding = new AddingCatWindow();
            adding.ShowDialog();
            if (adding.DialogResult == DialogResult.OK)
            {
                FillProduct();
                FillCategory();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FillProduct();
            FillCategory();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczoną kategorię?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView2.CurrentRow != null && (int)dataGridView2.CurrentRow.Cells[0].Value!=0 && DeleteCategory((int)dataGridView2.CurrentRow.Cells[0].Value))
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem kategorii");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null && (int)dataGridView2.CurrentRow.Cells[0].Value != 0)
            {
                EditingCatWindow editing = new EditingCatWindow(dataGridView2.CurrentRow);
                editing.ShowDialog();
                if (editing.DialogResult == DialogResult.OK)
                {
                    FillProduct();
                    FillCategory();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel=true;
            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno zakończyć działanie aplikacji?");
            confirm.ShowDialog();
            if (confirm.DialogResult == DialogResult.Yes)
            {
                e.Cancel = false;
            }
        }

        private void FilterProducts()
        {
            int idCat=0;
            if (dataGridView2.CurrentRow.Cells[0] != null && (int)dataGridView2.CurrentRow.Cells[0].Value!=0)
            {
                idCat = (int)dataGridView2.CurrentRow.Cells[0].Value;
                FillProductCat(idCat);
            }
            else
            {
                FillProduct();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FilterProducts();
        }
    }
}
