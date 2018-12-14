using System;
using System.Collections.Generic;
using System.IO;

namespace adb
{
    public class AdbHelp
    {
        private static string adbPath = Path.Combine(Environment.CurrentDirectory, "lib\\adb.exe");

        /// <summary>
        /// 发送adb全局命令
        /// </summary>
        /// <param name="cmdStr">命令参数</param>
        /// <returns></returns>
        public static string Run(string cmdStr)
        {
            return ProcessHelper.Run(adbPath, cmdStr).Trim();
        }

        /// <summary>
        /// 发送adb指定设备命令（同步）
        /// </summary>
        /// <param name="deviceStr">设备序列号</param>
        /// <param name="cmdStr">命令参数</param>
        /// <returns></returns>
        public static string Run(string deviceStr, string cmdStr)
        {
            return Run("-s " + deviceStr + " " + cmdStr);
        }

        #region 常用命令

        /// <summary>
        /// 获取连接的设备序列号
        /// </summary>
        /// <returns>设备序列号集合</returns>
        public static string[] GetDevices()
        {
            var cmdResult = Run("devices");
            var items = cmdResult.Split(new[] { "$", "#", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var itemsList = new List<string>();
            foreach (string item in items)
            {
                string temp = item.Trim();
                if (temp.IndexOf("\t") == -1) // 第一行不含\t所以排除
                {
                    continue;
                }
                var tmps = item.Split('\t');
                itemsList.Add(tmps[0]);
            }
            itemsList.Sort();
            return itemsList.ToArray();
        }

        /// <summary>
        /// 文件拉取
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <param name="source">移动端的源路径</param>
        /// <param name="target">本机目标路径</param>
        /// <returns></returns>
        public static string FilePull(string serialNo, string source, string target)
        {
            return Run("-s " + serialNo + " pull " + source + " " + target);
        }

        /// <summary>
        /// 文件推送
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <param name="source">本机源路径</param>
        /// <param name="target">移动端的目标路径</param>
        /// <returns></returns>
        public static string FilePush(string serialNo, string source, string target)
        {
            return Run("-s " + serialNo + " push " + source + " " + target);
        }

        /// <summary>
        /// 移动端文件重命名 adb -s OVBIAQMJ8LKNPJYD shell rename /sdcard/1.txt  /sdcard/2.txt
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <param name="source">移动端的源路径</param>
        /// <param name="target">移动端的目标路径</param>
        /// <returns></returns>
        public static string FileRename(string serialNo, string source, string target)
        {
            return Run("-s " + serialNo + " shell rename " + source + " " + target);
        }

        /// <summary>
        /// 获取设备安装的第三方APP
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <returns></returns>
        public static string[] GetAPP(string serialNo)
        {
            var cmdResult = Run(string.Format("-s {0} shell pm list packages -3", serialNo));
            string[] items = cmdResult.Split(new[] { "$", "#", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return items;
        }

        /// <summary>
        /// 检测App是否运行
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <param name="appName">App包名</param>
        public bool IsAppRun(string serialNo, string appName)
        {
            bool hasApp = false;
            var cmdResult = Run(serialNo, string.Format("shell \"ps | grep {0}\"", appName));

            var items = cmdResult.Split(new[] { "$", "#", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                if (item.Contains(appName))
                {
                    hasApp = true;
                    break;
                }
            }

            return hasApp;
        }

        /// <summary>
        /// 终止App运行
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <param name="appName">app包名</param>
        public static void StopApp(string serialNo, string appName)
        {
            Run(serialNo, " shell am force-stop " + appName);
        }

        /// <summary>
        /// 获取设备文件内容
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        public static string GetFileContent(string serialNo, string cmdStr)
        {
            return Run(string.Format("-s {0} {1}", serialNo, cmdStr));
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="port">端口</param>
        public static void StartServer(string port)
        {
            Run(string.Format("-P {0} start-server", port));
        }

        /// <summary>
        /// 退出服务
        /// </summary>
        public static void KillServer()
        {
            Run("kill-server");
        }

        /// <summary>
        /// 重启移动设备
        /// </summary>
        /// <param name="port">端口</param>
        public static void ReBoot(string port)
        {
            Run("-P " + port + " reboot");
        }

        #endregion

        /// <summary>
        /// 卸载软件
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <returns></returns>
        public static string APPUninstall(string packageName)
        {
            return Run("uninstall " + packageName);
        }

        #region 获取设备相关信息

        /// <summary>
        /// -s 0123456789ABCDEF shell getprop ro.product.brand
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <param name="propKey">属性名称</param>
        /// <returns></returns>
        public static string GetDeviceProp(string serialNo, string propKey)
        {
            return Run(string.Format("-s {0} shell getprop {1}", serialNo, propKey));
        }

        /// <summary>
        /// 获取设备型号：[ro.product.model]: [Titan-6575]
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <returns></returns>
        public static string GetDeviceModel(string serialNo)
        {
            return GetDeviceProp(serialNo, "ro.product.model");
        }

        /// <summary>
        /// 获取设备品牌：[ro.product.brand]: [Huawei]
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <returns></returns>
        public static string GetDeviceBrand(string serialNo)
        {
            return GetDeviceProp(serialNo, "ro.product.brand");
        }

        /// <summary>
        /// 获取设备指纹：[ro.build.fingerprint]: [Huawei/U8860/hwu8860:2.3.6/HuaweiU8860/CHNC00B876:user/ota-rel-keys,release-keys]
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <returns></returns>
        public static string GetDeviceFingerprint(string serialNo)
        {
            return GetDeviceProp(serialNo, "ro.build.fingerprint");
        }

        /// <summary>
        /// 获取设备系统版本：[ro.build.version.release]: [4.1.2]
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <returns></returns>
        public static string GetDeviceVersionRelease(string serialNo)
        {
            return GetDeviceProp(serialNo, "ro.build.version.release");
        }

        /// <summary>
        /// 获取设备SDK版本：[ro.build.version.sdk]: [16]
        /// </summary>
        /// <param name="serialNo">设备序列号</param>
        /// <returns></returns>
        public static string GetDeviceVersionSdk(string serialNo)
        {
            return GetDeviceProp(serialNo, "ro.build.version.sdk");
        }

        #endregion
    }
}
