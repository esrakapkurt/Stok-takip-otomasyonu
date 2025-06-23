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
    public partial class MusteriSil : Form
      
    {
        private string kullaniciGorevi;
        public MusteriSil(string gorev)
        {
            InitializeComponent();
            kullaniciGorevi = gorev;

        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        DataSet daset = new DataSet();
        private void MusteriSil_Load(object sender, EventArgs e)
        {
            Kayıt_Göster();

            

        }

        private void KayitAra()
        {
            baglanti.Open();
            DataSet ds = new DataSet();
            string SorguTum = "Select * from musteriekle";
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;

            if (chkAdSoyad.Checked && chkFirma.Checked)
            {
                SorguTum = "Select * from musteriekle where AdSoyad = @AdSoyad AND firmaadi = @Firma";
                komut.Parameters.AddWithValue("@AdSoyad", txtAdSoyadAra.Text.Trim());
                komut.Parameters.AddWithValue("@Firma", txtFirmaAra.Text.Trim());
            }
            else if (chkAdSoyad.Checked)
            {
                SorguTum = "Select * from musteriekle where AdSoyad = @AdSoyad";
                komut.Parameters.AddWithValue("@AdSoyad", txtAdSoyadAra.Text.Trim());
            }
            else if (chkFirma.Checked)
            {
                SorguTum = "Select * from musteriekle where firmaadi = @Firma";
                komut.Parameters.AddWithValue("@Firma", txtFirmaAra.Text.Trim());
            }

            komut.CommandText = SorguTum;

            SqlDataAdapter da = new SqlDataAdapter(komut);
            da.Fill(ds, "musteriekle");
            dataGridView1.DataSource = ds.Tables["musteriekle"];
            baglanti.Close();
        }

        private void Kayıt_Göster()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from musteriekle", baglanti);
            adtr.Fill(daset, "musteriekle");
            dataGridView1.DataSource = daset.Tables["musteriekle"];
            baglanti.Close();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            KayitAra();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["adsoyad"].Value.ToString();
            txtFirma.Text = dataGridView1.CurrentRow.Cells["firmaadi"].Value.ToString();
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
                SqlCommand komut = new SqlCommand("DELETE FROM musteriekle WHERE tc = @tc", baglanti);
                komut.Parameters.AddWithValue("@tc", dataGridView1.CurrentRow.Cells["tc"].Value.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();

                daset.Tables["musteriekle"].Clear();
                Kayıt_Göster();

                MessageBox.Show("Kayıt silindi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme sırasında hata oluştu: " + ex.Message);
                baglanti.Close();
            }



        }

        private void lblKGorev_Click(object sender, EventArgs e)
        {

        }

        private void btnİptal_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
