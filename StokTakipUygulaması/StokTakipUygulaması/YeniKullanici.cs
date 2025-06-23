using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTakipUygulaması
{
    public partial class YeniKullanici : Form
    {
        public YeniKullanici()
        {
            InitializeComponent();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void btnEkle_Click(object sender, EventArgs e)
        {
            
            kullanici k = new kullanici();
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;
            string adSoyad = txtAdSoyad.Text;
            string gorev = txtGorev.Text;
            if (k.KullaniciVarMi(kullaniciAdi))
            {
                MessageBox.Show("Bu kullanıcı adı zaten kayıtlı. Lütfen farklı bir kullanıcı adı girin.");
                return;
            }

            k.YeniKullanici(kullaniciAdi, sifre, adSoyad, gorev);
            MessageBox.Show("Kullanıcı başarıyla eklendi.");
            this.Close(); 
        }

        private void chkSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSifreGoster.Checked)
            {
                txtSifre.PasswordChar = '\0'; 
            }
            else
            {
                txtSifre.PasswordChar = '*'; 
            }
        }

        private void lblCikis_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
