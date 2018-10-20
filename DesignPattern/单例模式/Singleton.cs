using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 懒汉式单例类 第一次被引用时才会实例化
    /// </summary>
    sealed class Singleton
    {
        // private static Singleton instance = new Singleton(); // 饿汉式  在加载时就将自己实例化
        private static Singleton instance;
        private static readonly object syncRoot = new object();
        private Singleton() { }

        public static Singleton GetInstance()
        {
            if (instance == null) // 双重锁定
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                }
            }

            return instance;
        }
    }
}
