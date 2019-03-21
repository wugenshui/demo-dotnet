using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace common
{
    /// <summary>
    /// 调用浏览器类
    /// </summary>
    public static class BrowserHelper
    {
        /// <summary>
        /// ie浏览器程序名称
        /// </summary>
        private const string IE_NAME = "iexplore.exe";
        private const string IE64_PATH = @"C:\Program Files\Internet Explorer\iexplore.exe";
        private const string IE32_PATH = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe";
        private const string CHROME_NAME = "chrome.exe";
        /// <summary>
        /// Chrome 64位注册表路径
        /// </summary>
        private const string CHROME64_OPENKEY = @"SOFTWARE\Wow6432Node\Google\Chrome";
        /// <summary>
        /// Chrome 32位注册表路径
        /// </summary>
        private const string CHROME32_OPENKEY = @"SOFTWARE\Google\Chrome";

        /// <summary>
        /// 调用系统浏览器打开网页
        /// </summary>
        /// <param name="url">打开网页的链接</param>
        public static string OpenBrowserUrl(string url)
        {
            string errorMsg = string.Empty;
            try
            {
                var openKey = CHROME64_OPENKEY;
                if (IntPtr.Size == 4)
                {
                    openKey = CHROME32_OPENKEY;
                }
                RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);
                if (appPath != null)
                {
                    // 系统安装过谷歌浏览器就用谷歌打开
                    var result = Process.Start(CHROME_NAME, url);
                    if (result == null)
                    {
                        // 未找到就用ie浏览器
                        OpenIe(url);
                    }
                }
                else
                {
                    // 最后使用系统默认的浏览器
                    OpenDefaultBrowserUrl(url);
                }
            }
            catch
            {
                // 谷歌卸载了，注册表还没有清空，程序会返回一个"系统找不到指定的文件。"的bug
                OpenDefaultBrowserUrl(url);
            }
            return errorMsg;
        }

        /// <summary>
        /// 用IE打开浏览器
        /// </summary>
        /// <param name="url"></param>
        public static void OpenIe(string url)
        {
            try
            {
                Process.Start(IE_NAME, url);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                // IE浏览器路径安装：C:\Program Files\Internet Explorer
                // at System.Diagnostics.process.StartWithshellExecuteEx(ProcessStartInfo startInfo)注意这个错误
                try
                {
                    if (File.Exists(IE64_PATH))
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = IE64_PATH,
                            Arguments = url,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(processStartInfo);
                    }
                    else
                    {
                        if (File.Exists(IE32_PATH))
                        {
                            ProcessStartInfo processStartInfo = new ProcessStartInfo
                            {
                                FileName = IE32_PATH,
                                Arguments = url,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };
                            Process.Start(processStartInfo);
                        }
                        else
                        {
                            //if (MessageBox.Show("系统未安装IE浏览器，是否下载安装？", null, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                            //{
                            //    // 打开下载链接，从微软官网下载
                            //    OpenDefaultBrowserUrl("http://windows.microsoft.com/zh-cn/internet-explorer/download-ie");
                            //}
                        }
                    }
                }
                catch (Exception exception)
                {
                    //MessageBox.Show(exception.Message);
                }
            }
        }

        /// <summary>
        /// 打开系统默认浏览器（用户自己设置了默认浏览器）
        /// </summary>
        /// <param name="url"></param>
        public static void OpenDefaultBrowserUrl(string url)
        {
            try
            {
                // 方法1
                //从注册表中读取默认浏览器可执行文件路径
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                if (key != null)
                {
                    string s = key.GetValue("").ToString();
                    //s就是你的默认浏览器，不过后面带了参数，把它截去，不过需要注意的是：不同的浏览器后面的参数不一样！
                    //"D:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
                    var lastIndex = s.IndexOf(".exe", StringComparison.Ordinal);
                    var path = s.Substring(1, lastIndex + 3);
                    var result = Process.Start(path, url);
                    if (result == null)
                    {
                        // 方法2
                        // 调用系统默认的浏览器 
                        var result1 = Process.Start("explorer.exe", url);
                        if (result1 == null)
                        {
                            // 方法3
                            Process.Start(url);
                        }
                    }
                }
                else
                {
                    // 方法2
                    // 调用系统默认的浏览器 
                    var result1 = Process.Start("explorer.exe", url);
                    if (result1 == null)
                    {
                        // 方法3
                        Process.Start(url);
                    }
                }
            }
            catch
            {
                OpenIe(url);
            }
        }
    }
}
