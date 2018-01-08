using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    public class StringHelper
    {
        /// <summary>
        /// 获取非null字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNotNullString(object str)
        {
            if (str == null || str == DBNull.Value)
            {
                str = string.Empty;
            }

            return str.ToString();
        }
    }
}
