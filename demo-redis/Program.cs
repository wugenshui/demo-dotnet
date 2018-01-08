using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace demo_redis
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisHelper redis = new RedisHelper();
            //bool state = redis.Set("name", "陈博");

            UserInfo user = new UserInfo() { Age = 18, Id = 1, UserName = "张三" };
            for (int i = 0; i < 100000; i++)
            {
                bool b = redis.Set<UserInfo>("n", user);
            }


            UserInfo name = redis.Get<UserInfo>("n");
        }

        public class UserInfo
        {
            public int Id;
            public string UserName;
            public int Age;
        }

        public bool Set(string key, string value)
        {
            string host = ConfigurationManager.AppSettings["redisPath"];
            RedisClient redisClient = new RedisClient(host);
            return redisClient.Set(key, value);
        }

        public string Get(string key)
        {
            string host = ConfigurationManager.AppSettings["redisPath"];
            RedisClient redisClient = new RedisClient(host);
            return redisClient.Get<string>(key);
        }
    }
}
