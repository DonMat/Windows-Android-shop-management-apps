using System;
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
        private ShopContext database;
        private void FillCustomers()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Customer.Join(database.Account,
                       cus => cus.KontoId,
                       acc => acc.Id,
                       (cus, acc) => new { Id = cus.KontoId, Imie = cus.Imie, Nazwisko = cus.Nazwisko, Ulica = cus.Ulica, NrDomu = cus.NrDomu, Miasto = cus.Miasto, KodPocztowy = cus.KodPocztowy, Mail = cus.Mail, Telefon = cus.Telefon, Fax = cus.Fax, Login = acc.NazwaUzytkownika });
                dataGridView6.Rows.Clear();
                foreach (var cus in data1)
                {
                    dataGridView6.Rows.Add(cus.Id, cus.Login, cus.Imie, cus.Nazwisko, cus.Ulica, cus.NrDomu, cus.Miasto, cus.KodPocztowy, cus.Mail, cus.Telefon, cus.Fax);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView6.Sort(dataGridView6.Columns[3], ListSortDirection.Ascending);
            if (dataGridView6.RowCount > 0)
            {
                dataGridView6.Rows[0].Selected = true;
            }
        }

        private void FillDeliveries()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Delivery.Join(database.Provider,
                       del => del.DostawcaId,
                       prov => prov.Id,
                       (del, prov) => new { Id = del.Id, Dostawca = prov.Nazwa, Data = del.Data, DostawcaId = del.DostawcaId });
                dataGridView7.Rows.Clear();
                foreach (var del in data1)
                {
                    dataGridView7.Rows.Add(del.Id, del.Dostawca, del.Data, del.DostawcaId);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView7.Sort(dataGridView7.Columns[2], ListSortDirection.Descending);
            if (dataGridView7.RowCount > 0)
            {
                dataGridView7.Rows[0].Selected = true;
            }
        }

        private void FillAccounts()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Account;
                dataGridView4.Rows.Clear();
                foreach (var acc in data1)
                {
                    dataGridView4.Rows.Add(acc.Id, acc.NazwaUzytkownika, acc.Haslo, acc.Uprawnienia);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView4.Sort(dataGridView4.Columns[1], ListSortDirection.Ascending);
            if (dataGridView4.RowCount > 0)
            {
                dataGridView4.Rows[0].Selected = true;
            }
        }

        private void FillProviders()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Provider;
                dataGridView5.Rows.Clear();
                foreach (var prov in data1)
                {
                    dataGridView5.Rows.Add(prov.Id, prov.Nazwa, prov.Adres);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView5.Sort(dataGridView5.Columns[1], ListSortDirection.Ascending);
            if (dataGridView5.RowCount > 0)
            {
                dataGridView5.Rows[0].Selected = true;
            }
        }
        
        private void FillOrders()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Order.Join(database.OrderDetail,
                       ord => ord.ZamowienieId,
                       det => det.ZamowienieId,
                       (ord, det) => new { Id = ord.ZamowienieId, DataZamowienia = ord.DataZamowienia, DataZrealizowania = ord.DataZrealizowania, KlientId = ord.KlientId, ProduktId = det.ProduktId, Ilosc = det.Ilosc, MiastoDostawy = det.MiastoDostawy, KodPocztowyDostawy = det.KodPocztowyDostawy, AdresDostawy = det.AdresDostawy })
                       .Join(database.Customer,
                       ord => ord.KlientId,
                       cus => cus.KontoId,
                       (ord, cus) => new { Id = ord.Id, DataZamowienia = ord.DataZamowienia, DataZrealizowania = ord.DataZrealizowania, KlientId = ord.KlientId, ProduktId = ord.ProduktId, Ilosc = ord.Ilosc, MiastoDostawy = ord.MiastoDostawy, KodPocztowyDostawy = ord.KodPocztowyDostawy, AdresDostawy = ord.AdresDostawy, Imie = cus.Imie, Nazwisko = cus.Nazwisko })
                       .Join(database.Product,
                       ord => ord.ProduktId,
                       pro => pro.Id,
                       (ord, pro) => new { Id = ord.Id, DataZamowienia = ord.DataZamowienia, DataZrealizowania = ord.DataZrealizowania, KlientId = ord.KlientId, ProduktId = ord.ProduktId, Ilosc = ord.Ilosc, MiastoDostawy = ord.MiastoDostawy, KodPocztowyDostawy = ord.KodPocztowyDostawy, AdresDostawy = ord.AdresDostawy, Imie = ord.Imie, Nazwisko = ord.Nazwisko, NazwaProduktu = pro.Nazwa });
                dataGridView3.Rows.Clear();
                foreach (var ord in data1)
                {
                    dataGridView3.Rows.Add(ord.Id, ord.NazwaProduktu, ord.Ilosc, ord.DataZamowienia, ord.DataZrealizowania, ord.Imie + " " + ord.Nazwisko, ord.AdresDostawy + ", " + ord.KodPocztowyDostawy + " " + ord.MiastoDostawy, ord.ProduktId, ord.KlientId, ord.MiastoDostawy, ord.AdresDostawy, ord.KodPocztowyDostawy);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView3.Sort(dataGridView3.Columns[0], ListSortDirection.Descending);
            if (dataGridView3.RowCount > 0)
            {
                dataGridView3.Rows[0].Selected = true;
            }
        }


        private void FillCategory()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Category;
                dataGridView2.Rows.Clear();
                dataGridView2.Rows.Add(0, "<wszystko>");
                foreach (var cat in data1)
                {
                    dataGridView2.Rows.Add(cat.Id, cat.Nazwa);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView2.Sort(dataGridView2.Columns[1], ListSortDirection.Ascending);
            if (dataGridView2.RowCount > 0)
            {
                dataGridView2.Rows[0].Selected = true;
            }
        }
        private void FillProduct()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Product.Join(database.Price.Where(x => x.Do == null),
                       pro => pro.Id,
                       pri => pri.ProduktId,
                       (pro, pri) => new { Id = pro.Id, IdKategorii = pro.KategoriaId, Product = pro.Nazwa, Price = pri.Cena, IdCeny = pri.CenaId })
                       .Join(database.Category,
                       pro => pro.IdKategorii,
                       cat => cat.Id,
                       (pro, cat) => new { Id = pro.Id, Product = pro.Product, Kategoria = cat.Nazwa, Price = pro.Price, IdCeny = pro.IdCeny, IdKategorii = pro.IdKategorii })
                       .Join(database.Vat.Where(z => z.Do == null),
                       pro => pro.Id,
                       v => v.ProduktId,
                       (pro, v) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = v.WartoscVat, IdCeny = pro.IdCeny, IdKategorii = pro.IdKategorii, IdVat = v.Id })
                       .Join(database.Store,
                       pro => pro.Id,
                       s => s.ProduktId,
                       (pro, s) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = pro.Vat, Dostępnych = s.IloscDostepnych, Zamówionych = s.IloscZamowionych, IdKategorii = pro.IdKategorii, IdCeny = pro.IdCeny, IdVat = pro.IdVat });
                dataGridView1.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView1.Rows.Add(prod.Id, prod.Product, prod.Kategoria, prod.Price, prod.Vat, prod.Dostępnych, prod.Zamówionych, prod.IdKategorii, prod.IdCeny, prod.IdVat);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void FillProductCat(int idCat)
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Product.Join(database.Price.Where(x => x.Do == null),
                       pro => pro.Id,
                       pri => pri.ProduktId,
                       (pro, pri) => new { Id = pro.Id, IdKategorii = pro.KategoriaId, Product = pro.Nazwa, Price = pri.Cena })
                       .Join(database.Category.Where(x => x.Id == idCat),
                       pro => pro.IdKategorii,
                       cat => cat.Id,
                       (pro, cat) => new { Id = pro.Id, Product = pro.Product, Kategoria = cat.Nazwa, Price = pro.Price })
                       .Join(database.Vat.Where(z => z.Do == null),
                       pro => pro.Id,
                       v => v.ProduktId,
                       (pro, v) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = v.WartoscVat })
                       .Join(database.Store,
                       pro => pro.Id,
                       s => s.ProduktId,
                       (pro, s) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = pro.Vat, Dostępnych = s.IloscDostepnych, Zamówionych = s.IloscZamowionych });
                dataGridView1.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView1.Rows.Add(prod.Id, prod.Product, prod.Kategoria, prod.Price, prod.Vat, prod.Dostępnych, prod.Zamówionych);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void FillDeliveryDetails(int _dostawaId)
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Product.Join(database.DeliveryDetail.Where(x => x.DostawaId == _dostawaId),
                       pro => pro.Id,
                       del => del.ProduktId,
                       (pro, del) => new { Id = del.Id, Nazwa = pro.Nazwa, Netto = del.Netto, Vat = del.Vat, Sztuk = del.Sztuk, DostawaId = del.DostawaId, ProduktId = del.ProduktId});
                dataGridView8.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView8.Rows.Add(prod.Id, prod.Nazwa, prod.Netto, prod.Vat, prod.Sztuk, prod.DostawaId, prod.ProduktId);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd uzyskiwania danych z bazy");
            }
            finally
            {
                database.Dispose();
            }
            dataGridView8.Sort(dataGridView8.Columns[1], ListSortDirection.Ascending);
            if (dataGridView8.RowCount > 0)
            {
                dataGridView8.Rows[0].Selected = true;
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
        /*
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
        }*/

        public bool DeleteCategory(int _Id)
        {
            try
            {
                database = new ShopContext();
                var cat = database.Category.First(x => x.Id == _Id);
                database.Category.Remove(cat);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
            }
        }

        public bool DeleteCustomer(int _Id)
        {
            try
            {
                database = new ShopContext();
                var cust = database.Customer.First(x => x.Id == _Id);
                database.Customer.Remove(cust);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
            }
        }

        public bool DeleteProvider(int _Id)
        {
            try
            {
                database = new ShopContext();
                var prov = database.Provider.First(x => x.Id == _Id);
                database.Provider.Remove(prov);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
            }
        }

        public bool DeleteAccount(int _Id)
        {
            try
            {
                database = new ShopContext();
                var acc = database.Account.First(x => x.Id == _Id);
                database.Account.Remove(acc);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
            }
        }

        public bool DeleteDelivery(int _Id)
        {
            try
            {
                database = new ShopContext();
                var delivery = database.Delivery.First(x => x.Id == _Id);
                database.Delivery.Remove(delivery);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
            }
        }

        public bool DeleteDetail(int _id)
        {
            try
            {
                database = new ShopContext();
                var detail = database.DeliveryDetail.First(x => x.Id == _id);
                database.DeliveryDetail.Remove(detail);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
            }
        }

        public bool DeleteOrder(int _id)
        {
            try
            {
                database = new ShopContext();
                var ord = database.Order.First(x => x.ZamowienieId == _id);
                database.Order.Remove(ord);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
            }
        }

        public bool DeleteProduct(int _Id)
        {
            try
            {
                database = new ShopContext();
                var pro = database.Product.First(x => x.Id == _Id);
                database.Product.Remove(pro);
                database.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
            finally
            {
                database.Dispose();
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
            if (dataGridView2.CurrentRow != null && (int)dataGridView2.CurrentRow.Cells[0].Value!=0)
            {
                idCat = (int)dataGridView2.SelectedCells[0].Value;
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

        private void button15_Click(object sender, EventArgs e)
        {
            AddingProvWindow adding = new AddingProvWindow();
            adding.ShowDialog();
            if (adding.DialogResult == DialogResult.OK)
            {
                FillProviders();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczonego dostawcę?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView5.CurrentRow != null && DeleteProvider((int)dataGridView5.CurrentRow.Cells[0].Value))
            {
                dataGridView5.Rows.RemoveAt(dataGridView5.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem dostawcy");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            FillProviders();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView5.CurrentRow != null)
            {
                EditingProvWindow editing = new EditingProvWindow(dataGridView5.CurrentRow);
                editing.ShowDialog();
                if (editing.DialogResult == DialogResult.OK)
                {
                    FillProviders();
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FillAccounts();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            AddingAccWindow adding = new AddingAccWindow();
            adding.ShowDialog();
            if (adding.DialogResult == DialogResult.OK)
            {
                FillAccounts();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczone konto?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView4.CurrentRow != null && info.privilages < 3 && (int)dataGridView4.CurrentRow.Cells[0].Value != info.id && (int)dataGridView4.CurrentRow.Cells[3].Value>=info.privilages)
            {
                MessageBox.Show("Masz za małe uprawnienia aby usunąć to konto");
                return;
            }
            if (dataGridView4.CurrentRow != null && info.id == (int)dataGridView4.CurrentRow.Cells[0].Value && info.privilages == 3)
            {
                MessageBox.Show("Nie możesz usunąć swojego konta");
                return;
            }
            if (dataGridView4.CurrentRow != null && DeleteAccount((int)dataGridView4.CurrentRow.Cells[0].Value))
            {
                dataGridView4.Rows.RemoveAt(dataGridView4.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem konta");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow != null)
            {
                EditingAccWindow editing = new EditingAccWindow(dataGridView4.CurrentRow);
                editing.ShowDialog();
                if (editing.DialogResult == DialogResult.OK)
                {
                    FillAccounts();
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            AddingCustWindow customerAdd = new AddingCustWindow();
            customerAdd.ShowDialog();
            if (customerAdd.DialogResult == DialogResult.OK)
            {
                FillCustomers();
            }

        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (dataGridView6.CurrentRow != null)
            {
                EditingCustWindow editing = new EditingCustWindow(dataGridView6.CurrentRow);
                editing.ShowDialog();

                    if (editing.DialogResult == DialogResult.OK)
                    {
                        FillCustomers();
                    }
            }
            else
            {
                MessageBox.Show("fuck3");
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczonego klienta?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView6.CurrentRow != null && DeleteCustomer((int)dataGridView6.CurrentRow.Cells[0].Value))
            {
                dataGridView6.Rows.RemoveAt(dataGridView6.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem klienta");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            FillCustomers();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            AddingDeliveryWindow adding = new AddingDeliveryWindow();
            adding.ShowDialog();
            if (adding.DialogResult == DialogResult.OK)
            {
                FillDeliveries();
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (dataGridView7.CurrentRow != null)
            {
                EditingDeliveryWindow adding = new EditingDeliveryWindow(dataGridView7.CurrentRow);
                adding.ShowDialog();
                if (adding.DialogResult == DialogResult.OK)
                {
                    FillDeliveries();
                }
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczoną dostawę?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView7.CurrentRow != null && DeleteDelivery((int)dataGridView7.CurrentRow.Cells[0].Value))
            {
                dataGridView7.Rows.RemoveAt(dataGridView7.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem dostawy");
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            FillDeliveries();

        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int deliveryId = 0;
            if (dataGridView7.CurrentRow != null)
            {
                deliveryId = (int)dataGridView7.SelectedCells[0].Value;
                FillDeliveryDetails(deliveryId);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FilterProducts();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (dataGridView7.CurrentRow != null)
            {
                AddingDetailsWindow adding = new AddingDetailsWindow((int)dataGridView7.CurrentRow.Cells[0].Value);
                adding.ShowDialog();
                if (adding.DialogResult == DialogResult.OK)
                {
                    FillDeliveryDetails((int)dataGridView7.CurrentRow.Cells[0].Value);
                }
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczony produkt z zaznaczonej dostawy?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView7.CurrentRow != null && dataGridView8.CurrentRow != null && DeleteDetail((int)dataGridView8.CurrentRow.Cells[0].Value))
            {
                dataGridView8.Rows.RemoveAt(dataGridView8.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem produktu z dostawy");
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {

            if (dataGridView7.CurrentRow != null && dataGridView8.CurrentRow != null)
            {
                EditingDetailsWindow adding = new EditingDetailsWindow((int)dataGridView7.CurrentRow.Cells[0].Value, dataGridView8.CurrentRow);
                adding.ShowDialog();
                if (adding.DialogResult == DialogResult.OK)
                {
                    FillDeliveryDetails((int)dataGridView7.CurrentRow.Cells[0].Value);
                }
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            FillOrders();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AddingOrderWindow adding = new AddingOrderWindow();
            adding.ShowDialog();
            if (adding.DialogResult == DialogResult.OK)
            {
                FillOrders();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null)
            {
                EditingOrderWindow editing = new EditingOrderWindow(dataGridView3.CurrentRow);
                editing.ShowDialog();
                if (editing.DialogResult == DialogResult.OK)
                {
                    FillOrders();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            ConfirmWindow confirm = new ConfirmWindow("Czy na pewno usunąć zaznaczone zamówienie?");
            confirm.ShowDialog();
            if (confirm.DialogResult != DialogResult.Yes)
            {
                return;
            }
            if (dataGridView3.CurrentRow != null && DeleteOrder((int)dataGridView3.CurrentRow.Cells[0].Value))
            {
                dataGridView3.Rows.RemoveAt(dataGridView3.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Wystąpił błąd z usunięciem zamówienia");
            }
        }
    }
}
