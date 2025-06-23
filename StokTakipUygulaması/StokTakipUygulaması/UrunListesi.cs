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
    public partial class UrunListesi : Form
    {
        public UrunListesi()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void UrunListesi_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string sorgu = "SELECT BarkodNo, ÜrünAdi, Kategori, Marka, StokMiktarı, KritikStokMiktarı, AlışFiyatı, SatışFiyatı, KDVOrani, EklenmeTarihi FROM urun";
                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ürün listesi yüklenirken hata oluştu: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
    }
}
