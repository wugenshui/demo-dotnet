using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace demo_wpf
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                MessageBox.Show(e.Exception.Message);
                e.SetObserved();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show("出现异常！请联系获取支持！" + Environment.NewLine + (e.ExceptionObject as Exception));
            };
        }
    }
}
