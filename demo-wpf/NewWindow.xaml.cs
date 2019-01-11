using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// NewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewWindow : Window
    {
        Window win = null;
        public NewWindow(Window win)
        {
            InitializeComponent();
            this.win = win;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            win.Show();
            this.Close();
        }
    }
}
