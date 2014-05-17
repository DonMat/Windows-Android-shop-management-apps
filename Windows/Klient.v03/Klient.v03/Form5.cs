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
    public partial class Form5 : Form
    {
        int id;
        string kategoria;
        string nazwa;
        double cena;
        int vat;
        int dostepnych;
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
        public Form5(int _Id, string _Nazwa, string _Kategoria, double _Cena, int _Vat, int _Dostepnych)
        {
            id = _Id;
            kategoria = _Kategoria;
            nazwa = _Nazwa;
            cena = _Cena;
            vat = _Vat;
            dostepnych = _Dostepnych;
            InitializeComponent();
            textBox1.Text = _Nazwa;
            textBox2.Text = _Cena.ToString();
            textBox3.Text = _Vat.ToString();
            textBox4.Text = _Dostepnych.ToString();
            FillCategories();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals(kategoria))
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        public bool EditProduct()
        {
            using (var db = new ShopContext())
            {
                try
                {
                    Product Prod = db.Product.First(x => x.Id==id);
                    if (!nazwa.Equals(textBox1.Text))
                    {
                        Prod.Nazwa = textBox1.Text;
                        if (Prod.KategoriaId != (int)dataGridView1.CurrentRow.Cells[1].Value)
                        {
                            Prod.KategoriaId = (int)dataGridView1.CurrentRow.Cells[1].Value;
                        }
                        db.SaveChanges();
                    }
                    if (Prod.KategoriaId != (int)dataGridView1.CurrentRow.Cells[1].Value)
                    {
                        Prod.KategoriaId = (int)dataGridView1.CurrentRow.Cells[1].Value;
                        db.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool EditPrice(int _ProduktId, DateTime _Od, DateTime _Do, double _Cena)
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
