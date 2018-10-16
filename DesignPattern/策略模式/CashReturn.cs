using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 返利收费子类
    /// </summary>
    class CashReturn : CashSuper
    {
        private double moneyCondition = 0;
        private double moneyReturn = 0;

        public CashReturn(string moneyCondition, string moneyReturn)
        {
            this.moneyCondition = double.Parse(moneyCondition);
            this.moneyReturn = double.Parse(moneyReturn);
        }

        public override double accpetCash(double money)
        {
            return money - Math.Floor(money / moneyCondition) * moneyReturn;
        }
    }
}
