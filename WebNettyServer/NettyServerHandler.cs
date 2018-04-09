using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebNettyServer
{
    public class NettyServerHandler : ChannelHandlerAdapter //管道处理基类，较常用
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                LogHelper.Info("接收到客户端消息:" + buffer.ToString(Encoding.UTF8));
            }
            context.WriteAndFlushAsync(message);    // 回写输出流
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            LogHelper.Info("建立连接：" + context.Channel.LocalAddress.ToString());
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (exception is SocketException && ((SocketException)exception).ErrorCode == 10054)
            {
                LogHelper.Info("套接字异常:" + exception.Message);
            }
            else
            {
                LogHelper.Error("服务器异常:" + exception);
                NettyServerHelper.init().Wait();
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            LogHelper.Info("断开连接：" + context.Channel.LocalAddress.ToString());
        }
    }
}