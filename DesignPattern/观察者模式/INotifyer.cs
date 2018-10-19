using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 通知者接口
    /// </summary>
    interface INotifyer
    {
        void Attach(Observer observer);
        void Detach(Observer observer);
        void Notify();
        string NotifyerState
        {
            get;
            set;
        }
    }
}
