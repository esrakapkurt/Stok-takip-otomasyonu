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
    public partial class UrunEkle : Form
    {
        private string kullaniciGorevi;
        public UrunEkle(string gorev)
        {
            InitializeComponent();
            kullaniciGorevi = gorev;
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnBarkodNo_Click(object sender, EventArgs e)
        {
            string barkod = BarkodNoUret();
            txtBarkodNo.Text = barkod;
        }
        public void UrunEkleme()
        {
            try
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("INSERT INTO urun (BarkodNo, ÜrünAdi, Kategori, Marka, StokMiktarı, KritikStokMiktarı, AlışFiyatı, SatışFiyatı, KDVOrani, EklenmeTarihi) VALUES (@BarkodNo, @ÜrünAdi, @Kategori, @Marka, @StokMiktarı, @KritikStokMiktarı, @AlışFiyatı, @SatışFiyatı, @KDVOrani, @EklenmeTarihi)", baglanti);

            
                komut.Parameters.AddWithValue("@BarkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@ÜrünAdi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@Kategori", cmbKategori.Text);
                komut.Parameters.AddWithValue("@Marka", txtMarka.Text);
                komut.Parameters.AddWithValue("@StokMiktarı", txtStokMiktari.Text);
                komut.Parameters.AddWithValue("@KritikStokMiktarı", txtKStokMiktari.Text);

                decimal alisFiyati = decimal.Parse(txtAfiyat.Text);
                decimal kdvOrani = decimal.Parse(cmbKdv.Text) / 100;
                decimal satisFiyati = alisFiyati + (alisFiyati * kdvOrani);

                komut.Parameters.AddWithValue("@AlışFiyatı", alisFiyati);
                komut.Parameters.AddWithValue("@SatışFiyatı", satisFiyati);
                komut.Parameters.AddWithValue("@KDVOrani", cmbKdv.Text);
                komut.Parameters.AddWithValue("@EklenmeTarihi", dtEklenmeTarihi.Value);

                if (komut.ExecuteNonQuery() == 1)
                    MessageBox.Show("Kayıt Eklendi", "Veri Girişi");

                
                foreach (Control item in this.Controls)
                {
                    if (item is TextBox) item.Text = "";
                }

                cmbKategori.SelectedIndex = -1;
                cmbKdv.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
        private string BarkodNoUret()
        {
            Random rnd = new Random();
            string barkod = "";
            for (int i = 0; i < 13; i++)
            {
                barkod += rnd.Next(0, 10).ToString(); 
            }
            return barkod;
        }
        private void KategorileriListele()
        {
            try
            {
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Kategori", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void btnKategoriEkle_Click(object sender, EventArgs e)
        {
            if (txtKategoriAdi.Text != "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(baglanti.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand komut = new SqlCommand("INSERT INTO Kategori (Kategoriadi) VALUES (@adi)", conn);
                        komut.Parameters.AddWithValue("@adi", txtKategoriAdi.Text);
                        komut.ExecuteNonQuery();
                    }

                    MessageBox.Show("Kategori eklendi.");
                    txtKategoriAdi.Clear();

                    KategorileriYukle();   
                    KategorileriListele();  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen kategori adını girin.");
            }
        }
        private void KategorileriYukle()
        {
            cmbKategori.Items.Clear(); 
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM Kategori", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                cmbKategori.Items.Add(dr["Kategoriadi"].ToString());
            }
            baglanti.Close();
        }

        private void UrunEkle_Load(object sender, EventArgs e)
        {
            KategorileriYukle();
            KategorileriListele();
        }

        private void btnKategoriSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int kategoriId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Kategorikodu"].Value);

                try
                {
                    using (SqlConnection conn = new SqlConnection(baglanti.ConnectionString))
                    {
                        conn.Open();
                        SqlCommand komut = new SqlCommand("DELETE FROM Kategori WHERE Kategorikodu = @id", conn);
                        komut.Parameters.AddWithValue("@id", kategoriId);
                        int sonuc = komut.ExecuteNonQuery();

                        if (sonuc > 0)
                        {
                            MessageBox.Show("Kategori silindi.");
                            KategorileriListele();
                            KategorileriYukle();
                        }
                        else
                        {
                            MessageBox.Show("Kategori silinemedi.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir satır seçin.");
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {

            if (kullaniciGorevi.ToLower() != "admin")
            {
                MessageBox.Show("Bu işlemi yapmak için yetkiniz yoktur!", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            else
            {
                UrunEkleme();
            }
        }

        private void cmbKdv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAfiyat.Text, out decimal alisFiyati) &&
        decimal.TryParse(cmbKdv.Text, out decimal kdvOrani))
            {
                decimal kdv = kdvOrani / 100;
                decimal satisFiyati = alisFiyati + (alisFiyati * kdv);
                txtSfiyat.Text = satisFiyati.ToString("0.00");
            }
        }

        private void txtAfiyat_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAfiyat.Text, out decimal alisFiyati) &&
        decimal.TryParse(cmbKdv.Text, out decimal kdvOrani))
            {
                decimal kdv = kdvOrani / 100;
                decimal satisFiyati = alisFiyati + (alisFiyati * kdv);
                txtSfiyat.Text = satisFiyati.ToString("0.00");
            }
        }
    }
}
