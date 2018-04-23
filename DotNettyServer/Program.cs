using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
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
            NettyServerHelper.init().Wait();
            Console.WriteLine("服务启动成功！");
            Console.ReadLine();
        }
    }
}
