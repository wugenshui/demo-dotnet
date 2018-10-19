using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Boss : INotifyer
    {
        // 同事列表
        private IList<Observer> observers = new List<Observer>();
        private string action;

        public string NotifyerState
        {
            get { return action; }
            set { action = value; }
        }

        public void Attach(Observer observer)
        {
            observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (Observer observer in observers)
                observer.Update();
        }
    }
}
