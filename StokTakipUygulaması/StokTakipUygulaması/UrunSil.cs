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
    public partial class UrunSil : Form
    {
        private string kullaniciGorevi;
        public UrunSil(string gorev)
        {
            InitializeComponent();
            kullaniciGorevi = gorev;
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        DataSet daset = new DataSet();

        private void Kayıt_Göster()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from urun", baglanti);
            adtr.Fill(daset, "urun");
            dataGridView1.DataSource = daset.Tables["urun"];
            baglanti.Close();
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

        private void btnAra_Click(object sender, EventArgs e)
        {
            KayitAra();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtBarkodNo.Text = dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString();
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["ÜrünAdi"].Value.ToString();
            txtMarka.Text = dataGridView1.CurrentRow.Cells["Marka"].Value.ToString();
        }

        private void UrunSil_Load(object sender, EventArgs e)
        {
            Kayıt_Göster();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (kullaniciGorevi.ToLower() != "admin")
                {
                    MessageBox.Show("Bu işlemi yapmak için yetkiniz yoktur!", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM urun WHERE BarkodNo = @BarkodNo", baglanti);
                komut.Parameters.AddWithValue("@BarkodNo", dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();

                daset.Tables["urun"].Clear();
                Kayıt_Göster();

                MessageBox.Show("Kayıt silindi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme sırasında hata oluştu: " + ex.Message);
                baglanti.Close();
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
