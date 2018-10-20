using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class ConcreteCompany : Company
    {
        private List<Company> childrens = new List<Company>();
        public ConcreteCompany(string name) : base(name)
        {
        }

        public override void Add(Company c)
        {
            childrens.Add(c);
        }

        public override void Remove(Company c)
        {
            childrens.Remove(c);
        }

        public override void LineOfDuty()
        {
            foreach (var item in childrens)
            {
                item.LineOfDuty();
            }
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + name);
            foreach (var item in childrens)
            {
                item.Display(depth + 2);
            }
        }
    }
}
