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
    public partial class KritikStok : Form
    {
        public KritikStok()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void KritikStok_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string sorgu = "SELECT BarkodNo,ÜrünAdi, Kategori,Marka , StokMiktarı FROM urun WHERE KritikStokMiktarı <= 5 AND StokMiktarı <= 5";
                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                // StokMiktarı hücresini kırmızı yap
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["StokMiktarı"].Value != null)
                    {
                        row.Cells["StokMiktarı"].Style.BackColor = Color.Red;
                        row.Cells["StokMiktarı"].Style.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Azalan stoklar yüklenirken hata oluştu: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
    }
}
