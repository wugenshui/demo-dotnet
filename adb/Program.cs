using System;
using System.Collections.Generic;
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
            var m_SyncContext = SynchronizationContext.Current;
            new Program().StartADB();
        }

        void StartADB()
        {
            // 注册事件(adb,Android Debug Bridge)
            CmdHelp.EventReceiveData += new CmdHelp.DelegateReceiveData(cmdHelp_EventReceiveData);
            CmdHelp.EventReceiveThreadData += new CmdHelp.DelegateReceiveThreadData(CmdHelp_EventReceiveThreadData);
            CmdHelp.EventReceiveThreadErrorData += new CmdHelp.DelegateReceiveThreadErrorData(CmdHelp_EventReceiveThreadErrorData);
            m_SyncContext = SynchronizationContext.Current;

            cmdHelp.StartServer("10010"); // 防止端口占用
            string[] devices = cmdHelp.GetDevices();
            foreach (var name in devices)
            {
                Console.WriteLine("设备：" + name);
                string[] apps = cmdHelp.GetAPP(name);
                foreach (var app in apps)
                {
                    Console.WriteLine("\t" + app);
                }
                string file = name + ".txt";
                string remote = "/sdcard/" + file;
                File.WriteAllText(file, string.Join(",", apps));
                var pushResult = cmdHelp.FilePush(name, file, remote);
                Console.WriteLine("上传结果：" + pushResult);
                var pullResult = cmdHelp.FilePull(name, remote, "pull.txt");
                Console.WriteLine("下载结果：" + pullResult);
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
