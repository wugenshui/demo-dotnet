using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class CashContext
    {
        private CashSuper cashsuper;

        public CashContext(CashSuper cashsuper)
        {
            this.cashsuper = cashsuper;
        }

        public double GetResult(double money)
        {
            return cashsuper.accpetCash(money);
        }
    }
}
