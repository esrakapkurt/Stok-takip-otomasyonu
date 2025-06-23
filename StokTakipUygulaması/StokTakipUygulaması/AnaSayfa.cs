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
    public partial class AnaSayfa : Form
    {
        private string gorev;
        public AnaSayfa(string kullaniciGorevi)
        {
            InitializeComponent();
            gorev = kullaniciGorevi;
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");


        private void müşteriEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusteriEkle ekle = new MusteriEkle(gorev);
            ekle.ShowDialog();
        }

        private void müşteriGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusteriListele listele = new MusteriListele();
            listele.ShowDialog();
        }

        private void müşteriToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void müşteriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusteriSil sil = new MusteriSil(gorev);
            sil.ShowDialog();
        }

        private async void AnaSayfa_Load_1(object sender, EventArgs e)
        {
            if (gorev != "admin")
            {
                müşteriEkleToolStripMenuItem.Enabled = true;
                müşteriSilToolStripMenuItem.Enabled = true;
            }

            await Task.Delay(2000);  
            StokKontrolUyarisi();
        }
        private void StokKontrolUyarisi()
        {
            try
            {
                baglanti.Open();

                string sorgu = "SELECT ÜrünAdi, StokMiktarı FROM urun WHERE StokMiktarı <= 5";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                SqlDataReader dr = komut.ExecuteReader();

                List<string> kritikUrunler = new List<string>();

                while (dr.Read())
                {
                    string urunAdi = dr["ÜrünAdi"].ToString();
                    int stokMiktari = Convert.ToInt32(dr["StokMiktarı"]);
                    kritikUrunler.Add($"{urunAdi} (Stok Miktarı: {stokMiktari})");
                }
                dr.Close();

                if (kritikUrunler.Count > 0)
                {
                    string mesaj = "Stok Miktarı 5 ve altında olan ürünler:\n" + string.Join("\n", kritikUrunler);
                    MessageBox.Show(mesaj, "Stok Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stok kontrolü sırasında hata oluştu: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void AnaSayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ürünEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UrunEkle ekle = new UrunEkle(gorev);
            ekle.ShowDialog();
        }

        private void ürünGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UrunGuncelle guncelle= new UrunGuncelle();
            guncelle.ShowDialog();
        }

        private void ürünSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void ürünSilToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            UrunSil urunsil = new UrunSil(gorev);
            urunsil.ShowDialog();
        }

        private void stokGirişToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StokGiriş stokgiris = new StokGiriş();
            stokgiris.ShowDialog();
        }

        private void satışYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SatisYap satis = new SatisYap();
            satis.ShowDialog();
        }

        private void kullanıcıAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void yöneciAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gorev.ToLower() != "admin")
            {
                MessageBox.Show("Bu bölüme giriş yetkiniz yoktur.", "Yetkisiz Erişim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            YöneticiAyar kullaniciForm = new YöneticiAyar(gorev);
            kullaniciForm.Show();

        }

        private void personelAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            personelayar personelayar = new personelayar();
            personelayar.ShowDialog();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();  
            KullaniciGirisi girisFormu = new KullaniciGirisi();
            girisFormu.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MusteriEkle ekle = new MusteriEkle(gorev);
            ekle.ShowDialog();
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            UrunEkle ekle = new UrunEkle(gorev);
            ekle.ShowDialog();
        }

        private void btnStokEkle_Click(object sender, EventArgs e)
        {
            StokGiriş stokgiris = new StokGiriş();
            stokgiris.ShowDialog();
        }

        private void btnSatisGecmis_Click(object sender, EventArgs e)
        {
            SatisGecmisi satisGecmisForm = new SatisGecmisi();
            satisGecmisForm.ShowDialog();
        }

        private void btnStokListe_Click(object sender, EventArgs e)
        {
            UrunListesi urunlistesi = new UrunListesi();
            urunlistesi.ShowDialog();
        }

        private void btnKritikStok_Click(object sender, EventArgs e)
        {
           KritikStok kritikstok = new KritikStok();
           kritikstok.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
