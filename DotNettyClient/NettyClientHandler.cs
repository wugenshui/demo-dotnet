using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNettyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyClient
{
    // 代码和服务端也相差不多，并且继承了同样的基类。
    public class NettyClientHandler : ChannelHandlerAdapter
    {
        //重写基类方法，当链接上服务器后，马上发送Hello World消息到服务端
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Message msg = new Message()
            {
                type = MessageType.CONNECT.ToString(),
                from = Environment.UserName,
                to = CommonHelper.SYS
            };
            context.WriteAndFlushAsync(JsonHelper.JsonSerialize(msg));
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message != null)
            {
                Console.WriteLine(message);
                Message msg = JsonHelper.JsonDeserialize<Message>(message.ToString());
                if (msg != null)
                {
                    if (msg.type == MessageType.LIST.ToString())
                    {
                        Program.users = JsonHelper.JsonDeserialize<List<string>>(msg.msg);
                    }
                    else if (msg.type == MessageType.CONNECT.ToString())
                    {
                        Console.WriteLine(msg.msg);  // 打印出服务器消息
                    }
                    else if (msg.type == MessageType.EMIT.ToString())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("用户" + msg.from + "说:" + msg.msg);  // 打印出服务器消息
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
