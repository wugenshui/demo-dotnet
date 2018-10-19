using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class NBAObserver : Observer
    {
        public NBAObserver(string name, INotifyer notifyer) : base(name, notifyer)
        {
        }

        public override void Update()
        {
            Console.WriteLine("{0} {1}关闭NBA直播，继续工作！", notifyer.NotifyerState, name);
        }
    }
}
