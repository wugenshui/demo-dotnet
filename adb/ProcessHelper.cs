using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace adb
{
    class ProcessHelper
    {
        public static string Run(string exePath, string args)
        {
            string result = string.Empty;
            using (Process p = GetProcess())
            {
                p.StartInfo.FileName = exePath;
                p.StartInfo.Arguments = args;
                p.Start();
                result = p.StandardOutput.ReadToEnd(); // 正常信息
                // p.StandardError.ReadToEnd() 异常信息

                p.WaitForExit();
            }

            return result;
        }

        /// <summary>
        /// 读取数据的时候等待时间，等待时间过短时，可能导致读取不出正确的数据。
        /// </summary>
        public static int WaitTime = 50;

        private static Process GetProcess()
        {
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true; // 不显示Dos窗口
            process.StartInfo.UseShellExecute = false; // 是否指定操作系统外壳进程启动程序
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;

            return process;
        }
        private static string ReadStandardOutputLine(Process p)
        {
            StringBuilder tmp = new StringBuilder();

            //当下一次读取时，Peek可能为-1，但此时缓冲区其实是有数据的。
            //正常的Read一次之后，Peek就又有效了。
            if (p.StandardOutput.Peek() == -1)
            {
                tmp.Append((char)p.StandardOutput.Read());
            }

            while (p.StandardOutput.Peek() > -1)
            {
                tmp.Append((char)p.StandardOutput.Read());
            }


            return tmp.ToString();
        }

        /// <summary>
        /// 连续运行模式，支持打开某程序后，持续向其输入命令，直到结束。
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="args"></param>
        /// <param name="moreArgs"></param>
        /// <returns></returns>
        public static RunResult RunAsContinueMode(string exePath, string args, string[] moreArgs)
        {
            var result = new RunResult();
            try
            {
                using (var p = GetProcess())
                {
                    p.StartInfo.FileName = exePath;
                    p.StartInfo.Arguments = args;
                    p.Start();

                    //先输出一个换行，以便将程序的第一行输出显示出来。
                    //如adb.exe，假如不调用此函数的话，第一行等待的shell@android:/ $必须等待下一个命令输入才会显示。
                    p.StandardInput.WriteLine();

                    result.OutputString = ReadStandardOutputLine(p);

                    result.MoreOutputString = new Dictionary<int, string>();
                    for (int i = 0; i < moreArgs.Length; i++)
                    {
                        p.StandardInput.WriteLine(moreArgs[i] + '\r');

                        //必须等待一定时间，让程序运行一会儿，马上读取会读出空的值。
                        Thread.Sleep(WaitTime);

                        result.MoreOutputString.Add(i, ReadStandardOutputLine(p));
                    }

                    p.WaitForExit();
                    result.ExitCode = p.ExitCode;
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.OutputString = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 当执行不成功时，OutputString会输出错误信息。
        /// </summary>
        public class RunResult
        {

            public bool Success;
            public int ExitCode;
            public string OutputString;
            /// <summary>
            /// 调用RunAsContinueMode时，使用额外参数的顺序作为索引。
            /// 如：调用ProcessHelper.RunAsContinueMode(AdbExePath, "shell", new[] { "su", "ls /data/data", "exit", "exit" });
            /// 果：MoreOutputString[0] = su执行后的结果字符串；MoreOutputString[1] = ls ...执行后的结果字符串；MoreOutputString[2] = exit执行后的结果字符串
            /// </summary>
            public Dictionary<int, string> MoreOutputString;

            public new string ToString()
            {
                var str = new StringBuilder();
                str.AppendFormat("Success:{0}\nExitCode:{1}\nOutputString:{2}\nMoreOutputString:\n", Success, ExitCode, OutputString);
                if (MoreOutputString != null)
                {
                    foreach (var v in MoreOutputString)
                    {
                        str.AppendFormat("{0}:{1}\n", v.Key, v.Value.Replace("\r", "\\Ⓡ").Replace("\n", "\\Ⓝ"));
                    }
                }
                return str.ToString();
            }
        }
    }
}
