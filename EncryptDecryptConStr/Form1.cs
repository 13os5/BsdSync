using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KE.LibraryAPI.Configuration;
using KE.LibraryAPI.Data;

namespace EncryptDecryptConStr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void bEncrypt_Click(object sender, EventArgs e)
        {
            using (ConManage cm = new ConManage())
            {
                txtOIp.Text = cm.EncriptStrConn(txtIIp.Text);
                txtODb.Text = cm.EncriptStrConn(txtIDb.Text);
                txtOUsn.Text = cm.EncriptStrConn(txtIUsn.Text);
                txtOPwd.Text = cm.EncriptStrConn(txtIPwd.Text);
            }
        }

        private void bDecrypt_Click(object sender, EventArgs e)
        {
            using (ConManage cm = new ConManage())
            {
                txtOIpD.Text = cm.DecryptStrConn(txtIIpD.Text, ConfigMange.GetAppSettings("Tey"));
                txtODbD.Text = cm.DecryptStrConn(txtIDbD.Text, ConfigMange.GetAppSettings("Tey"));
                txtOUsnD.Text = cm.DecryptStrConn(txtIUsnD.Text, ConfigMange.GetAppSettings("Tey"));
                txtOPwdD.Text = cm.DecryptStrConn(txtIPwdD.Text, ConfigMange.GetAppSettings("Tey"));
            }
        }

        private void bCopy_Click(object sender, EventArgs e)
        {
            txtIIpD.Text = txtOIp.Text;
            txtIDbD.Text = txtODb.Text;
            txtIUsnD.Text = txtOUsn.Text;
            txtIPwdD.Text = txtOPwd.Text;
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            txtIIp.Text = "";
            txtIDb.Text = "";
            txtIUsn.Text = "";
            txtIPwd.Text = "";
            txtOIp.Text = "";
            txtODb.Text = "";
            txtOUsn.Text = "";
            txtOPwd.Text = "";

            txtIIpD.Text = "";
            txtIDbD.Text = "";
            txtIUsnD.Text = "";
            txtIPwdD.Text = "";
            txtOIpD.Text = "";
            txtODbD.Text = "";
            txtOUsnD.Text = "";
            txtOPwdD.Text = "";
            txtIIp.Focus();
        }
    }
}
