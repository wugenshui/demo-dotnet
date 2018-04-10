using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyService
{
    public class NettyServerHandler : ChannelHandlerAdapter //管道处理基类，较常用
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                LogHelper.Info(context.Channel.RemoteAddress.ToString() + Environment.NewLine + buffer.ToString(Encoding.UTF8));
            }
            context.WriteAndFlushAsync(message);    // 回写输出流
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            LogHelper.Info(context.Channel.RemoteAddress.ToString() + Environment.NewLine + "建立连接");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (exception is SocketException && ((SocketException)exception).ErrorCode == 10054)
            {
                LogHelper.Info(context.Channel.RemoteAddress.ToString() + Environment.NewLine + "套接字异常:" + exception.Message);
            }
            else
            {
                LogHelper.Error(context.Channel.RemoteAddress.ToString() + Environment.NewLine + "服务器异常:" + exception);
                NettyServerHelper.init();
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            LogHelper.Info(context.Channel.RemoteAddress.ToString() + Environment.NewLine + "断开连接");
        }
    }
}