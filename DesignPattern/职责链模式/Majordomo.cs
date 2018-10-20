using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 总监
    /// </summary>
    class Majordomo : Manager
    {
        public Majordomo(string name) : base(name)
        {
        }

        public override void RequestApplications(Request request)
        {
            if (request.Type == RequestType.请假 && request.Number <= 5)
            {
                Console.WriteLine("{0}:{1} 数量{2} 被批准", name, request.Content, request.Number);
            }
            else
            {
                if (superior != null)
                {
                    superior.RequestApplications(request);
                }
            }
        }
    }
}