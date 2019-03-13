using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathCalculate
{
    /// <summary>
    /// 除法类
    /// </summary>
    public static class Division
    {
        /// <summary>
        /// 除法调用
        /// </summary>
        /// <param name="divisor">除数</param>
        /// <param name="dividend">被除数</param>
        /// <returns></returns>
        public static double Calculation(double divisor, double dividend)
        {
            if (dividend == 0)
            {
                throw new Exception("除数不能为0");
            }
            return divisor / dividend;
        }
    }
}
