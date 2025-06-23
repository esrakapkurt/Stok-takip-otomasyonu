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
    public partial class UrunGuncelle : Form
    {
        public UrunGuncelle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        DataSet daset = new DataSet();
        private void label7_Click(object sender, EventArgs e)
        {

        }


        private void Kayıt_Göster()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from urun", baglanti);
            adtr.Fill(daset, "urun");
            dataGridView1.DataSource = daset.Tables["urun"];
            baglanti.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void UrunGuncelle_Load(object sender, EventArgs e)
        {
            Kayıt_Göster();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtBarkodNo.Text = dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString();
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["ÜrünAdi"].Value.ToString();
            txtMarka.Text = dataGridView1.CurrentRow.Cells["Marka"].Value.ToString();
            txtAfiyat.Text = dataGridView1.CurrentRow.Cells["AlışFiyatı"].Value.ToString();
            txtSfiyat.Text = dataGridView1.CurrentRow.Cells["SatışFiyatı"].Value.ToString();
            
        }
        private void KayitAra()
        {
            baglanti.Open();
            DataSet ds = new DataSet();
            string SorguTum = "Select * from urun";
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;

            if (chkUrun.Checked && chkKategori.Checked)
            {
                SorguTum = "Select * from urun where ÜrünAdi=@ÜrünAdi AND Kategori = @Kategori";
                komut.Parameters.AddWithValue("@ÜrünAdi", txtUrunAra.Text.Trim());
                komut.Parameters.AddWithValue("@Kategori", txtKategoriAra.Text.Trim());
            }
            else if (chkUrun.Checked)
            {
                SorguTum = "Select * from urun where ÜrünAdi=@ÜrünAdi";
                komut.Parameters.AddWithValue("@ÜrünAdi", txtUrunAra.Text.Trim());
            }
            else if (chkKategori.Checked)
            {
                SorguTum = "Select * from urun where Kategori = @Kategori";
                komut.Parameters.AddWithValue("@Kategori", txtKategoriAra.Text.Trim());
            }

            komut.CommandText = SorguTum;

            SqlDataAdapter da = new SqlDataAdapter(komut);
            da.Fill(ds, "urun");
            dataGridView1.DataSource = ds.Tables["urun"];
            baglanti.Close();
        }





        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            string sorgu = "UPDATE urun SET ÜrünAdi=@ÜrünAdi, Marka=@Marka, AlışFiyatı=@AlışFiyatı, SatışFiyatı=@SatışFiyatı WHERE BarkodNo=@BarkodNo";

            SqlCommand komut = new SqlCommand(sorgu, baglanti);

            decimal alisFiyat, satisFiyat;

            if (!decimal.TryParse(txtAfiyat.Text, out alisFiyat))
            {
                MessageBox.Show("Lütfen alış fiyatını doğru formatta giriniz.");
                baglanti.Close();
                return;
            }

            if (!decimal.TryParse(txtSfiyat.Text, out satisFiyat))
            {
                MessageBox.Show("Satış fiyatı hesaplanamadı. Alış fiyatı ve KDV oranını kontrol edin.");
                baglanti.Close();
                return;
            }

            komut.Parameters.AddWithValue("@ÜrünAdi", txtUrunAdi.Text);
            komut.Parameters.AddWithValue("@Marka", txtMarka.Text);
            komut.Parameters.AddWithValue("@AlışFiyatı", alisFiyat);
            komut.Parameters.AddWithValue("@SatışFiyatı", satisFiyat);
            komut.Parameters.AddWithValue("@BarkodNo", txtBarkodNo.Text);

            komut.ExecuteNonQuery();

            baglanti.Close();

            daset.Tables["urun"].Clear();
            Kayıt_Göster();

            MessageBox.Show("Ürün kaydı güncellendi.");

            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }


        }

        private void HesaplaVeGuncelleSatisFiyati()
        {
            if (decimal.TryParse(txtAfiyat.Text, out decimal alisFiyati) &&
                decimal.TryParse(cmbKdv.Text, out decimal kdvOrani))
            {
                decimal kdvOraniDecimal = kdvOrani / 100;
                decimal satisFiyati = alisFiyati + (alisFiyati * kdvOraniDecimal);
                txtSfiyat.Text = satisFiyati.ToString("F2");
            }
            else
            {
                txtSfiyat.Text = "";
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            KayitAra();
        }

        private void txtAfiyat_TextChanged(object sender, EventArgs e)
        {
            HesaplaVeGuncelleSatisFiyati();
        }

        private void cmbKdv_SelectedIndexChanged(object sender, EventArgs e)
        {
            HesaplaVeGuncelleSatisFiyati();
        }
    }
}
