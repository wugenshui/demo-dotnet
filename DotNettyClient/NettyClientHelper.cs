using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
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
    public class NettyClientHelper
    {
        private static IChannel clientChannel = default(IChannel);

        /// <summary>
        /// 初始化指令系统
        /// </summary>
        /// <returns></returns>
        public static async Task init()
        {
            try
            {
                // 工作线程组，默认为内核数*2的线程数
                var group = new MultithreadEventLoopGroup();

                X509Certificate2 cert = null;
                string targetHost = null;
                if (CommonHelper.IsSsl) //如果使用加密通道
                {
                    cert = new X509Certificate2(Path.Combine("", "dotnetty.com.pfx"), "password");
                    targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
                }

                Bootstrap bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)// 设置工作线程组
                    .Channel<TcpSocketChannel>()// 设置通道模式为TcpSocket
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoKeepalive, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        //工作线程连接器 是设置了一个管道，客户端主线程所有接收到的信息都会通过这个管道一层层往下传输
                        //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                        IChannelPipeline pipeline = channel.Pipeline;
                        if (cert != null)
                        {
                            pipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                        }
                        //编码解码配置，下面两个配置必须和服务器端保持一致
                        // 读超时、写超时、读写超时
                        pipeline.AddLast("timeout", new IdleStateHandler(10, 10, 20));
                        //出栈消息，通过这个handler 在消息顶部加上消息的长度
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(4));
                        //入栈消息通过该Handler,解析消息的包长信息，并将正确的消息体发送给下一个处理Handler
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                        pipeline.AddLast("decoder", new StringDecoder(Encoding.UTF8));
                        pipeline.AddLast("encoder", new StringEncoder(Encoding.UTF8));
                        //业务handler ，这里是实际处理业务的Handler
                        pipeline.AddLast(new NettyClientHandler());
                    }));

                //连接到指定服务器地址
                clientChannel = await bootstrap.ConnectAsync(CommonHelper.Host, CommonHelper.Port);
            }
            catch (Exception ex)
            {
                clientChannel = null;
                Console.WriteLine("指令系统初始化异常:" + ex);
            }
        }

        /// <summary>
        /// 指令系统发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static async void Send(string msg)
        {
            try
            {
                if (clientChannel == null)
                {
                    await init();
                }
                //发送给服务器端
                await clientChannel.WriteAndFlushAsync(msg);
            }
            catch (Exception ex)
            {
                clientChannel = null;
                Console.WriteLine("指令发送异常:" + ex);
            }
        }
    }
}
