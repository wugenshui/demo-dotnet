using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_ef
{
    class Program
    {
        static void Main(string[] args)
        {
            Entities entity = new Entities();
            var ws = entity.GMS_WorkOrder.ToList();
        }
    }
}
