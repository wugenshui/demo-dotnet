using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 管理者
    /// </summary>
    abstract class Manager
    {
        protected string name;
        // 管理者的上级
        protected Manager superior;

        public Manager(string name)
        {
            this.name = name;
        }

        public void SetSuperior(Manager superior)
        {
            this.superior = superior;
        }

        /// <summary>
        /// 申请请求
        /// </summary>
        /// <param name="request"></param>
        public abstract void RequestApplications(Request request);
    }
}
