using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace demo_console
{
    class Program
    {
        static void Main(string[] args)
        {
            FindFilePath(@"F:\Program Files (x86)\Tencent", ".exe");

            Console.Read();
        }

        /// <summary>
        /// 寻找符合条件的文件路径
        /// </summary>
        /// <param name="path">查找根目录</param>
        /// <param name="keyword">查找关键字</param>
        static void FindFilePath(string path, string keyword)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            foreach (FileInfo file in d.GetFiles())
            {
                if (file.Name.ToLower().Contains(keyword))
                {
                    Console.WriteLine(file.FullName);
                }
            }
            foreach (DirectoryInfo item in d.GetDirectories())
            {
                FindFilePath(item.FullName, keyword);
            }
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
