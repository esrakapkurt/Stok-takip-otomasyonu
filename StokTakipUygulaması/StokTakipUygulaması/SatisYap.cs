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
    public partial class SatisYap : Form
    {
        public SatisYap()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void MusteriListesiYukle()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT Tc, AdSoyad FROM musteriekle", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();

            cmbMusteri.DataSource = dt;
            cmbMusteri.DisplayMember = "AdSoyad";   
            cmbMusteri.ValueMember = "Tc";        
        }
        private void UrunListesiYukle()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT BarkodNo, ÜrünAdi FROM urun", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();

            cmbUrunAdi.DataSource = dt;
            cmbUrunAdi.DisplayMember = "ÜrünAdi";    
            cmbUrunAdi.ValueMember = "BarkodNo";     
        }










        private void SatisYap_Load(object sender, EventArgs e)
        {
            MusteriListesiYukle();
            UrunListesiYukle();
            SepetiGoster();

        }

        private void cmbMusteri_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMusteri.SelectedValue != null)
            {
                string secilenTc = cmbMusteri.SelectedValue.ToString();

                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT * FROM musteriekle WHERE Tc = @Tc", baglanti);
                komut.Parameters.AddWithValue("@Tc", secilenTc);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    TxtTC .Text = dr["Tc"].ToString();
                    txtFirma.Text = dr["FirmaAdi"].ToString();
                    txtTelefon.Text = dr["Telefon"].ToString();
                }
                baglanti.Close();
            }
        }

        private void cmbUrunAdi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUrunAdi.SelectedValue != null && cmbUrunAdi.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT Barkodno, Marka, SatışFiyatı, StokMiktarı FROM urun WHERE Barkodno = @Barkodno", baglanti);
                komut.Parameters.AddWithValue("@Barkodno", cmbUrunAdi.SelectedValue.ToString());

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    txtBarkodno.Text = dr["Barkodno"].ToString();
                    txtMarka.Text = dr["Marka"].ToString();
                    txtSatisFiyat.Text = dr["SatışFiyatı"].ToString();
                    txtStokMiktari.Text = dr["StokMiktarı"].ToString(); 
                }

                dr.Close();
                baglanti.Close();
            }
        }
        


        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtMiktar.Text, out decimal miktar) && decimal.TryParse(txtSatisFiyat.Text, out decimal fiyat))
            {
                decimal genelToplam = miktar * fiyat;
                txtTopFiyat.Text = genelToplam.ToString("N2"); // Virgülden sonra 2 basamak
            }
            else
            {
                txtTopFiyat.Text = "0,00";
            }
        }
        private void SepetiGoster()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT Tc, AdSoyad, Firma, Telefon, BarkodNo, ÜrünAdı, Marka, SatışFiyatı, Miktar, ToplamFiyat, Tarih FROM sepet", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();

          
            decimal genelToplam = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (decimal.TryParse(row["ToplamFiyat"].ToString(), out decimal fiyat))
                {
                    genelToplam += fiyat;
                }
            }

            txtGenelToplam.Text = genelToplam.ToString("N2"); 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMiktar.Text, out int miktar))
            {
                MessageBox.Show("Lütfen geçerli bir miktar giriniz.");
                return;
            }

            if (!int.TryParse(txtStokMiktari.Text, out int stok))
            {
                MessageBox.Show("Stok bilgisi geçersiz.");
                return;
            }

            if (miktar > stok)
            {
                MessageBox.Show("Yetersiz stok miktarı.");
                return;
            }

            if (!decimal.TryParse(txtSatisFiyat.Text, out decimal satisFiyat))
            {
                MessageBox.Show("Satış fiyatı geçerli değil.");
                return;
            }

            decimal toplamFiyat = miktar * satisFiyat;

            
            string tc = TxtTC.Text;
            string adSoyad = cmbMusteri.Text;
            string firma = txtFirma.Text;
            string telefon = txtTelefon.Text;
            string barkod = txtBarkodno.Text;
            string urunAdi = cmbUrunAdi.Text;
            string marka = txtMarka.Text;
            DateTime tarih = DateTime.Now;

            // 3. Veritabanına ekle
            baglanti.Open();
            SqlCommand komut = new SqlCommand(@"INSERT INTO sepet 
        (Tc, AdSoyad, Firma, Telefon, BarkodNo, ÜrünAdı, Marka, SatışFiyatı, Miktar, ToplamFiyat, Tarih)
        VALUES (@Tc, @AdSoyad, @Firma, @Telefon, @BarkodNo, @ÜrünAdı, @Marka, @SatışFiyatı, @Miktar, @ToplamFiyat, @Tarih)", baglanti);

            komut.Parameters.AddWithValue("@Tc", tc);
            komut.Parameters.AddWithValue("@AdSoyad", adSoyad);
            komut.Parameters.AddWithValue("@Firma", firma);
            komut.Parameters.AddWithValue("@Telefon", telefon);
            komut.Parameters.AddWithValue("@BarkodNo", barkod);
            komut.Parameters.AddWithValue("@ÜrünAdı", urunAdi);
            komut.Parameters.AddWithValue("@Marka", marka);
            komut.Parameters.AddWithValue("@SatışFiyatı", satisFiyat);
            komut.Parameters.AddWithValue("@Miktar", miktar);
            komut.Parameters.AddWithValue("@ToplamFiyat", toplamFiyat);
            komut.Parameters.AddWithValue("@Tarih", tarih);
            komut.ExecuteNonQuery();
            baglanti.Close();

            SepetiGoster();


            txtMiktar.Text = "";
            txtTopFiyat.Text = "0,00";
            txtBarkodno.Text = "";
            txtMarka.Text = "";
            txtSatisFiyat.Text = "";
            txtStokMiktari.Text = "";

            TxtTC.Text = "";
            txtFirma.Text = "";
            txtTelefon.Text = "";


            cmbMusteri.SelectedIndex = -1;  
            cmbUrunAdi.SelectedIndex = -1;
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sepetteki tüm ürünler silinecek. Emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM sepet", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();

                SepetiGoster(); 
                MessageBox.Show("Sepet temizlendi.");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) 
            {
               
                string tc = dataGridView1.SelectedRows[0].Cells["Tc"].Value.ToString();
                string barkodNo = dataGridView1.SelectedRows[0].Cells["BarkodNo"].Value.ToString();

                DialogResult dr = MessageBox.Show("Seçili ürünü sepetten silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand("DELETE FROM sepet WHERE Tc=@Tc AND BarkodNo=@BarkodNo", baglanti);
                        komut.Parameters.AddWithValue("@Tc", tc);
                        komut.Parameters.AddWithValue("@BarkodNo", barkodNo);
                        komut.ExecuteNonQuery();
                        baglanti.Close();

                        MessageBox.Show("Seçili ürün sepetten silindi.");
                        SepetiGoster(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Silme işlemi sırasında hata oluştu: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz satırı seçin.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                decimal genelToplam;
                if (!decimal.TryParse(txtGenelToplam.Text, out genelToplam))
                {
                    MessageBox.Show("Genel toplam bilgisi geçersiz.");
                    return;
                }

                baglanti.Open();

                SqlCommand komutSepet = new SqlCommand("SELECT Tc, AdSoyad, Firma, Telefon, BarkodNo, ÜrünAdı, Marka, SatışFiyatı, Miktar, ToplamFiyat FROM sepet", baglanti);
                SqlDataReader dr = komutSepet.ExecuteReader();

                List<dynamic> satilanUrunler = new List<dynamic>();

                while (dr.Read())
                {
                    satilanUrunler.Add(new
                    {
                        Tc = dr["Tc"].ToString(),
                        AdSoyad = dr["AdSoyad"].ToString(),
                        Firma = dr["Firma"].ToString(),
                        Telefon = dr["Telefon"].ToString(),
                        BarkodNo = dr["BarkodNo"].ToString(),
                        UrunAdi = dr["ÜrünAdı"].ToString(),
                        Marka = dr["Marka"].ToString(),
                        SatisFiyat = Convert.ToDecimal(dr["SatışFiyatı"]),
                        Miktar = Convert.ToInt32(dr["Miktar"]),
                        ToplamFiyat = Convert.ToDecimal(dr["ToplamFiyat"])
                    });
                }
                dr.Close();

                foreach (var urun in satilanUrunler)
                {
                    SqlCommand komutStokGuncelle = new SqlCommand("UPDATE urun SET StokMiktarı = StokMiktarı - @Miktar WHERE BarkodNo = @BarkodNo", baglanti);
                    komutStokGuncelle.Parameters.AddWithValue("@Miktar", urun.Miktar);
                    komutStokGuncelle.Parameters.AddWithValue("@BarkodNo", urun.BarkodNo);
                    komutStokGuncelle.ExecuteNonQuery();

                    SqlCommand komutSatisEkle = new SqlCommand(@"INSERT INTO satıs (Tc, AdSoyad, Firma, Telefon, BarkodNo, ÜrünAdı, Marka, SatışFiyatı, Miktar, ToplamFiyat, Tarih)
                                                        VALUES (@Tc, @AdSoyad, @Firma, @Telefon, @BarkodNo, @ÜrünAdı, @Marka, @SatışFiyatı, @Miktar, @ToplamFiyat, @Tarih)", baglanti);
                    komutSatisEkle.Parameters.AddWithValue("@Tc", urun.Tc);
                    komutSatisEkle.Parameters.AddWithValue("@AdSoyad", urun.AdSoyad);
                    komutSatisEkle.Parameters.AddWithValue("@Firma", urun.Firma);
                    komutSatisEkle.Parameters.AddWithValue("@Telefon", urun.Telefon);
                    komutSatisEkle.Parameters.AddWithValue("@BarkodNo", urun.BarkodNo);
                    komutSatisEkle.Parameters.AddWithValue("@ÜrünAdı", urun.UrunAdi);
                    komutSatisEkle.Parameters.AddWithValue("@Marka", urun.Marka);
                    komutSatisEkle.Parameters.AddWithValue("@SatışFiyatı", urun.SatisFiyat);
                    komutSatisEkle.Parameters.AddWithValue("@Miktar", urun.Miktar);
                    komutSatisEkle.Parameters.AddWithValue("@ToplamFiyat", urun.ToplamFiyat);
                    komutSatisEkle.Parameters.AddWithValue("@Tarih", DateTime.Now);
                    komutSatisEkle.ExecuteNonQuery();
                }

                SqlCommand komutSepetTemizle = new SqlCommand("DELETE FROM sepet", baglanti);
                komutSepetTemizle.ExecuteNonQuery();

                baglanti.Close();

                MessageBox.Show($"Satış işlemi tamamlandı.\nGenel Toplam: {genelToplam:C}");

                SepetiGoster();
                txtGenelToplam.Text = "0,00";
            }
            catch (Exception ex)
            {
                baglanti.Close();
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
