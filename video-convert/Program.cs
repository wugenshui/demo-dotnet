using NReco.VideoConverter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace video_convert
{
    class Program
    {
        static void Main(string[] args)
        {
            FFMpegConverter ffMpeg = new FFMpegConverter();
            ConvertSettings setting = new ConvertSettings();
            setting.CustomOutputArgs = " -threads 2";   // 以两个线程进行运行,加快处理的速度
            for (int i = 0; i < 100; i++)
            {
                ffMpeg.ConvertMedia("1.amr", "2.mp3", "MP3");  // 将h5不支持的视频转换为支持的视频
                Console.WriteLine(i);
            }
        }

        static void ConvertVideo()
        {
            FFMpegConverter ffMpeg = new FFMpegConverter();
            ConvertSettings setting = new ConvertSettings();
            setting.VideoFrameSize = FrameSize.svga800x600;
            setting.CustomOutputArgs = " -threads 2";   // 以两个线程进行运行,加快处理的速度
            //setting.SetVideoFrameSize(100, 20);
            for (int i = 0; i < 100; i++)
            {
                ffMpeg.ConvertMedia("1.mp4", Format.mp4, "2.mp4", Format.mp4, setting);  // 将h5不支持的视频转换为支持的视频
                Console.WriteLine(i);
            }
            //VideoHelper.RunProcess(" -i 1.mp4 -ab 56 -ar 22050 -b 500K 2.mp4");
        }
    }

    public class VideoHelper
    {
        /// <summary>
        /// 调用ffmpeg.exe 执行命令
        /// </summary>
        /// <param name="Parameters">命令参数</param>
        /// <returns>返回执行结果</returns>
        public static string RunProcess(string Parameters)
        {
            string FFmpegPath = "./ffmpeg.exe";
            //创建一个ProcessStartInfo对象 并设置相关属性
            var process = new ProcessStartInfo(FFmpegPath, Parameters);
            process.UseShellExecute = false;
            process.CreateNoWindow = true;
            process.RedirectStandardOutput = true;
            process.RedirectStandardError = true;
            process.RedirectStandardInput = true;
            //创建一个字符串和StreamReader 用来获取处理结果
            string output = string.Empty;
            StreamReader srOutput = null;

            try
            {
                var proc = Process.Start(process); //调用ffmpeg开始处理命令
                proc.WaitForExit();
                srOutput = proc.StandardError;//获取输出流
                output = srOutput.ReadToEnd();//转换成string
                proc.Close();//关闭处理程序
            }
            catch (Exception ex)
            {
                output = string.Empty;
            }
            finally
            {
                //释放资源
                if (srOutput != null)
                {
                    srOutput.Close();
                    srOutput.Dispose();
                }
            }
            return output;
        }
    }
}
