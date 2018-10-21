using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    abstract class Human
    {
        public abstract void Accept(Action visitor);
    }
}
