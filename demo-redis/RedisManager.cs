using ServiceStack.Redis;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_redis
{
    public class RedisManager
{
    /// <summary>  
    /// redis配置文件信息  
    /// </summary>  
    private static string RedisPath = ConfigurationManager.AppSettings["redisPath"];
    private static PooledRedisClientManager _prcm;

    public RedisManager()
    {
        CreateManager();
    }

    /// <summary>  
    /// 创建链接池管理对象  
    /// </summary>  
    private static void CreateManager()
    {
        _prcm = CreateManager(new string[] { RedisPath }, new string[] { RedisPath });
    }


    private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
    {
        //WriteServerList：可写的Redis链接地址。  
        //ReadServerList：可读的Redis链接地址。  
        //MaxWritePoolSize：最大写链接数。  
        //MaxReadPoolSize：最大读链接数。  
        //AutoStart：自动重启。  
        //LocalCacheTime：本地缓存到期时间，单位:秒。  
        //RecordeLog：是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项。  
        //RedisConfigInfo类是记录redis连接信息，此信息和配置文件中的RedisConfig相呼应  

        // 支持读写分离，均衡负载   
        return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
        {
            MaxWritePoolSize = 5, // “写”链接池链接数   
            MaxReadPoolSize = 5, // “读”链接池链接数   
            AutoStart = true,
        });
    }

    private static IEnumerable<string> SplitString(string strSource, string split)
    {
        return strSource.Split(split.ToArray());
    }

    /// <summary>  
    /// 客户端缓存操作对象  
    /// </summary>  
    public static IRedisClient GetClient()
    {
        if (_prcm == null)
        {
            CreateManager();
        }
        return _prcm.GetClient();
    }




    /// <summary>  
    /// RedisOperatorBase类，是redis操作的基类，继承自IDisposable接口，主要用于释放内存  
    /// </summary>  
    public abstract class RedisOperatorBase : IDisposable
    {
        protected IRedisClient Redis { get; private set; }
        private bool _disposed = false;
        protected RedisOperatorBase()
        {
            Redis = RedisManager.GetClient();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Redis.Dispose();
                    Redis = null;
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>  
        /// 保存数据DB文件到硬盘  
        /// </summary>  
        public void Save()
        {
            Redis.Save();
        }
        /// <summary>  
        /// 异步保存数据DB文件到硬盘  
        /// </summary>  
        public void SaveAsync()
        {
            Redis.SaveAsync();
        }
    }

    /// <summary>  
    /// HashOperator类，是操作哈希表类。继承自RedisOperatorBase类  
    /// </summary>  
    public class HashOperator : RedisOperatorBase
    {
        public HashOperator() : base() { }
        /// <summary>  
        /// 判断某个数据是否已经被缓存  
        /// </summary>  
        public bool Exist<T>(string hashId, string key)
        {
            return Redis.HashContainsEntry(hashId, key);
        }
        /// <summary>  
        /// 存储数据到hash表  
        /// </summary>  
        public bool Set<T>(string hashId, string key, T t)
        {
            var value = JsonSerializer.SerializeToString<T>(t);
            return Redis.SetEntryInHash(hashId, key, value);
        }
        /// <summary>  
        /// 移除hash中的某值  
        /// </summary>  
        public bool Remove(string hashId, string key)
        {
            return Redis.RemoveEntryFromHash(hashId, key);
        }
        /// <summary>  
        /// 移除整个hash  
        /// </summary>  
        public bool Remove(string key)
        {
            return Redis.Remove(key);
        }
        /// <summary>  
        /// 从hash表获取数据  
        /// </summary>  
        public T Get<T>(string hashId, string key)
        {
            string value = Redis.GetValueFromHash(hashId, key);
            return JsonSerializer.DeserializeFromString<T>(value);
        }
        /// <summary>  
        /// 获取整个hash的数据  
        /// </summary>  
        public List<T> GetAll<T>(string hashId)
        {
            var result = new List<T>();
            var list = Redis.GetHashValues(hashId);
            if (list != null && list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonSerializer.DeserializeFromString<T>(x);
                    result.Add(value);
                });
            }
            return result;
        }
        /// <summary>  
        /// 设置缓存过期  
        /// </summary>  
        public void SetExpire(string key, DateTime datetime)
        {
            Redis.ExpireEntryAt(key, datetime);
        }
    }

    [Serializable]
    public class UserInfo
    {
        public int Id;
        public string UserName;
        public int Age;
    }
}
}
