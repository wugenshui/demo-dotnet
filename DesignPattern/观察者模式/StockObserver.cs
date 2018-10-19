using System;

namespace DesignPattern
{
    /// <summary>
    /// 看股票同事类
    /// </summary>
    class StockObserver : Observer
    {
        public StockObserver(string name, INotifyer notifyer) : base(name, notifyer)
        {
        }

        public override void Update()
        {
            Console.WriteLine("{0} {1}关闭股票行情，继续工作！", notifyer.NotifyerState, name);
        }
    }
}