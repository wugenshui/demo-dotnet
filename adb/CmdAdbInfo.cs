using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adb
{
    public class CmdAdbInfo
    {
        /* 指定设备 */
        public static string adb_universal_s = "-s";
        public static string adb_universal_p = "-P";
        /**启动adb服务*/
        public static string adb_start_server = "start-server";
        /**停止adb服务*/
        public static string adb_kill_server = "kill-server";
        /**重启adb服务*/
        public static string adb_reboot = "reboot";
        /**获取adb的版本信息*/
        public static string adb_version = "version";
        /**获取连接的设备信息*/
        public static string adb_devices = "devices";
        /**列出手机装的所有app包名*/
        public static string adb_shell_pm_list_packages = "shell pm list packages";
        /**列出系统应用app包名*/
        public static string adb_shell_pm_list_packages_s = "shell pm list packages s";
        /**列出第三方应用app包名*/
        public static string adb_shell_pm_list_packages_3 = "shell pm list packages -3";
        /**安装软件*/
        public static string adb_install = "install";
        /**卸载软件*/
        public static string adb_uninstall = "uninstall";
        /**电池状况*/
        public static string adb_shell_dumpsys_battery = "shell dumpsys battery";
        /**屏幕分辨率*/
        public static string adb_shell_wm_size = "shell wm size";
        /**屏幕信息*/
        public static string adb_shell_dumpsys_window_displays = "shell dumpsys window displays";
        /**网络配置*/
        public static string adb_shell_netcfg = "shell netcfg";
        /**读取设备文件内容**/
        public static string adb_shell_cat = "shell cat";
        /**root*/
        public static string adb_root = "root";
        /**复制设备文件到电脑**/
        public static string adb_pull = "pull";
        /**复制电脑文件到设备**/
        public static string adb_push = "push";
        // 重命名
        public static string adb_rename = "rename";

        /**获取所有adb指令*/
        public static List<string> GetAllADBCmdList()
        {
            List<string> itemsList = new List<string>();
            itemsList.Add(adb_start_server);
            itemsList.Add(adb_kill_server);
            itemsList.Add(adb_reboot);
            itemsList.Add(adb_version);
            itemsList.Add(adb_devices);
            itemsList.Add(adb_shell_pm_list_packages);
            itemsList.Add(adb_shell_pm_list_packages_s);
            itemsList.Add(adb_shell_pm_list_packages_3);
            itemsList.Add(adb_install);
            itemsList.Add(adb_uninstall);
            itemsList.Add(adb_shell_dumpsys_battery);
            itemsList.Add(adb_shell_wm_size);
            itemsList.Add(adb_shell_dumpsys_window_displays);
            itemsList.Add(adb_shell_netcfg);
            itemsList.Add(adb_shell_cat);
            itemsList.Add(adb_root);
            itemsList.Add(adb_pull);
            itemsList.Add(adb_push);
            itemsList.Sort();
            return itemsList;
        }

    }
}
