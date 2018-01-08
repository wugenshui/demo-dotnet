using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace demo_redis
{
    /// <summary>  
    /// Redis公共辅助类库  
    /// </summary>  
    public class RedisHelper : IDisposable
    {
        public RedisClient Redis;
        PooledRedisClientManager prcm = new PooledRedisClientManager();  //缓存池  
        public int secondsTimeOut = 30 * 60;    //默认缓存过期时间  单位秒  

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="openPooledRedis">是否开启缓冲池</param>  
        public RedisHelper(bool openPooledRedis = false)
        {
            string redisPath = ConfigurationManager.AppSettings["redisPath"];
            if (openPooledRedis)
            {
                prcm = CreateManager(new string[] { redisPath }, new string[] { redisPath });
                Redis = prcm.GetClient() as RedisClient;
            }
            else
            {
                Redis = new RedisClient(redisPath);
            }
        }

        /// <summary>  
        /// 缓冲池  
        /// </summary>  
        /// <param name="readWriteHosts">读写服务器</param>  
        /// <param name="readOnlyHosts">只读服务器</param>  
        /// <returns></returns>  
        public static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = readWriteHosts.Length * 5,
                MaxReadPoolSize = readOnlyHosts.Length * 5,
                AutoStart = true,
            });
            // { RedisClientFactory = (IRedisClientFactory)RedisCacheClientFactory.Instance.CreateRedisClient("127.0.0.1", 6379) };            
        }


        #region Key/Value存储
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存建</param>
        /// <param name="t">缓存值</param>  
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>  
        /// <returns></returns>  
        public bool Set<T>(string key, T t, int timeout = 0)
        {
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(key, secondsTimeOut);
            }

            return Redis.Add<T>(key, t);
        }
        /// <summary>  
        /// 获取  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public T Get<T>(string key)
        {
            return Redis.Get<T>(key);
        }
        /// <summary>  
        /// 删除  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public bool Remove(string key)
        {
            return Redis.Remove(key);
        }

        public bool Add<T>(string key, T t, int timeout)
        {
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(key, secondsTimeOut);
            }
            return Redis.Add<T>(key, t);
        }
        #endregion
        public void d()
        {

        }

        #region 链表操作
        /// <summary>  
        /// 根据IEnumerable数据添加链表  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <param name="values"></param>  
        /// <param name="timeout"></param>  
        public void AddList<T>(string listId, IEnumerable<T> values, int timeout = 0)
        {
            Redis.Expire(listId, 60);
            IRedisTypedClient<T> iredisClient = Redis.As<T>();
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(listId, secondsTimeOut);
            }
            var redisList = iredisClient.Lists[listId];
            redisList.AddRange(values);
            iredisClient.Save();
        }

        /// <summary>  
        /// 添加单个实体到链表中  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <param name="Item"></param>  
        /// <param name="timeout"></param>  
        public void AddEntityToList<T>(string listId, T Item, int timeout = 0)
        {
            IRedisTypedClient<T> iredisClient = Redis.As<T>();
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(listId, secondsTimeOut);
            }
            var redisList = iredisClient.Lists[listId];
            redisList.Add(Item);
            iredisClient.Save();
        }

        /// <summary>  
        /// 获取链表  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <returns></returns>  
        public IEnumerable<T> GetList<T>(string listId)
        {
            IRedisTypedClient<T> iredisClient = Redis.As<T>();
            return iredisClient.Lists[listId];
        }

        /// <summary>  
        /// 在链表中删除单个实体  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <param name="t"></param>  
        public void RemoveEntityFromList<T>(string listId, T t)
        {
            IRedisTypedClient<T> iredisClient = Redis.As<T>();
            var redisList = iredisClient.Lists[listId];
            redisList.RemoveValue(t);
            iredisClient.Save();
        }

        /// <summary>  
        /// 根据lambada表达式删除符合条件的实体  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="listId"></param>  
        /// <param name="func"></param>  
        //public void RemoveEntityFromList<T>(string listId, Func<T, bool> func)  
        //{  
        //    using (IRedisTypedClient<T> iredisClient = Redis.As<T>())  
        //    {  
        //        var redisList = iredisClient.Lists[listId];  
        //        T value = redisList.Where(func).FirstOrDefault();  
        //        redisList.RemoveValue(value);  
        //        iredisClient.Save();  
        //    }  
        //}  
        #endregion

        #region 释放资源
        public void Dispose()
        {
            if (Redis != null)
            {
                Redis.Dispose();
                Redis = null;
            }
        }
        #endregion
    }
}
