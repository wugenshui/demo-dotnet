using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_redis
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["connect"]);
            IDatabase db = redis.GetDatabase();
            string value = "1";
            //db.StringSet("mykey", value);
            value = db.StringGet("mykey");
            Console.WriteLine(value);
        }
    }
}
