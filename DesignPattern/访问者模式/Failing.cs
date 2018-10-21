using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Failing : Action
    {
        public override void GetManConclusion(Man man)
        {
            Console.WriteLine("{0} {1}时,闷头喝酒，谁也不用劝。", man.GetType().Name, this.GetType().Name);
        }

        public override void GetWomanConclusion(Woman woman)
        {
            Console.WriteLine("{0} {1}时,眼泪汪汪，谁也劝不了。", woman.GetType().Name, this.GetType().Name);
        }
    }
}
