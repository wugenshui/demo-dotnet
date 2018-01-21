using common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace demo_console
{
    class Program
    {
        public class user
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        static void Main(string[] args)
        {
            user u = new user()
            {
                id = 1,
                name = "陈波"
            };

            var s = JsonHelper.JsonSerialize(u);
            var uu = JsonHelper.JsonDeserialize<user>(s);

            //Console.ReadKey();
        }

        static void GetUrl()
        {
            string[] code = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string baseUrl = "https://github.com/";
            for (int i = 0; i < code.Length; i++)
            {
                for (int j = 0; j < code.Length; j++)
                {
                    for (int k = 0; k < code.Length; k++)
                    {
                        IsExist(baseUrl + code[i] + code[j] + code[k]);
                    }
                }
            }

            Console.WriteLine("输出完毕！");
        }

        static void IsExist(string url)
        {
            try
            {
                var response = HttpHelper.GetResponse(url); // 最好能捕获异常302的HttpException,然后再处理一下。在Data中取键值 Location  
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine(url);
                }
                Console.WriteLine(url.Replace("https://github.com/", ""));
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                if (rsp == null || rsp.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine(url);
                }
            }
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
