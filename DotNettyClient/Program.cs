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
using System.Threading;
using System.Threading.Tasks;

namespace DotNettyClient
{
    public class Program
    {
        public static List<string> users = new List<string>();
        static void Main(string[] args)
        {
            NettyClientHelper.init().Wait();

            for (;;)
            {
                string line = Console.ReadLine();
                Message msg = new Message()
                {
                    type = "list",
                };
                NettyClientHelper.Send(JsonHelper.JsonSerialize(msg));
                Thread.Sleep(200);
                foreach (string user in users)
                {
                    Message msgs = new Message()
                    {
                        type = "emit",
                        from = Environment.UserName,
                        to = user,
                        msg = line,
                    };
                    NettyClientHelper.Send(JsonHelper.JsonSerialize(msgs));
                }
            }
        }
    }
}
