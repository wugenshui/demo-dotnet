using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class TestPaperA : TestPaper
    {
        protected override string Answer1()
        {
            return "a";
        }
        protected override string Answer2()
        {
            return "a";
        }
        protected override string Answer3()
        {
            return "a";
        }
    }
}
