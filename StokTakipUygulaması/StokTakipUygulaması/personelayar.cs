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
    public partial class personelayar : Form
    {
        public personelayar()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        DataSet daset = new DataSet();
        private void btnAra_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKadi.Text.Trim();

            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                string sorgu = "SELECT sifre, adisoyadi FROM kullanicilar WHERE LOWER(kullaniciadi) = LOWER(@kullaniciadi)";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);

                SqlDataReader reader = komut.ExecuteReader();

                if (reader.Read())
                {
                    txtSifre.Text = reader["sifre"].ToString();
                    txtAdsoyad.Text = reader["adisoyadi"].ToString();
                }
                else
                {
                    MessageBox.Show("Kullanıcı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSifre.Clear();
                    txtAdsoyad.Clear();
                }

                reader.Close();

                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
            catch (Exception ex)
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();

                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKadi.Text))
            {
                MessageBox.Show("Lütfen güncellenecek kullanıcıyı seçiniz veya kullanıcı adını giriniz.");
                return;
            }

            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                string sorgu = "UPDATE kullanicilar SET sifre = @sifre, adisoyadi = @adsoyad WHERE kullaniciadi = @kullaniciadi";

                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@sifre", txtSifre.Text.Trim());
                komut.Parameters.AddWithValue("@adsoyad", txtAdsoyad.Text.Trim());
                komut.Parameters.AddWithValue("@kullaniciadi", txtKadi.Text.Trim());

                int etkilenenSatir = komut.ExecuteNonQuery();

                if (etkilenenSatir > 0)
                {
                    MessageBox.Show("Kullanıcı başarıyla güncellendi.");
                    
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız oldu. Kullanıcı bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }
    }
}
