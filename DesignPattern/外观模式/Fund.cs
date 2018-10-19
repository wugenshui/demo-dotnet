using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 基金类
    /// </summary>
    class Fund
    {
        Stock1 stock1;
        Stock2 stock2;
        Stock3 stock3;
        NationalDebt debt;
        Realty realty;

        public Fund()
        {
            stock1 = new Stock1();
            stock2 = new Stock2();
            stock3 = new Stock3();
            debt = new NationalDebt();
            realty = new Realty();
        }

        public void Buy()
        {
            stock1.Buy();
            stock2.Buy();
            stock3.Buy();
            debt.Buy();
            realty.Buy();
        }

        public void Sell()
        {
            stock1.Sell();
            stock2.Sell();
            stock3.Sell();
            debt.Sell();
            realty.Sell();
        }
    }
}
