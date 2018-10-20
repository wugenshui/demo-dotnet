using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 总经理
    /// </summary>
    class GeneralManager : Manager
    {
        public GeneralManager(string name) : base(name)
        {
        }

        public override void RequestApplications(Request request)
        {
            if (request.Type == RequestType.请假)
            {
                Console.WriteLine("{0}:{1} 数量{2} 被批准", name, request.Content, request.Number);
            }
            else if (request.Type == RequestType.加薪 && request.Number <= 500)
            {
                Console.WriteLine("{0}:{1} 数量{2} 被批准", name, request.Content, request.Number);
            }
            else if (request.Type == RequestType.加薪 && request.Number > 500)
            {
                Console.WriteLine("{0}:{1} 数量{2} 再说吧", name, request.Content, request.Number);
            }
        }
    }
}