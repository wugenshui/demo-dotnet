using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyClient
{
    // 代码和服务端也相差不多，并且继承了同样的基类。
    public class NettyClientHandler : ChannelHandlerAdapter
    {
        readonly IByteBuffer initialMessage;

        public NettyClientHandler()
        {
            this.initialMessage = Unpooled.Buffer(CommonHelper.Size);
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this.initialMessage.WriteBytes(messageBytes);
        }

        //重写基类方法，当链接上服务器后，马上发送Hello World消息到服务端
        public override void ChannelActive(IChannelHandlerContext context)
        {
            context.WriteAndFlushAsync(this.initialMessage);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                Console.WriteLine("Received from server: " + byteBuffer.ToString(Encoding.UTF8));
            }
            //context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
