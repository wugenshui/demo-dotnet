using DotNetSpeech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace demo_winform
{
    public partial class Form1 : Form
    {
        public string msg = "黄伟均身份已识别，欢迎登录系统！";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //VoiceHelper.PlayVoice(msg);

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择要打开的文件";
            ofd.Multiselect = false;
            //ofd.InitialDirectory = @"C:\Uss\shaofeng\Desktop";
            ofd.Filter = "文本文件|*.txt|所有文件|*.*";
            ofd.ShowDialog();
            string path = ofd.FileName;
            if (path == "")
            {
                return;
            }

            FileInfo file = new FileInfo(path);

            DateTime date = DateTime.Today.AddDays(-1);
            file.CreationTime = date;
            file.LastWriteTime = date;
            file.LastAccessTime = date;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SpVoice m_voice = VoiceHelper.m_voice;
            m_voice.Rate = -1;
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFDefault;
            m_voice.Speak(msg, spFlags);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SpVoice m_voice = VoiceHelper.m_voice;
            m_voice.Rate = -1;
            m_voice.Speak(msg);
        }
    }
}
