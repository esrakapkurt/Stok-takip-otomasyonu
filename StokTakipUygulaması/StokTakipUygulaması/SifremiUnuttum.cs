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
    public partial class SifremiUnuttum : Form
    {
        public SifremiUnuttum()
        {
            InitializeComponent();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string yeniSifre = txtSifre.Text;
            string sifreTekrar = txtSifreTekrar.Text;

            // 1. Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(kullaniciAdi) ||
                string.IsNullOrWhiteSpace(yeniSifre) ||
                string.IsNullOrWhiteSpace(sifreTekrar))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

          
            if (yeniSifre != sifreTekrar)
            {
                MessageBox.Show("Şifre ve Şifre Tekrar uyuşmuyor!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            
            kullanici k = new kullanici();
            bool basarili = k.SifreGuncelle(kullaniciAdi, yeniSifre);

            if (basarili)
            {
                MessageBox.Show("Şifreniz başarıyla güncellendi.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Kullanıcı bulunamadı.");
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
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
