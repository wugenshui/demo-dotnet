using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Proxy : IGiveGift
    {
        private Pursuit pursuit;

        public Proxy(SchoolGirl gril)
        {
            pursuit = new Pursuit(gril);
        }

        public void GiveChocolate()
        {
            pursuit.GiveChocolate();
        }

        public void GiveDolls()
        {
            pursuit.GiveDolls();
        }

        public void GiveFlowers()
        {
            pursuit.GiveFlowers();
        }
    }
}
