using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTakipUygulaması
{
    public partial class Anagiris : Form
    {
        public Anagiris()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value++;
               
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value > 0)
            {
                progressBar1.Value--;
                
            }
            else
            {
                KullaniciGirisi kullanicigirisi = new KullaniciGirisi();
                kullanicigirisi.Show();
                this.Hide();
                timer2.Enabled = false;


            }
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = true;
        }
    }
}
