using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyServer
{
    public class Message
    {
        public string type { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public bool state { get; set; }
        public string msg { get; set; }
    }

    public class NettyServerHandler : ChannelHandlerAdapter //管道处理基类，较常用
    {
        private static ConcurrentDictionary<string, IChannelHandlerContext> channels = new ConcurrentDictionary<string, IChannelHandlerContext>();

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Message response = new Message();
            if (message != null)
            {
                Console.WriteLine("接收到客户端" + context.Channel.RemoteAddress.ToString() + "消息:" + message);
                Message request = JsonHelper.JsonDeserialize<Message>(message.ToString());
                response.type = request.type;
                response.from = request.to;
                response.to = request.from;
                IChannelHandlerContext channel = null;

                switch (request.type)
                {
                    case "connect":
                        channel = context;
                        response.msg = "注册成功！";
                        response.state = true;
                        break;
                    case "list":
                        channel = context;
                        response.msg = JsonHelper.JsonSerialize(channels.Keys);
                        response.state = true;
                        break;
                    case "emit":
                        response.from = request.from;
                        response.to = request.to;
                        if (channels.TryGetValue(request.to, out channel))
                        {
                            response.msg = request.msg;
                            response.state = true;
                        }
                        else
                        {
                            channel = context;
                            response.msg = "未查询到指定用户！";
                            response.state = false;
                        }
                        break;
                    default:
                        break;
                }
                channel.WriteAndFlushAsync(JsonHelper.JsonSerialize(response));    // 回写输出流
            }
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine("建立连接：" + context.Channel.RemoteAddress.ToString());
            channels.TryAdd(context.Channel.RemoteAddress.ToString(), context);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (exception is SocketException && ((SocketException)exception).ErrorCode == 10054)
            {
                Console.WriteLine(context.Channel.RemoteAddress.ToString() + "套接字异常:" + exception.Message);
            }
            else
            {
                Console.WriteLine(context.Channel.RemoteAddress.ToString() + "服务器异常:" + exception);
                NettyServerHelper.init().Wait();
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("断开连接：" + context.Channel.RemoteAddress.ToString());
            channels.TryRemove(context.Channel.RemoteAddress.ToString(), out context);
        }
    }
}
