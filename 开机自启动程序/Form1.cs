using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Threading;
using log4net;
using System.Reflection;
using System.DirectoryServices;
using Microsoft.Web.Administration;

namespace 开机自启动程序
{
    public partial class Form1 : Form
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);   //日志
        private Dictionary<string, string> dic = new Dictionary<string, string>();          //文件名，文件路径+文件名

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowAutoStartImage();           //显示自启动软件
            ThreadPool.QueueUserWorkItem(new WaitCallback(startNew), "");   //检测软件是否退出
            ThreadPool.QueueUserWorkItem(new WaitCallback(start), "");      //检测IIS应用池是否关闭
        }

        private void button1_Click(object sender, EventArgs e)      //添加启动项
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fullpath = openFileDialog1.FileName;
                string exeName = Path.GetFileName(fullpath);        //软件名称
                SetAutoRun(fullpath);       //添加自启动
            }
            ShowAutoStartImage();           //显示自启动软件
            SoftQuitCheck();                //检测程序是否退出，若退出则重新启动
        }

        private void button2_Click(object sender, EventArgs e)      //删除启动项
        {
            ListView.SelectedListViewItemCollection lvs = listView1.SelectedItems;
            foreach (ListViewItem item in lvs)
            {
                string exeName = item.Name;        //软件名称
                SetAutoRun(exeName, false);        //删除自启动
            }
            ShowAutoStartImage();           //显示自启动软件
        }

        #region 将自启动软件转为图片，加入imageList1并显示在listView1上
        private void ShowAutoStartImage()
        {
            try
            {
                imageList1.Images.Clear();      //清空原imageList1
                listView1.Clear();              //清空原listView1
                dic.Clear();
                //this.listView1.LargeImageList = this.imageList1;
                //this.listView1.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
                RegistryKey local = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                string[] regs = local.GetValueNames();      //注册表中自启动软件名称
                foreach (string reg in regs)        //将图片加入imagelist
                {
                    object obj = local.GetValue(reg);       //软件名查询软件路径
                    string filePath = obj.ToString().Trim();      //软件路径+软件名称
                    if (filePath.EndsWith(".exe"))
                    {
                        Icon icon = ExtractAssociatedIcon(filePath);
                        Image img = Image.FromHbitmap(icon.ToBitmap().GetHbitmap());
                        img.Tag = reg;
                        imageList1.Images.Add(reg, img);
                        dic.Add(reg, filePath);         //加入dictionary
                    }
                }
                for (int i = 0; i < this.imageList1.Images.Count; i++)      //加入listView并显示
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = i;
                    item.Text = this.imageList1.Images.Keys[i].ToString();
                    item.Name = this.imageList1.Images.Keys[i].ToString();
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
            }
        }
        #endregion

        #region 设置应用程序开机自动运行
        /// <summary>
        /// 设置应用程序开机自动运行
        /// </summary>
        /// <param name="fileName">应用程序的文件名</param>
        /// <param name="isAutoRun">是否自动运行，为false时，取消自动运行</param>
        /// <exception cref="System.Exception">设置不成功时抛出异常</exception>
        public void SetAutoRun(string fileName, bool isAutoRun = true)
        {
            RegistryKey reg = null;
            try
            {
                if (isAutoRun)
                {
                    if (!System.IO.File.Exists(fileName))
                        throw new Exception("该文件不存在!");
                }
                string name = Path.GetFileName(fileName);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                {
                    reg.SetValue(name, fileName);
                    log.Info("添加启动项：" + name + "\t路径：" + fileName);
                }
                else
                {
                    reg.DeleteValue(name, false);
                    log.Info("删除启动项：" + name);
                }

            }
            catch (Exception ex)
            {
                log.Debug(ex);
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }
        #endregion

        #region 取出文件中icon
        #region Win32 API
        [System.Runtime.InteropServices.DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIcon")]
        private static extern IntPtr ExtractAssociatedIconA(
            IntPtr hInst,
            [System.Runtime.InteropServices.MarshalAs(
                 System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpIconPath,
            ref int lpiIcon);

        [System.Runtime.InteropServices.DllImport("shell32.dll", EntryPoint = "ExtractIcon")]
        private static extern IntPtr ExtractIconA(
            IntPtr hInst,
            [System.Runtime.InteropServices.MarshalAs(
                 System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpszExeFileName,
            int nIconIndex);
        private static IntPtr hInst = default(IntPtr);
        #endregion

        public static System.Drawing.Icon ExtractIcon(string fileName, int index)
        {
            if (System.IO.File.Exists(fileName) || System.IO.Directory.Exists(fileName))
            {
                System.IntPtr hIcon;

                // 文件所含图标的总数
                hIcon = ExtractIconA(hInst, fileName, -1);

                // 没取到的时候
                if (hIcon.Equals(IntPtr.Zero))
                {
                    // 取得跟文件相关的图标
                    return ExtractAssociatedIcon(fileName);
                }
                else
                {
                    // 图标的总数
                    int numOfIcons = hIcon.ToInt32();

                    if (0 <= index && index < numOfIcons)
                    {
                        hIcon = ExtractIconA(hInst, fileName, index);

                        if (!hIcon.Equals(IntPtr.Zero))
                        {
                            return System.Drawing.Icon.FromHandle(hIcon);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public static System.Drawing.Icon ExtractAssociatedIcon(string fileName)    //取出文件中icon
        {
            bool b1 = System.IO.File.Exists(fileName);
            bool b2 = System.IO.Directory.Exists(fileName);
            if (System.IO.File.Exists(fileName) || System.IO.Directory.Exists(fileName))
            {
                int i = 0;

                IntPtr hIcon = ExtractAssociatedIconA(hInst, fileName, ref i);

                if (!hIcon.Equals(IntPtr.Zero))
                {
                    return System.Drawing.Icon.FromHandle(hIcon);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 定时检测
        public void startNew(object obj)
        {
            while (true)       //死循环执行
            {
                try
                {
                    if (checkBox1.Checked)
                        SoftQuitCheck();
                }
                catch (Exception ex)
                {
                    log.Debug(ex);
                }
            }
        }

        private void start(object obj)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 60 * 1000;//执行间隔时间,单位为毫秒  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PoolQuitCheck();     //每分钟检测一次
        }
        #endregion

        #region 检测程序是否退出，若退出则重新启动
        public void SoftQuitCheck()
        {
            string exeName = string.Empty;
            bool isExist = false;       //软件是否在进程中存在
            try
            {
                System.Diagnostics.Process[] pros = System.Diagnostics.Process.GetProcesses();      //当前系统所有进程
                for (int i = 0; i < this.imageList1.Images.Count; i++)
                {
                    isExist = false;        //软件状态重置为不存在
                    exeName = this.imageList1.Images.Keys[i].ToString();
                    exeName = Path.GetFileNameWithoutExtension(exeName);
                    foreach (System.Diagnostics.Process item in pros)
                    {
                        string proName = item.ProcessName;
                        if (exeName == proName)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)    //如果软件不存在进程中,重新启动进程
                    {
                        if (dic.ContainsKey(exeName + ".exe"))
                        {
                            ProcessStartInfo info = new ProcessStartInfo();
                            info.FileName = dic[exeName + ".exe"];
                            info.Arguments = "";
                            info.WindowStyle = ProcessWindowStyle.Minimized;
                            Process pro = Process.Start(info);
                            log.Error("启动程序：" + dic[exeName + ".exe"]);
                            //pro.WaitForExit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
            }
        }
        #endregion

        #region IIS应用程序池辅助类
        public void PoolQuitCheck()
        {
            try
            {
                List<string> list = GetAppPools();
                foreach (string item in list)
                {
                    ApplicationPool pool = GetAppPool(item);
                    if (pool.State == ObjectState.Stopped || pool.State == ObjectState.Stopping)
                    {
                        pool.Start();
                        log.Info(item + "\t启动");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
            }
        }

        protected string Host = "localhost";
        /// <summary>
        ///     取得所有应用程序池
        /// </summary>
        /// <returns></returns>
        public List<string> GetAppPools()
        {
            var appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", Host));
            return (from DirectoryEntry entry in appPools.Children select entry.Name).ToList();
        }

        /// <summary>
        ///     取得单个应用程序池
        /// </summary>
        /// <returns></returns>
        public ApplicationPool GetAppPool(string appPoolName)
        {
            ApplicationPool app = null;
            var appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", Host));
            foreach (DirectoryEntry entry in appPools.Children)
            {
                if (entry.Name == appPoolName)
                {
                    var manager = new ServerManager();
                    app = manager.ApplicationPools[appPoolName];
                }
            }
            return app;
        }
        #endregion

        #region 右下角图标
        //关闭事件里，阻止之间关闭按钮（为了使得用户单机右下角的退出时，能够退出，所以必须使用CloseReason属性）
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        //双击icon还原
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = true;
            this.Show();
            this.Activate();
            this.WindowState = FormWindowState.Normal;
        }

        //右下角单机退出时，运行退出
        private void Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        #endregion
    }
}
