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
    public partial class KullaniciGirisi : Form
    {
        public KullaniciGirisi()
        {
            InitializeComponent();
        }

        private void lblCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linklblSifremiUnuttum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SifremiUnuttum sifremiunuttum = new SifremiUnuttum();
            sifremiunuttum.ShowDialog();
        }

        private void lblUyeOl_Click(object sender, EventArgs e)
        {
            YeniKullanici yenikullanici = new YeniKullanici();
            yenikullanici.ShowDialog();
        }
       
        
        kullanici K = new kullanici();

       
        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSİfre.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            kullanici K = new kullanici();

            if (K.GirisYap(kullaniciAdi, sifre))
            {
                MessageBox.Show("Giriş başarılı!");

                
                AnaSayfa anaForm = new AnaSayfa(K.Gorev);
                anaForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSifreGoster.Checked)
            {
                txtSİfre.PasswordChar = '\0'; 
            }
            else
            {
                txtSİfre.PasswordChar = '*'; 
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
