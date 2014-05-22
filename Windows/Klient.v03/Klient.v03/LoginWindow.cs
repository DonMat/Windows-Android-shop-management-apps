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
    public partial class LoginWindow : Form
    {
        public AccountInfo info;
        public LoginWindow(AccountInfo _info)
        {
            info = _info;
            InitializeComponent();
            AcceptButton = button2;
            DialogResult = DialogResult.Cancel;
            //MessageBox.Show(CalculateMD5Hash("no"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterWindow registration = new RegisterWindow();
            registration.ShowDialog();
        }

        public string CalculateMD5Hash(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = false;
            bool internet = true;
            using (var db = new ShopContext())
            {
                try
                {
                    foreach (var entry in db.Account)
                    {
                        if (entry.NazwaUzytkownika == textBox1.Text && entry.Haslo == CalculateMD5Hash(maskedTextBox1.Text).ToLower() && entry.Uprawnienia >= 2)
                        {
                            info.id = entry.Id;
                            info.login = entry.NazwaUzytkownika;
                            info.privilages = entry.Uprawnienia;
                            this.DialogResult = DialogResult.OK;
                            flag = true;
                        }
                    }
                }
                catch (Exception)
                {
                    internet = false;
                    MessageBox.Show("Błąd w logowaniu");
                }
            }
            if (flag)
            {
                this.Close();
            }
            else if(internet)
            {
                MessageBox.Show("Niepoprawy login/hasło lub zbyt małe uprawnienia");
            }
            internet = true;
        }
    }
}
