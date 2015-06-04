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
    public partial class EditingDetailsWindow : Form
    {
        private ShopContext database;

        DataGridViewRow row;
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

        public EditingDetailsWindow(int _delivId, DataGridViewRow _row)
        {
            delivId = _delivId;
            row = _row;
            InitializeComponent();
            FillProducts();
            textBox1.Text = ((double)row.Cells[2].Value).ToString();
            textBox2.Text = ((int)row.Cells[3].Value).ToString();
            textBox3.Text = ((int)row.Cells[4].Value).ToString();
        }

        public bool EditDetails(int _id, int _dostawaId, int _produktId, double _netto, int _vat, int _sztuk)
        {
            try
            {
                DeliveryDetail details = database.DeliveryDetail.First(x => x.Id == _id);
                if (details.DostawaId != _dostawaId)
                {
                    details.DostawaId = _dostawaId;
                }
                if (details.ProduktId != _produktId)
                {
                    details.ProduktId = _produktId;
                }
                if (details.Netto != _netto)
                {
                    details.Netto = _netto;
                }
                if (details.Vat != _vat)
                {
                    details.Vat = _vat;
                }
                if (details.Sztuk != _sztuk)
                {
                    details.Sztuk = _sztuk;
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
                if (EditDetails((int)row.Cells[0].Value, delivId, prodId, price, vat, count))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z edytowaniem produktu");
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

        private void EditingDetailsWindow_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                if ((int)r.Cells[0].Value == (int)row.Cells[6].Value)
                {
                    r.Selected = true;
                    break;
                }
            }
        }
    }
}
