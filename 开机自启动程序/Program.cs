using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;

namespace 开机自启动程序
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Process instance = RunningInstance();
            if (instance == null)
            {
                Application.Run(new Form1());
            }
            else
            {
                HandleRunningInstance(instance);
            }
        }

        // 返回正在运行的程序进程
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/ ", "\\ ") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;    // 第一次运行，返回null
        }

        // 置窗口为正常状态,只有窗口最小化的时候可以达到此效果，如果隐藏到托盘则无法将打开的程序显示到桌面
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL); // 置窗口为正常状态
            SetForegroundWindow(instance.MainWindowHandle);
        }

        // 调用系统api
        [DllImport("User32.dll ")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;
    }
}
