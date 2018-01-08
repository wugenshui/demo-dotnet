using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_console
{
    class Program
    {
        static void Main(string[] args)
        {
            int q = gcd(27, 18);


            Console.WriteLine(q);

            Console.Read();
        }

        /// <summary>
        /// 欧几里得算法(找到2个数的最大公约数)
        /// </summary>
        /// <param name="num1">值1</param>
        /// <param name="num2">值2</param>
        /// <returns>最大公约数</returns>
        static int gcd(int num1, int num2)
        {
            if (num2 == 0)
            {
                return num1;
            }
            int r = num1 % num2;
            return gcd(num2, r);
        }
    }
}
