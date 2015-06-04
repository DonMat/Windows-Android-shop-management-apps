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
        private ShopContext database;
        private void FillCategories()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Category;
                dataGridView1.Rows.Clear();
                foreach (var cat in data1)
                {
                    dataGridView1.Rows.Add(cat.Id, cat.Nazwa);
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
        }

        public AddingWindow(/*DataGridView _datagrid*/)
        {
            //datagrid = _datagrid;
            InitializeComponent();
            FillCategories();
        }

        public int InsertNewProduct(string _Nazwa, int _KategoriaId)
        {
            try
            {
                Product ProductToInsert = new Product()
                {
                    Nazwa = _Nazwa,
                    KategoriaId = _KategoriaId
                };
                database.Product.Add(ProductToInsert);
                database.SaveChanges();
                return ProductToInsert.Id;
            }
            catch(Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return -1;
            }
        }

        public bool InsertNewPrice(int _ProduktId, DateTime _Od, DateTime _Do, double _Cena)
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

                database.Price.Add(PriceToInsert);
                return true;
            }
            catch(Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
        }

        public bool InsertNewPrice(int _ProduktId, DateTime _Od, double _Cena)
        {
            try
            {
                Price PriceToInsert = new Price()
                {
                    ProduktId = _ProduktId,
                    Od = _Od,
                    Cena = _Cena
                };

                database.Price.Add(PriceToInsert);
                return true;
            }
            catch(Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
        }

        public bool InsertNewVat(int _ProduktId, DateTime _Od, DateTime _Do, int _WartoscVat)
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

                database.Vat.Add(VatToInsert);
                return true;
            }
            catch(Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
        }

        public bool InsertNewVat(int _ProduktId, DateTime _Od, int _WartoscVat)
        {
                try
                {
                    Vat VatToInsert = new Vat()
                    {
                        ProduktId = _ProduktId,
                        Od = _Od,
                        WartoscVat = _WartoscVat
                    };

                    database.Vat.Add(VatToInsert);
                    return true;
                }
                catch(Exception e)
                {
                    System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                    return false;
                }
        }

        public bool InsertNewStore(int _ProduktId, int _IloscDostepnych, int _IloscZamowionych)
        {
            try
            {
                Store StoreToInsert = new Store()
                {
                    ProduktId = _ProduktId,
                    IloscDostepnych = _IloscDostepnych,
                    IloscZamowionych = _IloscZamowionych
                };

                database.Store.Add(StoreToInsert);
                return true;
            }
            catch(Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
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
                cat = (int)dataGridView1.SelectedCells[0].Value;
            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Niepoprawna kategoria");
                return;
            }
            try
            {
                price = Convert.ToDouble(textBox2.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox2.Text+" nie jest poprawną ceną");
                return;
            }
            try
            {
                vat = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox3.Text + " nie jest poprawną stawką vat");
                return;
            }
            try
            {
                availble = Convert.ToInt32(textBox4.Text);
            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox4.Text + " nie jest poprawną liczbą");
                return;
            }
            if (availble < 0)
            {
                MessageBox.Show(textBox4.Text + " nie jest poprawną wartością");
                return;
            }
            if (price < 0)
            {
                MessageBox.Show("Cena powinna być dodatnia");
                return;
            }
            try
            {
                database = new ShopContext();
                int productId = InsertNewProduct(textBox1.Text, cat);
                if (productId > 0 && InsertNewPrice(productId, DateTime.Now, price) &&
                    InsertNewVat(productId, DateTime.Now, vat) &&
                    InsertNewStore(productId, availble, 0))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z dodawaniem produktu");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Błąd zapisu do bazy");
                return;
            }
            finally
            {
                database.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
