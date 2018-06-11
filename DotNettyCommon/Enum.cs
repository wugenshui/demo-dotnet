using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyCommon
{
    public enum MessageType
    {
        /// <summary>
        /// 连接
        /// </summary>
        CONNECT,
        /// <summary>
        /// 获取在线列表
        /// </summary>
        LIST,
        /// <summary>
        /// 发送消息
        /// </summary>
        EMIT
    }
}
