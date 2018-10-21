using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class ObjectStructure
    {
        private IList<Human> humans = new List<Human>();

        public void Attach(Human human)
        {
            humans.Add(human);
        }

        public void Detach(Human human)
        {
            humans.Remove(human);
        }

        public void Display(Action visitor)
        {
            foreach (Human human in humans)
            {
                human.Accept(visitor);
            }
        }
    }
}
