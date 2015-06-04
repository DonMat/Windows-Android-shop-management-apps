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
    public partial class EditingOrderWindow : Form
    {
        private ShopContext database;
        DataGridViewRow row;
        private int bylo;
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
                    (prodI, strId) => new { Id = prodI.Id, Nazwa = prodI.Nazwa, Ilosc = strId.IloscDostepnych });

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
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
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
                    dataGridView2.Rows.Add(cus.KontoId, cus.Imie + " " + cus.Nazwisko);
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

        public EditingOrderWindow(DataGridViewRow _row)
        {
            row = _row;
            InitializeComponent();
            FillCustomers();
            FillProducts();

            bylo = Convert.ToInt32(row.Cells[2].Value);

            if (row.Cells[4].Value != null)
                checkBox1.Checked = true;
            textBox1.Text = ((int)row.Cells[2].Value).ToString();
            textBox2.Text = ((string)row.Cells[10].Value);
            textBox3.Text = ((string)row.Cells[11].Value);
            textBox4.Text = ((string)row.Cells[9].Value);

        }

        public bool EditOrder(int _zamowienieId, DateTime _zrealizowane, int _klientId)
        {
            try
            {
                Order ord = database.Order.First(x => x.ZamowienieId == _zamowienieId);
                if (ord.DataZrealizowania == null && checkBox1.Checked == true)
                {
                    ord.DataZrealizowania = _zrealizowane;
                }
                else if (ord.DataZrealizowania != null && checkBox1.Checked == false)
                {
                    ord.DataZrealizowania = null;
                }
                if (ord.KlientId != _klientId)
                {
                    ord.KlientId = _klientId;
                }
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
        }

        public bool EditDetail(int _zamowienieId, int _ile, string _miasto, string _adres, string _kod)
        {
            try
            {
                OrderDetail ord = database.OrderDetail.First(x => x.ZamowienieId == _zamowienieId);
                //if (ord.ProduktId != _produktId)
                //{
                //    ord.ProduktId = _produktId;
                //}
                if (ord.Ilosc != _ile)
                {
                    ord.Ilosc = _ile;
                }
                if (ord.MiastoDostawy != _miasto)
                {
                    ord.MiastoDostawy = _miasto;
                }
                if (ord.AdresDostawy != _adres)
                {
                    ord.AdresDostawy = _adres;
                }
                if (ord.KodPocztowyDostawy != _kod)
                {
                    ord.KodPocztowyDostawy = _kod;
                }
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
                    //SelectedCells[0].Value;
               // MessageBox.Show(prodId + "");
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
                //SelectedCells[0].Value;
                //MessageBox.Show(custId+"");
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
            //try
            {
                database = new ShopContext();
                int ordId = (int)row.Cells[0].Value;
                if (EditOrder(ordId, DateTime.Now, custId) && EditDetail(ordId, count, textBox4.Text, textBox2.Text, textBox3.Text))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();

                    Store pr = database.Store.First(x => x.ProduktId == prodId);

                    int diff = count - bylo;

                    pr.IloscDostepnych -= diff;
                    pr.IloscZamowionych += diff;
                    database.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z edytowaniem produktu");
                }
            }
            //catch (Exception ex)
            {
            //    System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
            //    MessageBox.Show("Błąd zapisu do bazy");
            //    return;
            }
            //finally
            {
            //    database.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditingOrderWindow_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                if ((int)r.Cells[0].Value == (int)row.Cells[7].Value)
                {
                    r.Selected = true;
                    break;
                }
            }
            foreach (DataGridViewRow r in this.dataGridView2.Rows)
            {
                if ((int)r.Cells[0].Value == (int)row.Cells[8].Value)
                {
                    r.Selected = true;
                    break;
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView2.SelectedCells[2].Value + "";
            textBox3.Text = dataGridView2.SelectedCells[3].Value + "";
            textBox4.Text = dataGridView2.SelectedCells[4].Value + "";
        }
    }
}
