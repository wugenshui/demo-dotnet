using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    abstract class Action
    {
        /// <summary>
        /// 得到男人结论或反映
        /// </summary>
        /// <param name="man"></param>
        public abstract void GetManConclusion(Man man);

        /// <summary>
        /// 得到女人结论或反映
        /// </summary>
        /// <param name="man"></param>
        public abstract void GetWomanConclusion(Woman man);
    }
}
