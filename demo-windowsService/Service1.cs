using Cjwdev.WindowsApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace demo_windowsService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer(1000); // 单位：毫秒
            timer.Elapsed += new ElapsedEventHandler(Timer_Click); // Timer_Click是到达时间的时候执行事件的函数
            timer.AutoReset = true; // 执行一次（false）还是一直执行(true)
            timer.Enabled = true; // 是否执行Timer.Elapsed事件
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        private void Timer_Click(Object sender, ElapsedEventArgs e)
        {
            HasUI();
        }

        /// <summary>
        /// 服务启动程序 不显示界面
        /// </summary>
        private void NoUI()
        {
            string appName = "demo-winform";
            string appPath = "G://demo-winform.exe";
            Process[] localByName = Process.GetProcessesByName(appName);
            if (localByName.Length == 0) //如果得到的进程数是0, 那么说明程序未启动，需要启动程序
            {
                Process.Start(appPath); //启动程序 
            }
            else
            {
                //如果程序已经启动，则执行这一部分代码
            }
        }

        /// <summary>
        /// 服务启动程序 显示界面
        /// </summary>
        private void HasUI()
        {
            string appName = "demo-winform";
            string appPath = "G://demo-winform.exe";
            Process[] localByName = Process.GetProcessesByName(appName);
            if (localByName.Length == 0) //如果得到的进程数是0, 那么说明程序未启动，需要启动程序
            {
                try
                {
                    IntPtr userTokenHandle = IntPtr.Zero;
                    ApiDefinitions.WTSQueryUserToken(ApiDefinitions.WTSGetActiveConsoleSessionId(), ref userTokenHandle);

                    ApiDefinitions.PROCESS_INFORMATION procInfo = new ApiDefinitions.PROCESS_INFORMATION();
                    ApiDefinitions.STARTUPINFO startInfo = new ApiDefinitions.STARTUPINFO();
                    startInfo.cb = (uint)Marshal.SizeOf(startInfo);

                    ApiDefinitions.CreateProcessAsUser(
                        userTokenHandle,
                        appPath,
                        "",
                        IntPtr.Zero,
                        IntPtr.Zero,
                        false,
                        0,
                        IntPtr.Zero,
                        null,
                        ref startInfo,
                        out procInfo);

                    if (userTokenHandle != IntPtr.Zero)
                        ApiDefinitions.CloseHandle(userTokenHandle);

                    int _currentAquariusProcessId = (int)procInfo.dwProcessId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("启动程序失败！" + ex);
                }

            }
            else
            {
                //如果程序已经启动，则执行这一部分代码
            }
        }
    }
}
