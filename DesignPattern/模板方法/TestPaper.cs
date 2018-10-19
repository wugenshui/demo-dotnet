using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class TestPaper
    {
        public void TestQuestion1()
        {
            Console.WriteLine("1+1=() a.1 b.2 c.3 d.4");
            Console.WriteLine("答案:" + Answer1());
        }

        protected virtual string Answer1()
        {
            return "";
        }

        public void TestQuestion2()
        {
            Console.WriteLine("1*1=() a.1 b.2 c.3 d.4");
            Console.WriteLine("答案:" + Answer2());
        }

        protected virtual string Answer2()
        {
            return "";
        }

        public void TestQuestion3()
        {
            Console.WriteLine("1/1=() a.1 b.2 c.3 d.4");
            Console.WriteLine("答案:" + Answer3());
        }

        protected virtual string Answer3()
        {
            return "";
        }
    }
}
