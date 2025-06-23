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
    public partial class MusteriEkle : Form
    {
        private string kullaniciGorevi;
        public MusteriEkle(string gorev)
        {
            InitializeComponent();
            kullaniciGorevi = gorev;
        }

        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kullanicivb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void MusteriEkle_Load(object sender, EventArgs e)
        {

        }

        public void KayitEkle()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into musteriekle(tc,AdSoyad,Firmaadi,telefon,adres,email) values(@tc,@AdSoyad,@Firmaadi,@telefon,@adres,@email)", baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@Firmaadi", txtFirma.Text);
            komut.Parameters.AddWithValue("@telefon", txtTel.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@email", txtMail.Text);
            if (komut.ExecuteNonQuery() == 1)
                MessageBox.Show("Kayıt Eklendi", "Veri Girişi");
            baglanti.Close();
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }

        }



        private void button1_Click(object sender, EventArgs e)
        {


            if (kullaniciGorevi.ToLower() != "admin")
            {
                MessageBox.Show("Bu işlemi yapmak için yetkiniz yoktur!", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            if (BoslukKontrol())
            {
                MessageBox.Show("Lütfen boş alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                KayitEkle();
            }



        }
        public bool BoslukKontrol()
        {
            bool bos = false; //boş alan yok
            txtTc.BackColor = Color.White;
            txtAdSoyad.BackColor = Color.White;
            txtFirma.BackColor = Color.White;
            txtTel.BackColor = Color.White;
            txtAdres.BackColor = Color.White;
            txtMail.BackColor = Color.White;

            if (txtMail.Text == "")
            {
               txtMail.BackColor = Color.Red;
               txtMail.Focus();
                bos = true;
            }




            if (txtAdres.Text=="")
            {
                txtAdres.BackColor = Color.Red;
                txtAdres.Focus();
                bos = true;
            }



            if (txtTel.Text =="")
            {
                txtTel.BackColor = Color.Red;
                txtTel.Focus();
                bos = true;
            }


            if (txtFirma.Text=="")
            {
                txtFirma.BackColor = Color.Red;
                txtFirma.Focus();
                bos = true;
            }

            if (txtAdSoyad.Text == "")
            {
                txtAdSoyad.BackColor = Color.Red;
                txtAdSoyad.Focus();
                bos = true;
            }


            if (txtTc.Text=="")
            {
                txtTc.BackColor = Color.Red;
               txtTc.Focus();
                bos = true; //boş alan var
            }


            return bos;

        }

        private void btnİptal_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }


}
