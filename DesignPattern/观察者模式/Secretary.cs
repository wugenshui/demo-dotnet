using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 前台秘书类
    /// </summary>
    class Secretary : INotifyer
    {
        // 同事列表
        private IList<Observer> observers = new List<Observer>();
        private string action;

        // 前台状态
        public string NotifyerState
        {
            get { return action; }
            set { action = value; }
        }

        // 增加
        public void Attach(Observer observer)
        {
            observers.Add(observer);
        }

        // 减少
        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        // 通知
        public void Notify()
        {
            foreach (Observer observer in observers)
                observer.Update();
        }

    }
}
