using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Register
{
    public partial class Form1 : Form
    {
        private string privatKey = "gDjqtOZOMtaNIbyH6ZelYY1JSmteYRAFt+gEkOajRLCcHnjMTk/6puRyVg6r58dB9QrHsG7wvuPrPBQXrrudE82TIIxPsJpLA5q/EgOmr8lIJM4ZMIUSGroNsiOLgdXU4Q6pYbuTwWqwbRdY+j0EAO+O3TcHfB+ijsaKdUTBB2WzmEeAzV8kKagvIUmkv6NKydN8jjQKHuP8UYr4UMQtYGRRrEPb3R/+fFOHYuF7nBYuEJfdPzQkmyJrhbN6CHPqURgnSJLVr+5h4/wsCthGfM/lk73XfnmqUXXleZPijP/npgZf8RlJVWHPqvFOPILAuntz+VMzt1Dl5Oh9iQBDsM0=";
        public Form1()
        {
            RSAHelper.RSAKey keyPair = RSAHelper.GetRASKey();
            Console.WriteLine("公钥:" + keyPair.PublicKey + "\r\n");
            Console.WriteLine("私钥:" + keyPair.PrivateKey + "\r\n");

            string[] tests = new string[]
            {
                "21537821537",
                "1231bcjewvcbyudwekuidgfyubwdhewvgyg+++123",
                "1273216392173192jcnfreiubfurebyurey02193-192-03bfrueibvbevb",
                "12371293912893ncuirefibvyubreufbwbfgvqag213bd2gdkd-[21dkqwd21udh19ugd769uif32fh7823fh372gfg327fgugf23urvf67uhf23ubft23fh8923yfi3h78h2bg8t73gfbjhvfiuwfvy23bfy",
                "{\"sc\":\"his51\",\"no\":\"1\",\"na\":\"管理员\"}{\"sc\":\"@his51\",\"no\":\"1\",\"na\":\"管理员\"}{\"sc\":\"his51\",\"no\":\"1\",\"na\":\"管员\"}{\"sc\":\"his522",
                "774E1B24F0ABC82C5AF5C4326B14F45DCD9EEF201461AF676E8312D1C35C81911B3BBC8AF45D521DA2EFFDC52160041E6EC77D806A7E4286117B615411CC9AEBE7168C56CAC5E4227B7F19B2CFA666B6E9399AA8CBE16FEF29B2A0DEC172D171932FE6CD8CDD0AFE155A355F2A94FA6ED715DB51C1A2AD917F0B5C75979E388E"
            };
            foreach (var test in tests)
            {
                string encode = RSAHelper.EncryptString(test, keyPair.PrivateKey);
                string decode = RSAHelper.DecryptString(encode, keyPair.PublicKey);
                if (decode != MD5Helper.CreateMD5(test))
                {
                    MessageBox.Show("加密算法出错！");
                }
            }

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = new HardwareInfo().GetUniqueCode();
            }
            label1.Text = RSAHelper.EncryptString(textBox1.Text, privatKey);
            Clipboard.SetDataObject(label1.Text);
        }
    }
}
