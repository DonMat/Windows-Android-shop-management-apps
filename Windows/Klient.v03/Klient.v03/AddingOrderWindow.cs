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
    public partial class AddingOrderWindow : Form
    {
        private ShopContext database;

        private void FillProducts()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Product.
                    Join(
                        database.Store, 
                        prodI => prodI.Id, 
                        strId => strId.ProduktId,
                        (prodI, strId) => new {Id = prodI.Id, Nazwa = prodI.Nazwa, Ilosc = strId.IloscDostepnych}) ;

                dataGridView1.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView1.Rows.Add(prod.Id, prod.Nazwa, prod.Ilosc);
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
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void FillCustomers()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Customer;
                dataGridView2.Rows.Clear();
                foreach (var cus in data1)
                {
                    dataGridView2.Rows.Add(cus.KontoId, cus.Imie + " " + cus.Nazwisko, cus.Ulica, cus.KodPocztowy, cus.Miasto);
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

        public AddingOrderWindow()
        {
            InitializeComponent();
            FillCustomers();
            FillProducts();
        }

        public int InsertNewOrder(DateTime _zamowione, int _klientId)
        {
            try
            {
                Order OrderToInsert = new Order()
                {
                    DataZamowienia = _zamowione,
                    KlientId = _klientId
                };
                database.Order.Add(OrderToInsert);
                database.SaveChanges();
                return OrderToInsert.ZamowienieId;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return -1;
            }
        }

        public bool InsertNewOrderDetail(int _zamowienieId, int _produktId, int _ile, string _miasto, string _adres, string _kod)
        {
            try
            {
                OrderDetail OrderDetailToInsert = new OrderDetail()
                {
                    ZamowienieId = _zamowienieId,
                    ProduktId = _produktId,
                    Ilosc = _ile,
                    MiastoDostawy = _miasto,
                    AdresDostawy = _adres,
                    KodPocztowyDostawy = _kod
                };
                database.OrderDetail.Add(OrderDetailToInsert);
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int prodId;
            int custId;
            int count;
            try
            {
                prodId = (int)dataGridView1.SelectedCells[0].Value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Niepoprawny produkt");
                return;
            }
            try
            {
                custId = (int)dataGridView2.SelectedCells[0].Value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Niepoprawny klient");
                return;
            }
            try
            {
                count = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox1.Text + " nie jest poprawną liczbą");
                return;
            }
            if (count < 0)
            {
                MessageBox.Show(textBox1.Text + " nie jest poprawną wartością");
                return;
            }
            try
            {
                database = new ShopContext();
                int ordId = InsertNewOrder(DateTime.Now, custId);
                if ((ordId > 0) && (InsertNewOrderDetail(ordId, prodId, count, textBox4.Text, textBox2.Text, textBox3.Text)==true))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();

                    Store pr = database.Store.First(x => x.ProduktId == prodId);
                    pr.IloscDostepnych -= count;
                    pr.IloscZamowionych += count;
                    database.SaveChanges();
                    
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z dodawaniem produktu\n" + custId +"\n"+ ordId + "\n " + prodId + "\n " + count + "\n " + textBox4.Text + "\n " + textBox2.Text + "\n " + textBox3.Text);
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

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView2.SelectedCells[2].Value+"";
            textBox3.Text = dataGridView2.SelectedCells[3].Value + "";
            textBox4.Text = dataGridView2.SelectedCells[4].Value + "";
        }
    }
}
