using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyCommon
{
    public class Message
    {
        public string type { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public bool state { get; set; }
        public string msg { get; set; }
    }
}
