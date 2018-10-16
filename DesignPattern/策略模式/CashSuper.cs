using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 现金收费抽象类
    /// </summary>
    abstract class CashSuper
    {
        public abstract double accpetCash(double money);
    }
}
