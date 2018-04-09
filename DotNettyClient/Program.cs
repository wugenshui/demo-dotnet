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
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            NettyClientHelper.init().Wait();

            for (;;)
            {
                string line = Console.ReadLine();
                NettyClientHelper.Send(line);
            }
        }
    }
}
