using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KE.LibraryAPI;
using KE.LibraryAPI.Security;
using KE.LibraryAPI.Configuration;
using KE.LibraryAPI.Data;
using System.IO;
using System.Reflection;

namespace WinApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string Source { get; set; }
        public string IP { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string StrConn { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Source = "";
            this.IP = "";
            this.Username = "";
            this.Password = "";


            string pa = "Data source = {0};Initial Catalog = {1}; User ID = {2}; Password = {3}";
            string i = "172.25.33.16";
            string d = "KE_BKKSD";
            string u = "warakorn_t";
            string p = "bwbww13os5!#";

            string paa = "";
            string ii = "";
            string dd = "";
            string uu = "";
            string pp = "";
            using (EncryptStr en = new EncryptStr())
            {
                paa = en.Encrypt(pa, ConfigMange.GetAppSettings("Tey"));
                ii = en.Encrypt(i, ConfigMange.GetAppSettings("Tey"));
                dd = en.Encrypt(d, ConfigMange.GetAppSettings("Tey"));
                uu = en.Encrypt(u, ConfigMange.GetAppSettings("Tey"));
                pp = en.Encrypt(p, ConfigMange.GetAppSettings("Tey"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string paa = "";
            string ii = "";
            string dd = "";
            string uu = "";
            string pp = "";

            string pa = "";
            string i = "";
            string d = "";
            string u = "";
            string p = "";

            string current = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            string fpa = current + @"\pa.txw";
            string fi = current + @"\i.tsu";
            string fd = current + @"\d.xwm";
            string fu = current + @"\u.lpn";
            string fp = current + @"\p.txu";

            if (File.Exists(fpa))
            {
                string[] lines = File.ReadAllLines(fpa);
                foreach (string line in lines)
                {
                    paa = line;
                }
            }
            if (File.Exists(fi))
            {
                string[] lines = File.ReadAllLines(fi);
                foreach (string line in lines)
                {
                    ii = line;
                }
            }
            if (File.Exists(fd))
            {
                string[] lines = File.ReadAllLines(fd);
                foreach (string line in lines)
                {
                    dd = line;
                }
            }
            if (File.Exists(fu))
            {
                string[] lines = File.ReadAllLines(fu);
                foreach (string line in lines)
                {
                    uu = line;
                }
            }
            if (File.Exists(fp))
            {
                string[] lines = File.ReadAllLines(fp);
                foreach (string line in lines)
                {
                    pp = line;
                }
            }

            using (EncryptStr en = new EncryptStr())
            {
                pa = en.Decrypt(paa, ConfigMange.GetAppSettings("Tey"));
                i = en.Decrypt(ii, ConfigMange.GetAppSettings("Tey"));
                d = en.Decrypt(dd, ConfigMange.GetAppSettings("Tey"));
                u = en.Decrypt(uu, ConfigMange.GetAppSettings("Tey"));
                p = en.Decrypt(pp, ConfigMange.GetAppSettings("Tey"));
            }


            //string pa = "Data source = {0};Initial Catalog = {1}; User ID = {2}; Password = {3}";
            //string i = "172.25.33.16";
            //string d = "KE_BKKSD";
            //string u = "warakorn_t";
            //string p = "bwbww13os5!#";

            string ppp = string.Format(pa, i, d, u, p);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pa = "Data source = {0};Initial Catalog = {1}; User ID = {2}; Password = {3}";
            string i = "172.25.33.16";
            string d = "KE_BKKSD";
            string u = "warakorn_t";
            string p = "bwbww13os5!#";

            string current = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            ConManage cm = new ConManage();
            StrConnEntity sce = new StrConnEntity();
            sce.conStr = "Data source = {0};Initial Catalog = {1}; User ID = {2}; Password = {3}";
            sce.ip = "172.25.33.16";
            sce.db = "KE_BKKSD";
            sce.usn = "warakorn_t";
            sce.pwd = "bwbww13os5!#";

            cm.EncriptStrConn(sce, current, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] bbd;
            string[] bdwp;
            string[] bnoc;
            string[] bnsu;
            string[] bpi;

            StrConnEntity sce = new StrConnEntity();
            using (ConManage cm = new ConManage())
            {
                using (FileManage fm = new FileManage())
                {
                    bbd = fm.ReadFile(@"D:\BsdServiceSync\WinApp1\bin\Debug\bbd.agi");
                    bdwp = fm.ReadFile(@"D:\BsdServiceSync\WinApp1\bin\Debug\bdwp.luk");
                    bnoc = fm.ReadFile(@"D:\BsdServiceSync\WinApp1\bin\Debug\bnoc.int");
                    bnsu = fm.ReadFile(@"D:\BsdServiceSync\WinApp1\bin\Debug\bnsu.dex");
                    bpi = fm.ReadFile(@"D:\BsdServiceSync\WinApp1\bin\Debug\bpi.str");

                    sce.db = cm.DecryptStrConn(bbd[0], ConfigMange.GetAppSettings("Tey"));
                    sce.pwd = cm.DecryptStrConn(bdwp[0], ConfigMange.GetAppSettings("Tey"));
                    sce.conStr = cm.DecryptStrConn(bnoc[0], ConfigMange.GetAppSettings("Tey"));
                    sce.usn = cm.DecryptStrConn(bnsu[0], ConfigMange.GetAppSettings("Tey"));
                    sce.ip = cm.DecryptStrConn(bpi[0], ConfigMange.GetAppSettings("Tey"));

                    label1.Text = string.Format(sce.conStr, sce.ip, sce.db, sce.usn, sce.pwd);
                }
            }
        }
    }
}
