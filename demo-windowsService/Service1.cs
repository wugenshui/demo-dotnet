﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
            timer = new Timer(20000); // 单位：毫秒
            timer.Elapsed += new ElapsedEventHandler(Timer_Click); //Timer_Click是到达时间的时候执行事件的函数
            timer.AutoReset = true; //设置是执行一次（false）还是一直执行(true)
            timer.Enabled = true; //是否执行System.Timers.Timer.Elapsed事件
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        private void Timer_Click(Object sender, ElapsedEventArgs e)
        {
            NoUI();
        }

        /// <summary>
        /// 服务启动程序 不显示界面
        /// </summary>
        private void NoUI()
        {
            string path = "G://demo-winform.exe";
            Process[] localByName = Process.GetProcessesByName("demo-winform.exe");
            //这里的360tray.exe就是你想要执行的程序的进程的名称。基本上就是.exe文件的文件名。
            //localByName得到的是进程中所有同名的进程。
            if (localByName.Length == 0) //如果得到的进程数是0, 那么说明程序未启动，需要启动程序
            {
                Process.Start(path); //启动程序 
            }
            else
            {
                //如果程序已经启动，则执行这一部分代码
            }
        }
    }
}
