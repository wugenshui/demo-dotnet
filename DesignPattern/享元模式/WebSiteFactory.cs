using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 网站工厂
    /// </summary>
    class WebSiteFactory
    {
        private Hashtable flyweights = new Hashtable();

        // 获取网站分类
        public WebSite GetWebSiteCategory(string key)
        {
            if (!flyweights.ContainsKey(key))
            {
                flyweights.Add(key, new ConcreteWebSite(key));
            }
            return (WebSite)flyweights[key];
        }

        // 获取网站分类总数
        public int GetWebSiteCount()
        {
            return flyweights.Count;
        }
    }
}
