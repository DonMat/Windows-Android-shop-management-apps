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
    public partial class Form1 : Form
    {
        private void FillOrders()
        {
            using (var db = new ShopContext()){
                
            }
        }
        private void FillProducts()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Products.Join(db.Prices.Where(x => x.Do == null),
                       pro => pro.Id,
                       pri => pri.Id_produktu,
                       (pro, pri) => new { Id = pro.Id, IdKategorii = pro.Kategoria_id, Product = pro.Nazwa, Price = pri.Cena })
                       .Join(db.Category,
                       pro => pro.IdKategorii,
                       cat => cat.Id,
                       (pro, cat) => new { Id = pro.Id, Product = pro.Product, Kategoria = cat.Nazwa, Price = pro.Price })
                       .Join(db.Vat.Where(z => z.Do == null),
                       pro => pro.Id,
                       v => v.Id_produktu,
                       (pro, v) => new { Id = pro.Id, Product = pro.Product, Kategoria = pro.Kategoria, Price = pro.Price, Vat = v.Wartosc_vat });
                dataGridView1.Rows.Clear();
                foreach (var prod in data1)
                {
                    dataGridView1.Rows.Add(prod.Id, prod.Product, prod.Kategoria, prod.Price, prod.Vat);
                }
            }
        }

        public Form1()
        {
            this.Visible = false;
            Form2 logging = new Form2();
            logging.ShowDialog();
            if (logging.DialogResult != DialogResult.OK)
                Environment.Exit(0);

            InitializeComponent();
            FillProducts();
            
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
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

        public bool DeleteCategory(string _Nazwa)
        {
            using (var db = new ShopContext())
            {
                try
                {
                    var cat = db.Category.First(x => x.Nazwa.Equals(_Nazwa));
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

        private void button2_Click(object sender, EventArgs e)
        {
            if(InsertNewCategory("a"))
                MessageBox.Show("d");
            else
                MessageBox.Show("z");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DeleteCategory("d"))
                MessageBox.Show("P");
            else
                MessageBox.Show("z");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 adding = new Form4();
            adding.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 editing = new Form5((int)dataGridView1.CurrentRow.Cells[0].Value);
            editing.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
