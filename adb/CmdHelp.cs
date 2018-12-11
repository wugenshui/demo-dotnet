using System;
using System.Collections.Generic;
using System.IO;

namespace adb
{
    public class CmdHelp
    {
        #region 常用命令

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public string[] GetDevices()
        {
            var result = ProcessHelper.Run(AdbExePath, CmdAdbInfo.adb_devices);
            string itemsString = result.OutputString;
            EventReceiveData(itemsString);

            var items = itemsString.Split(new[] { "$", "#", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var itemsList = new List<string>();
            foreach (string item in items)
            {
                string temp = item.Trim();
                //第一行不含\t所以排除
                if (temp.IndexOf("\t") == -1)
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
        /// 获取设备安装的第三方APP
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <returns></returns>
        public string[] GetAPP(string deviceName)
        {
            var result = ProcessHelper.Run(AdbExePath, "-s " + deviceName + " " + CmdAdbInfo.adb_shell_pm_list_packages_3);
            var itemsString = result.OutputString;
            EventReceiveData(itemsString);
            string[] items = itemsString.Split(new[] { "$", "#", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return items;
        }

        /// <summary>
        /// 文件拉取
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="source">移动端的源路径</param>
        /// <param name="target">本机目标路径</param>
        /// <returns></returns>
        public string FilePull(string deviceName, string source, string target)
        {
            var result = ProcessHelper.Run(AdbExePath, "-s " + deviceName + " " + CmdAdbInfo.adb_pull + " " + source + " " + target);
            var itemsString = result.OutputString;

            return itemsString;
        }

        /// <summary>
        /// 文件推送
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="source">本机源路径</param>
        /// <param name="target">移动端的目标路径</param>
        /// <returns></returns>
        public string FilePush(string deviceName, string source, string target)
        {
            var result = ProcessHelper.Run(AdbExePath, "-s " + deviceName + " " + CmdAdbInfo.adb_push + " " + source + " " + target);
            var itemsString = result.OutputString;

            return itemsString;
        }

        /// <summary>
        /// 移动端文件重命名 adb -s OVBIAQMJ8LKNPJYD shell rename /sdcard/1.txt  /sdcard/2.txt
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="source">移动端的源路径</param>
        /// <param name="target">移动端的目标路径</param>
        /// <returns></returns>
        public string FileRename(string deviceName, string source, string target)
        {
            var result = ProcessHelper.Run(AdbExePath, "-s " + deviceName + " shell " + CmdAdbInfo.adb_rename + " " + source + " " + target);
            var itemsString = result.OutputString;

            return itemsString;
        }

        /// <summary>
        /// 发送adb 命令（同步）
        /// </summary>
        /// <returns></returns>
        public void SendAdbCmd(string deviceStr, string cmdStr)
        {
            CmdProcessHelper.Run(AdbExePath, CmdAdbInfo.adb_universal_s + " " + deviceStr + " " + cmdStr);
        }

        /// <summary>
        /// 获取设备文件内容
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        public string GetFileContent(string deviceNo, string cmdStr)
        {
            var result = ProcessHelper.Run(AdbExePath, string.Format("-s {0} {1}", deviceNo, cmdStr));
            EventReceiveData(result.OutputString.Trim());
            return result.OutputString.Trim();
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="port"></param>
        public void StartServer(string port)
        {
            CmdProcessHelper.Run(AdbExePath, CmdAdbInfo.adb_universal_p + " " + port + " " + CmdAdbInfo.adb_start_server);
        }

        /// <summary>
        /// 退出服务
        /// </summary>
        public void KillServer()
        {
            CmdProcessHelper.Run(AdbExePath, CmdAdbInfo.adb_kill_server);
        }

        /// <summary>
        /// 重启移动设备
        /// </summary>
        /// <param name="port"></param>
        public void ReBoot(string port)
        {
            CmdProcessHelper.Run(AdbExePath, CmdAdbInfo.adb_universal_p + " " + port + " " + CmdAdbInfo.adb_reboot);
        }

        #endregion

        //adb路径
        public static string AdbExePath
        {
            get
            {
                return Path.Combine(System.Environment.CurrentDirectory, "lib\\adb.exe");
            }
        }

        #region 委托、事件

        //委托(接收到原始数据)
        /// <summary>
        /// 委托(接收到原始数据)
        /// </summary>
        /// <param name="state"></param>
        public delegate void DelegateReceiveData(string data);

        //接收到原始数据
        /// <summary>
        /// 接收到原始数据
        /// </summary>
        public static event DelegateReceiveData EventReceiveData;

        //委托(接收到原始数据)
        /// <summary>
        /// 委托(接收到原始数据)
        /// </summary>
        /// <param name="state"></param>
        public delegate void DelegateReceiveThreadData(string data);

        //接收到原始数据
        /// <summary>
        /// 接收到原始数据
        /// </summary>
        public static event DelegateReceiveThreadData EventReceiveThreadData;

        //委托(接收到错误数据)
        /// <summary>
        /// 委托(接收到错误数据)
        /// </summary>
        /// <param name="state"></param>
        public delegate void DelegateReceiveThreadErrorData(string data);

        //接收到错误数据
        /// <summary>
        /// 接收到错误数据
        /// </summary>
        public static event DelegateReceiveThreadErrorData EventReceiveThreadErrorData;

        /**触发事件回调结果*/
        public static void PutResultToUI(string receiveMsg)
        {
            EventReceiveData(receiveMsg);
        }

        /**触发事件回调结果(线程)*/
        public static void PutResultThreadToUI(string receiveMsg)
        {
            EventReceiveThreadData(receiveMsg);
        }

        /**触发事件回调错误结果(线程)*/
        public static void PutResultThreadErrorToUI(string receiveMsg)
        {
            EventReceiveThreadErrorData(receiveMsg);
        }

        #endregion

        /// <summary>
        /// 发送adb 命令(异步)
        /// </summary>
        /// <returns></returns>
        public void AysnSendAdbCmd(string deviceStr, string cmdStr)
        {
            CmdProcessHelper.AysnRun(AdbExePath, CmdAdbInfo.adb_universal_s + " " + deviceStr + " " + cmdStr);
        }

        /// <summary>
        /// 卸载软件
        /// </summary>
        /// <returns></returns>
        public void APPUninstall(string packageName)
        {
            var result = ProcessHelper.Run(AdbExePath, "uninstall " + packageName);
            var itemsString = result.OutputString;
            EventReceiveData(itemsString);
        }

        #region 获取设备相关信息

        /// <summary>
        /// -s 0123456789ABCDEF shell getprop ro.product.brand
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        public string GetDeviceProp(string deviceNo, string propKey)
        {
            var result = ProcessHelper.Run(AdbExePath, string.Format("-s {0} shell getprop {1}", deviceNo, propKey));
            EventReceiveData(result.OutputString.Trim());
            return result.OutputString.Trim();
        }
        /// <summary>
        /// 型号：[ro.product.model]: [Titan-6575]
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public string GetDeviceModel(string deviceNo)
        {
            return GetDeviceProp(deviceNo, "ro.product.model");
        }
        /// <summary>
        /// 牌子：[ro.product.brand]: [Huawei]
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public string GetDeviceBrand(string deviceNo)
        {
            return GetDeviceProp(deviceNo, "ro.product.brand");
        }
        /// <summary>
        /// 设备指纹：[ro.build.fingerprint]: [Huawei/U8860/hwu8860:2.3.6/HuaweiU8860/CHNC00B876:user/ota-rel-keys,release-keys]
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public string GetDeviceFingerprint(string deviceNo)
        {
            return GetDeviceProp(deviceNo, "ro.build.fingerprint");
        }
        /// <summary>
        /// 系统版本：[ro.build.version.release]: [4.1.2]
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public string GetDeviceVersionRelease(string deviceNo)
        {
            return GetDeviceProp(deviceNo, "ro.build.version.release");
        }
        /// <summary>
        /// SDK版本：[ro.build.version.sdk]: [16]
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public string GetDeviceVersionSdk(string deviceNo)
        {
            return GetDeviceProp(deviceNo, "ro.build.version.sdk");
        }
        #endregion
    }
}
