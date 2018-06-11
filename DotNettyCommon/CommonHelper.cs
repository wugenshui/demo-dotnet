using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyCommon
{
    public class CommonHelper
    {
        public static bool IsSsl
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["IsSsl"]);
            }
        }

        public static int Port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            }
        }

        public static string ProcessDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessDirectory"];
            }
        }

        public static IPAddress Host
        {
            get
            {
                return IPAddress.Parse(ConfigurationManager.AppSettings["Host"]);
            }
        }

        public static int Size
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["Size"]);
            }
        }

        public const string SYS = "SYS";
    }
}
