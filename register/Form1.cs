using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
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
            string str = "{\"sc\":\"his51\",\"no\":\"1\",\"na\":\"管理员\"}{\"sc\":\"@his51\",\"no\":\"1\",\"na\":\"管理员\"}{\"sc\":\"his51\",\"no\":\"1\",\"na\":\"管员\"}{\"sc\":\"his522";
            RSAHelper.RSAKey keyPair = RSAHelper.GetRASKey();
            Console.WriteLine("公钥:" + keyPair.PublicKey + "\r\n");
            Console.WriteLine("私钥:" + keyPair.PrivateKey + "\r\n");
            string en = RSAHelper.EncryptString(str, keyPair.PrivateKey);
            Console.WriteLine("加密后：" + en + "\r\n");
            Console.WriteLine("解密：" + RSAHelper.DecryptString(en, keyPair.PublicKey) + "\r\n");

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
