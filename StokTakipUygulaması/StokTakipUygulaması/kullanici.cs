using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace StokTakipUygulaması
{
    class kullanici
    {
      SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private string _kullaniciadi;
        private string _sifre;

        public string KullaniciAdi
        {
            get { return _kullaniciadi; }
            set { _kullaniciadi = value; }
        }

        public string Sifre
        {
            get { return _sifre; }
            set { _sifre = value; }
        }

        public string Gorev { get; private set; } 

        public bool GirisYap(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool girisBasarili = false;

            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT * FROM Kullanicilar WHERE kullaniciadi = @kullaniciadi", baglanti);
                komut.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);
                SqlDataReader reader = komut.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["sifre"].ToString() == sifre)
                    {
                        girisBasarili = true;
                        Gorev = reader["gorev"].ToString(); 
                    }
                }

                reader.Close();
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

            return girisBasarili;
        }

        public void YeniKullanici(string kullaniciadi, string sifre, string adisoyadi, string gorev)
        {
            try
            {
              
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }

                string sorgu = "INSERT INTO Kullanicilar (kullaniciadi, sifre, adisoyadi, gorev) VALUES (@kullaniciadi, @sifre, @adisoyadi, @gorev)";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);

                komut.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
                komut.Parameters.AddWithValue("@sifre", sifre);
                komut.Parameters.AddWithValue("@adisoyadi", adisoyadi);
                komut.Parameters.AddWithValue("@gorev", gorev);

                komut.ExecuteNonQuery();
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        public bool KullaniciVarMi(string kullaniciadi)
        {
            try
            {
               
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

               
                string sorgu = "SELECT COUNT(*) FROM Kullanicilar WHERE kullaniciadi = @kullaniciadi";

               
                SqlCommand komut = new SqlCommand(sorgu, baglanti);

              
                komut.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);

              
                int kayitSayisi = (int)komut.ExecuteScalar();

                
                return kayitSayisi > 0;
            }
            catch (Exception ex)
            {
              
                MessageBox.Show("Hata oluştu: " + ex.Message);
                return false; 
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }

        public bool SifreGuncelle(string kullaniciadi, string yenisifre)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                
                string kontrolSorgu = "SELECT COUNT(*) FROM Kullanicilar WHERE kullaniciadi = @kullaniciadi";
                SqlCommand kontrolKomut = new SqlCommand(kontrolSorgu, baglanti);
                kontrolKomut.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);

                int kayitSayisi = (int)kontrolKomut.ExecuteScalar();

                if (kayitSayisi == 0)
                {
                    return false; 
                }

                
                string sorgu = "UPDATE Kullanicilar SET sifre = @sifre WHERE kullaniciadi = @kullaniciadi";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@sifre", yenisifre);
                komut.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);

                komut.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                return false;
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }





    }

}
