using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNettyServer
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
    }
}