using System;

namespace DesignPattern
{
    class Man : Human
    {
        public override void Accept(Action visitor)
        {
            visitor.GetManConclusion(this);
        }
    }
}