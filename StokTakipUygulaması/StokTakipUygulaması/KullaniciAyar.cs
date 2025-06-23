using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StokTakipUygulaması
{
    public partial class YöneticiAyar : Form
    {
        private string kullaniciGorevi;
        public YöneticiAyar(string gorev)
        {
            InitializeComponent();
            kullaniciGorevi = gorev;
        }

        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");



        private void KullanicilariListele()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT kullaniciadi, sifre, adisoyadi, gorev FROM kullanicilar", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();

        }



        private void KullaniciAyar_Load(object sender, EventArgs e)
        {
           
            KullanicilariListele();
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                txtKadi.Text = satir.Cells["kullaniciadi"].Value?.ToString();
                txtSifre.Text = satir.Cells["sifre"].Value?.ToString();
                txtAdsoyad.Text = satir.Cells["adisoyadi"].Value?.ToString();
                cmbGorev.Text = satir.Cells["gorev"].Value?.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (kullaniciGorevi.ToLower() != "admin")
            {
                MessageBox.Show("Bu işlemi yapmak için yetkiniz yok.", "Yetkisiz İşlem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string girilenKadi = txtKadi.Text.Trim();

          
            baglanti.Open();
            SqlCommand kontrolKomut = new SqlCommand("SELECT COUNT(*) FROM kullanicilar WHERE kullaniciadi = @kadi", baglanti);
            kontrolKomut.Parameters.AddWithValue("@kadi", girilenKadi);

            int ayniKullaniciSayisi = (int)kontrolKomut.ExecuteScalar();

            if (ayniKullaniciSayisi > 0)
            {
                baglanti.Close();
                MessageBox.Show("Bu kullanıcı adı zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            SqlCommand komut = new SqlCommand("INSERT INTO kullanicilar (kullaniciadi, sifre, adisoyadi, gorev) VALUES (@kadi, @sifre, @adsoyad, @gorev)", baglanti);
            komut.Parameters.AddWithValue("@kadi", girilenKadi);
            komut.Parameters.AddWithValue("@sifre", txtSifre.Text.Trim());
            komut.Parameters.AddWithValue("@adsoyad", txtAdsoyad.Text.Trim());
            komut.Parameters.AddWithValue("@gorev", cmbGorev.Text.Trim());

            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Kullanıcı başarıyla eklendi.");
            KullanicilariListele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
           
            if (kullaniciGorevi.ToLower() != "admin")
            {
                MessageBox.Show("Bu işlemi yapmak için yetkiniz yok.", "Yetkisiz İşlem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            if (string.IsNullOrEmpty(txtKadi.Text))
            {
                MessageBox.Show("Lütfen silmek istediğiniz kullanıcıyı seçiniz.");
                return;
            }

            DialogResult sonuc = MessageBox.Show("Seçilen kullanıcıyı silmek istediğinize emin misiniz?", "Kullanıcı Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM kullanicilar WHERE kullaniciadi = @kadi", baglanti);
                komut.Parameters.AddWithValue("@kadi", txtKadi.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Kullanıcı başarıyla silindi.");
                KullanicilariListele(); 
            }
        }

        private void btnDegistir_Click(object sender, EventArgs e)
        {
            if (txtKadi.Text == "")
            {
                MessageBox.Show("Lütfen bir kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Şifre boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                baglanti.Open();
                string sorgu = "UPDATE kullanicilar SET sifre = @sifre WHERE kullaniciadi = @kullaniciadi";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@sifre", txtSifre.Text);
                komut.Parameters.AddWithValue("@kullaniciadi", txtKadi.Text);

                int sonuc = komut.ExecuteNonQuery();
                baglanti.Close();

                if (sonuc > 0)
                {
                    MessageBox.Show("Şifre başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    KullanicilariListele(); 
                }
                else
                {
                    MessageBox.Show("Şifre güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtKadi.Text))
            {
                MessageBox.Show("Lütfen güncellenecek kullanıcıyı seçiniz.");
                return;
            }

            try
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True"))
                {
                    baglanti.Open();

                    string sorgu = "UPDATE kullanicilar SET sifre = @sifre, adisoyadi = @adsoyad, gorev = @gorev WHERE kullaniciadi = @kullaniciadi";

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@sifre", txtSifre.Text.Trim());
                        komut.Parameters.AddWithValue("@adsoyad", txtAdsoyad.Text.Trim());
                        komut.Parameters.AddWithValue("@gorev", cmbGorev.Text.Trim());
                        komut.Parameters.AddWithValue("@kullaniciadi", txtKadi.Text.Trim());

                        int etkilenenSatir = komut.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("Kullanıcı başarıyla güncellendi.");
                            KullanicilariListele();  
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme yapılamadı.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }

        }
    }
}
