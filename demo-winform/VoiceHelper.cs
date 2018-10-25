using DotNetSpeech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace demo_winform
{
    class VoiceHelper
    {
        public static SpVoice m_voice = new SpVoice();

        /// <summary>
        /// 根据文字播放出语音
        /// </summary>
        /// <param name="text">文字</param>
        public static void PlayVoice(string text, Boolean closeVoice = true)
        {
            m_voice.Rate = -1;
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            m_voice.Speak(text, spFlags);
            //m_voice.WaitUntilDone(System.Threading.Timeout.Infinite);
            //Thread.Sleep(500);
        }


        public static void CloseVoice()
        {
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak;
            m_voice.Speak("", spFlags);
        }
    }
}
