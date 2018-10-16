using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 打折收费子类
    /// </summary>
    class CashRebate : CashSuper
    {
        private double moneyRebate = 1;

        public CashRebate(string moneyRebate)
        {
            this.moneyRebate = double.Parse(moneyRebate);
        }

        public override double accpetCash(double money)
        {
            return money * moneyRebate;
        }
    }
}
