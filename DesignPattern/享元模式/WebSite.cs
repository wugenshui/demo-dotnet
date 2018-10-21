using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 网站抽象类
    /// </summary>
    abstract class WebSite
    {
        public abstract void Use(User user);
    }
}
