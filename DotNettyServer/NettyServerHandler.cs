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
    public class NettyServerHandler : ChannelHandlerAdapter //管道处理基类，较常用
    {
        private static ConcurrentDictionary<string, IChannelHandlerContext> channels = new ConcurrentDictionary<string, IChannelHandlerContext>();

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                Console.WriteLine("接收到客户端" + context.Channel.RemoteAddress.ToString() + "消息:" + buffer.ToString(Encoding.UTF8));
            }
            var chanels = channels.Last();
            Console.WriteLine("发送至客户端" + chanels.Value.Channel.RemoteAddress.ToString() + "消息:" + buffer.ToString(Encoding.UTF8));
            chanels.Value.WriteAndFlushAsync(message);    // 回写输出流
        }

        //public override void ChannelReadComplete(IChannelHandlerContext context)
        //{
        //    context.Flush();
        //}

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
