using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    public class LogHelper
    {
        // 日志纪录对象
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="info">日志信息</param>
        public static void Error(object info)
        {
            _Log.Error(info);
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="info">日志信息</param>
        public static void Debug(object info)
        {
            _Log.Debug(info);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="info">日志信息</param>
        public static void Info(object info)
        {
            _Log.Info(info);
        }
    }
}
