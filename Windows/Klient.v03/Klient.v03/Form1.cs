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
            dataGridView2.Rows[0].Selected = true;
        }
        private void FillProduct()
        {
            using (var db = new ShopContext())
            {
                var data1 = db.Product.Join(db.Price.Where(x => x.Do == null),
                       pro => pro.Id,
                       pri => pri.ProduktId,
                       (pro, pri) => new { Id = pro.Id, IdKategorii = pro.KategoriaId, Product = pro.Nazwa, Price = pri.Cena })
                       .Join(db.Category,
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
            dataGridView1.Rows[0].Selected = true;
        }

        public Form1()
        {
            this.Visible = false;
            Form2 logging = new Form2();
            logging.ShowDialog();
            if (logging.DialogResult != DialogResult.OK)
                Environment.Exit(0);

            InitializeComponent();
            FillProduct();
            FillCategory();
            
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
            if(InsertNewCategory("a"))
                MessageBox.Show("d");
            else
                MessageBox.Show("z");
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 adding = new Form4(dataGridView1);
            adding.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 editing = new Form5((int)dataGridView1.CurrentRow.Cells[0].Value,
                (string)dataGridView1.CurrentRow.Cells[1].Value,
                (string)dataGridView1.CurrentRow.Cells[2].Value,
                (double)dataGridView1.CurrentRow.Cells[3].Value,
                (int)dataGridView1.CurrentRow.Cells[4].Value,
                (int)dataGridView1.CurrentRow.Cells[5].Value);
            editing.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (DeleteProduct((int)dataGridView1.CurrentRow.Cells[0].Value))
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

        }

        private void button9_Click(object sender, EventArgs e)
        {
            FillProduct();
            FillCategory();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
