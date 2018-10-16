using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Sneaker : Finery
    {
        public override void Show()
        {
            Console.Write("胶底鞋 ");
            base.Show();
        }
    }
}
