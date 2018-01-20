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
        //private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);  

        /// <summary>
        /// 记录日志（log4net）
        /// </summary>
        /// <param name="info">日志信息</param>
        public static void Debug(object info)
        {
            //_Log.Debug(info);
        }
    }
}
