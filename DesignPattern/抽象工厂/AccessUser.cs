using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class AccessUser : IUser
    {
        public void Insert()
        {
            Console.WriteLine("在Access中插入一条User记录");
        }


        public void Search()
        {
            Console.WriteLine("在Access中查询User记录");
        }
    }
}
