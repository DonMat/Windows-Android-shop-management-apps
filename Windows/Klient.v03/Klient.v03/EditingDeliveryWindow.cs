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
    public partial class EditingDeliveryWindow : Form
    {
        private ShopContext database;
        DataGridViewRow row;
        public EditingDeliveryWindow(DataGridViewRow _row)
        {
            row = _row;
            InitializeComponent();
            FillProviders();
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

        public bool EditDelivery(int _id, int _dostawcaId)
        {
            try
            {
                Delivery delivery = database.Delivery.First(x => x.Id == _id);
                if (!delivery.DostawcaId.Equals(_dostawcaId))
                {
                    delivery.DostawcaId = _dostawcaId;
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
            int prov;
            try
            {
                prov = (int)dataGridView5.SelectedCells[0].Value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ex.Source, ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                MessageBox.Show("Niepoprawny dostawca");
                return;
            }
            try
            {
                database = new ShopContext();
                if (EditDelivery((int)row.Cells[0].Value, prov))
                {
                    database.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił problem z dodawaniem dostawy");
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

        private void EditingDeliveryWindow_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView5.Rows)
            {
                if ((int)r.Cells[0].Value == (int)row.Cells[3].Value)
                {
                    r.Selected = true;
                    break;
                }
            }
        }
    }
}