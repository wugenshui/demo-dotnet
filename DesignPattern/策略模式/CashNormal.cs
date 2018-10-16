using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 正常收费子类
    /// </summary>
    class CashNormal : CashSuper
    {
        public override double accpetCash(double money)
        {
            return money;
        }
    }
}
