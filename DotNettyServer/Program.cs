using DotNettyServerBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NettyServerHelper.init();
            Console.ReadKey();
        }
    }
}
