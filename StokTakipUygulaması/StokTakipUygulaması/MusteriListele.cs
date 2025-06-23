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
    public partial class MusteriListele : Form
    {
        public MusteriListele()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        DataSet daset = new DataSet();
        private void MusteriListele_Load(object sender, EventArgs e)
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["adsoyad"].Value.ToString();
            txtFirma.Text = dataGridView1.CurrentRow.Cells["firmaadi"].Value.ToString();
            txtTel.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
            txtMail.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update musteriekle set adsoyad=@AdSoyad,Firmaadi=@Firmaadi,telefon=@telefon,adres=@adres,email=@email where tc=@tc", baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@Firmaadi", txtFirma.Text);
            komut.Parameters.AddWithValue("@telefon", txtTel.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@email", txtMail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["musteriekle"].Clear();
            Kayıt_Göster();
            MessageBox.Show("Müşteri kaydı güncellendi");
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from musteriekle where tc='" + dataGridView1.CurrentRow.Cells["tc"].Value.ToString() + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["musteriekle"].Clear();
            Kayıt_Göster();
            MessageBox.Show("Kayıt silindi");
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            KayitAra();
        }
    }
    



}
