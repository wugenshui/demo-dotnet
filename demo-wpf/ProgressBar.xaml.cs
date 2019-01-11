using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace demo_wpf
{
    /// <summary>
    /// 调用WPFSpark组件
    /// </summary>
    public partial class ProgressBar : Window
    {
        private BackgroundWorker bgWorker = new BackgroundWorker();
        private bool isBGWorking = false;
        public ProgressBar()
        {
            InitializeComponent();

            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnWorkCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(OnProgress);

            // 窗体加载完成事件
            Loaded += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isBGWorking)
            {
                isBGWorking = true;
                bgWorker.RunWorkerAsync();
            }
        }

        #region 进度条效果

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            StatusMessage msg = new StatusMessage();
            //pgBarDownload.Visibility = Visibility.Visible;
            msg.Message = string.Format("数据接口准备中！");
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);
            Thread.Sleep(1500);

            msg.Message = string.Format("开始同步！");
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);
            Thread.Sleep(1500);

            for (int i = 1; i < 10; i++)
            {
                msg.Message = string.Format("任务" + i + "进行中！");
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(100);
            }

            msg.Message = string.Format("快要完成！");
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);
            Thread.Sleep(1500);

            msg.Message = string.Format("任务已完成！");
            msg.IsAnimated = true;
            bgWorker.ReportProgress(100, msg);
            Thread.Sleep(1500);
        }

        private void OnWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pgBarDownload.Visibility = Visibility.Collapsed;
            isBGWorking = false;
        }

        private void OnProgress(object sender, ProgressChangedEventArgs e)
        {
            StatusMessage msg = e.UserState as StatusMessage;
            if (msg != null)
            {
                customStatusBar.SetStatus(msg.Message, msg.IsAnimated);
            }
        }

        #endregion
    }
}
