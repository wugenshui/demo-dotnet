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
            Current.DispatcherUnhandledException += (sender, e) =>
            {
                MessageBox.Show("UI线程全局异常！" + Environment.NewLine + (e.Exception));
                e.Handled = true;
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show("非UI线程全局异常！" + Environment.NewLine + (e.ExceptionObject as Exception));
            };
        }
    }
}
