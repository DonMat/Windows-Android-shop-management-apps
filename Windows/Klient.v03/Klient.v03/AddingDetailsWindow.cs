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
    public partial class AddingDetailsWindow : Form
    {
        private ShopContext database;
        private int delivId;

        private void FillProducts()
        {
            try
            {
                database = new ShopContext();
                var data1 = database.Product;
                dataGridView1.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView1.Rows.Add(prod.Id, prod.Nazwa);
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

        public bool InsertNewDetail(int _dostawaId, int _produktId, double _netto, int _vat, int _sztuk)
        {
            try
            {
                DeliveryDetail DetailToInsert = new DeliveryDetail()
                {
                    DostawaId = _dostawaId,
                    ProduktId = _produktId,
                    Netto = _netto,
                    Vat = _vat,
                    Sztuk = _sztuk
                };
                database.DeliveryDetail.Add(DetailToInsert);
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry(e.Source, e.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }
        }

        public AddingDetailsWindow(int _delivId)
        {
            delivId = _delivId;
            InitializeComponent();
            FillProducts();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int prodId;
            double price;
            int vat;
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
                price = Convert.ToDouble(textBox1.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox1.Text + " nie jest poprawną ceną");
                return;
            }
            try
            {
                vat = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox2.Text + " nie jest poprawną stawką vat");
                return;
            }
            try
            {
                count = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show(textBox3.Text + " nie jest poprawną liczbą");
                return;
            }
            if (count < 0)
            {
                MessageBox.Show(textBox3.Text + " nie jest poprawną wartością");
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
                if (InsertNewDetail(delivId, prodId, price, vat, count))
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
