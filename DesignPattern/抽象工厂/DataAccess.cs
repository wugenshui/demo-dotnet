using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class DataAccess
    {
        private static readonly string db = ConfigurationManager.AppSettings["db"];

        public static IUser CreateUser()
        {
            IUser result = null;
            result = (IUser)Assembly.Load("DesignPattern").CreateInstance("DesignPattern." + db + "User");
            return result;
        }
    }
}
