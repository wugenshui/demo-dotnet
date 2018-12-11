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
        CmdHelp cmdHelp = new CmdHelp();
        SynchronizationContext m_SyncContext = null;
        public string m_ReviceMsgStr = string.Empty;

        static void Main(string[] args)
        {

            //RunCmd2("", "node");
            //RunCmd2("", "1+1");
            new Program().StartADB();

            Console.ReadKey();
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
            CmdHelp.EventReceiveData += new CmdHelp.DelegateReceiveData(cmdHelp_EventReceiveData);
            CmdHelp.EventReceiveThreadData += new CmdHelp.DelegateReceiveThreadData(CmdHelp_EventReceiveThreadData);
            CmdHelp.EventReceiveThreadErrorData += new CmdHelp.DelegateReceiveThreadErrorData(CmdHelp_EventReceiveThreadErrorData);
            m_SyncContext = SynchronizationContext.Current;
            cmdHelp.StartServer("10010"); // 防止端口占用

            cmdHelp.StopApp("com.tencent.mm"); // 关闭程序
            string[] devices = cmdHelp.GetDevices();
            foreach (var device in devices)
            {
                Console.WriteLine("设备：" + device);
                Console.WriteLine("型号:" + cmdHelp.GetDeviceModel(device));
                Console.WriteLine("品牌:" + cmdHelp.GetDeviceBrand(device));
                Console.WriteLine("设备指纹:" + cmdHelp.GetDeviceFingerprint(device));
                Console.WriteLine("系统版本:" + cmdHelp.GetDeviceVersionRelease(device));
                Console.WriteLine("SDK版本:" + cmdHelp.GetDeviceVersionSdk(device));
                string[] apps = cmdHelp.GetAPP(device);
                foreach (var app in apps)
                {
                    Console.WriteLine("\t" + app);
                }
                string file = device + ".txt";
                string remote = "/sdcard/" + file;
                File.WriteAllText(file, string.Join("\r\n", apps));
                var pushResult = cmdHelp.FilePush(device, file, remote);
                Console.WriteLine("上传结果：" + pushResult);
                var pullResult = cmdHelp.FilePull(device, remote, "pull.txt");
                Console.WriteLine("下载结果：" + pullResult);
                var renameResult = cmdHelp.FileRename(device, remote, remote.Replace(device, "target"));
                Console.WriteLine("重命名结果：成功！");
            }

            Console.Read();
        }

        //接收到数据
        private void cmdHelp_EventReceiveData(string data)
        {
            //m_SyncContext.Post(setListData, data);
        }

        //接收错误数据
        private void CmdHelp_EventReceiveThreadErrorData(string data)
        {
            //m_SyncContext.Post(setListData, data);
        }


        void CmdHelp_EventReceiveThreadData(string data)
        {
            //m_SyncContext.Post(setListData, data);
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
