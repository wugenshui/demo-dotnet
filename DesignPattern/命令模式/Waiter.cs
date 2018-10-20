using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Waiter
    {
        private IList<Command> orders = new List<Command>();

        public void SetOrder(Command command)
        {
            orders.Add(command);
            Console.WriteLine("增加订单：" + command.ToString() + " 时间：" + DateTime.Now);
        }

        public void CancerOrder(Command command)
        {
            orders.Remove(command);
            Console.WriteLine("取消订单：" + command.ToString() + " 时间：" + DateTime.Now);
        }

        public void Notify()
        {
            foreach (Command order in orders)
            {
                order.ExeuteCommand();
            }
        }
    }
}
