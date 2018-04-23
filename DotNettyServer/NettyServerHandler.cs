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

    public class Model
    {
        public string username { get; set; }
        public IChannelHandlerContext channel { get; set; }
    }

    public class NettyServerHandler : ChannelHandlerAdapter //管道处理基类，较常用
    {
        private static ConcurrentDictionary<string, Model> _channels = new ConcurrentDictionary<string, Model>();

        public override void ChannelActive(IChannelHandlerContext context)
        {
            if (context != null)
            {
                Console.WriteLine("建立连接：" + context.Channel.RemoteAddress.ToString());
                _channels.TryAdd(context.Channel.RemoteAddress.ToString(), new Model() { channel = context });
            }
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Message response = new Message();
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                Console.WriteLine("接收到客户端" + context.Channel.RemoteAddress.ToString() + "消息:" + buffer.ToString(Encoding.UTF8));
                Message request = JsonHelper.JsonDeserialize<Message>(buffer.ToString(Encoding.UTF8));
                if (request != null)
                {
                    response.type = request.type;
                    response.from = request.to;
                    response.to = request.from;
                    Model oldModel = null;

                    IChannelHandlerContext channel = null;

                    switch (request.type)
                    {
                        case "connect":
                            channel = context;
                            response.msg = "注册成功！";
                            response.state = true;
                            _channels.TryGetValue(context.Channel.RemoteAddress.ToString(), out oldModel);
                            Model newModel = new Model();
                            newModel.channel = oldModel.channel;
                            newModel.username = request.from;
                            _channels.TryUpdate(context.Channel.RemoteAddress.ToString(), newModel, oldModel);
                            break;
                        case "list":
                            channel = context;
                            List<string> users = new List<string>();
                            foreach (string key in _channels.Keys)
                            {
                                if (_channels.TryGetValue(key, out oldModel) && !string.IsNullOrWhiteSpace(oldModel.username))
                                {
                                    users.Add(oldModel.username);
                                }
                            }
                            response.msg = JsonHelper.JsonSerialize(users);
                            response.state = true;
                            break;
                        case "emit":
                            response.from = request.from;
                            response.to = request.to;
                            if (!string.IsNullOrWhiteSpace(request.to))
                            {
                                if (request.to == "SYS")
                                {

                                }
                                else
                                {
                                    bool hasUser = false;
                                    foreach (string key in _channels.Keys)
                                    {
                                        if (_channels.TryGetValue(key, out oldModel) && oldModel.username == request.to)
                                        {
                                            if (_channels.TryGetValue(key, out oldModel))
                                            {
                                                channel = oldModel.channel;
                                                response.msg = request.msg;
                                                response.state = true;
                                            }
                                            hasUser = true;
                                            break;
                                        }
                                    }
                                    if (!hasUser)
                                    {
                                        channel = context;
                                        response.msg = "未查询到指定用户！";
                                        response.state = false;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    byte[] messageBytes = Encoding.UTF8.GetBytes(JsonHelper.JsonSerialize(response));
                    //缓存区
                    IByteBuffer initialMessage = Unpooled.Buffer(1024);
                    initialMessage.WriteBytes(messageBytes);
                    channel.WriteAndFlushAsync(initialMessage);    // 回写输出流
                }
            }
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
            Model model = new Model();
            Console.WriteLine("断开连接：" + context.Channel.RemoteAddress.ToString());
            _channels.TryRemove(context.Channel.RemoteAddress.ToString(), out model);
        }
    }
}
