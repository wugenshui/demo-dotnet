using DotNetSpeech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            VoiceHelper.PlayVoice(msg);
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
