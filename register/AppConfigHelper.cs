using System;
using System.Configuration;
using System.Reflection;
using System.Xml;

namespace Register
{
    public static class AppConfigHelper
    {
        private static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>  
        /// 获得键值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings["key"];
        }

        /// <summary>  
        /// 修改键值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void ModifyKey(string key, string value)
        {
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);

            config.Save();
        }

        /// <summary>  
        /// 添加键值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void AddKey(string key, string value)
        {
            config.AppSettings.Settings.Add(key, value);
            config.Save();
        }

        /// <summary>  
        /// 移除键值  
        /// </summary>  
        /// <param name="key"></param>  
        public static void DeleteKey(string key)
        {
            config.AppSettings.Settings.Remove(key);
            config.Save();
        }
    }
}

