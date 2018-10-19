using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class SqlserverUser : IUser
    {
        public void Insert()
        {
            Console.WriteLine("在Sql Server中插入一条User记录");
        }


        public void Search()
        {
            Console.WriteLine("在Sql Server中查询User记录");
        }
    }
}
