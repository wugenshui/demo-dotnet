namespace DesignPattern
{
    abstract class Observer
    {
        protected string name;
        protected INotifyer notifyer;

        public Observer(string name, INotifyer notifyer)
        {
            this.name = name;
            this.notifyer = notifyer;
        }

        public abstract void Update();
    }
}