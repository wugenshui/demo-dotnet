using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace adb
{
    class Program
    {
        public string m_ReviceMsgStr = string.Empty;
        static int total = 0;
        static int threadCount = 50;
        static int exeCount = 2;

        static void Main(string[] args)
        {
            var adbProcess = Process.GetProcessesByName("adb");
            int adbCount = 0;
            foreach (Process adb in adbProcess)
            {
                adb.Kill();
                Console.WriteLine("杀死" + (++adbCount) + "个adb进程!!!!!!!!!!!!!!");
            }

            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(TestClose);
                thread.Name = "线程" + (i + 1).ToString("00");
                thread.Start();
            }

            while (true)
            {
                Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("..............开始新一轮遍历..............");
                total = 0;
                for (int i = 0; i < threadCount; i++)
                {
                    Thread thread = new Thread(TestClose);
                    thread.Name = "线程" + (i + 1).ToString("00");
                    thread.Start();
                }
            }

            //Console.ReadKey();
        }

        static void TestClose()
        {
            int counter = 0;

            AdbHelp.StartServer("10010");
            string[] devices = AdbHelp.GetDevices();
            string deviceName = devices.Length > 0 ? devices[0] : "0123456789ABCDEF";
            for (int i = 0; i < exeCount; i++)
            {
                AdbHelp.StopApp(deviceName, "com.tencent.mm");
                AdbHelp.FilePull(deviceName, "/sdcard/sdms/dbs/PdaSDMS.db", Environment.CurrentDirectory);
                AdbHelp.FilePull(deviceName, Environment.CurrentDirectory, "/sdcard/sdms/dbs/1.db");
                AdbHelp.FileRename(deviceName, "/sdcard/sdms/dbs/1.db", "/sdcard/sdms/dbs/2.db");
                AdbHelp.GetDeviceModel(deviceName);
                Console.WriteLine(string.Format("{0}:当前执行条数{1}....................{2}/{3}", Thread.CurrentThread.Name, ++counter, ++total, threadCount * exeCount));
            }
        }

        static void RunCmd()
        {
            Console.WriteLine("请输入要执行的命令:");
            string strInput = Console.ReadLine();
            Process p = new Process();
            // 设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            // 是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            // 输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            // 不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            // 启动程序
            p.Start();

            // 向cmd窗口发送输入信息
            //p.StandardInput.WriteLine(strInput + "&exit");
            p.StandardInput.WriteLine(strInput + "/r/n");

            p.StandardInput.AutoFlush = true;

            // 获取输出信息
            string strOuput = p.StandardOutput.ReadToEnd();
            string[] strs = strOuput.Split(new string[] { "/r/n" }, StringSplitOptions.RemoveEmptyEntries);
            // 等待程序执行完退出进程
            p.WaitForExit();
            p.Close();

            Console.WriteLine(strOuput);
        }

        /// <summary>
        /// 运行cmd命令
        /// 不显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        static string RunCmd2(string cmdExe, string cmdStr)
        {
            string output = string.Empty;
            try
            {
                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = "cmd.exe";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                    string str = string.Empty;
                    if (string.IsNullOrWhiteSpace(cmdExe))
                    {
                        str = string.Format(@"{0} {1}", cmdStr, "&exit");
                    }
                    else
                    {
                        str = string.Format(@"""{0}"" {1} {2}", cmdExe, cmdStr, "&exit");
                    }

                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.WriteLine("1+1");
                    myPro.StandardInput.WriteLine("exit");
                    myPro.StandardInput.AutoFlush = true;
                    output = myPro.StandardOutput.ReadToEnd();
                    myPro.WaitForExit();
                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        void StartADB()
        {
            // 注册事件(adb,Android Debug Bridge)
            AdbHelp.StartServer("10010"); // 防止端口占用

            string[] devices = AdbHelp.GetDevices();
            foreach (var device in devices)
            {
                AdbHelp.StopApp(device, "com.tencent.mm");
                Console.WriteLine("设备：" + device);
                Console.WriteLine("型号:" + AdbHelp.GetDeviceModel(device));
                Console.WriteLine("品牌:" + AdbHelp.GetDeviceBrand(device));
                Console.WriteLine("设备指纹:" + AdbHelp.GetDeviceFingerprint(device));
                Console.WriteLine("系统版本:" + AdbHelp.GetDeviceVersionRelease(device));
                Console.WriteLine("SDK版本:" + AdbHelp.GetDeviceVersionSdk(device));
                string[] apps = AdbHelp.GetAPP(device);
                foreach (var app in apps)
                {
                    Console.WriteLine("\t" + app);
                }
                string file = device + ".txt";
                string remote = "/sdcard/" + file;
                File.WriteAllText(file, string.Join("\r\n", apps));
                var pushResult = AdbHelp.FilePush(device, file, remote);
                Console.WriteLine("上传结果：" + pushResult);
                var pullResult = AdbHelp.FilePull(device, remote, "pull.txt");
                Console.WriteLine("下载结果：" + pullResult);
                var renameResult = AdbHelp.FileRename(device, remote, remote.Replace(device, "target"));
                Console.WriteLine("重命名结果：成功！");
            }

            Console.Read();
        }

        private void setListData(object data)
        {
            if (data != null)
            {
                m_ReviceMsgStr = data.ToString();
            }
        }
    }
}
