using common;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace demo_console
{
    class Program
    {
        static void Main(string[] args)
        {
            nlog();
            //for (int i = 0; i < 10000; i++)
            //{
            //    Thread thread = new Thread(ThreadFun);
            //    thread.Start();
            //}
            //Console.ReadKey();
        }

        public static int num = 0;
        static void ThreadFun() // 来自委托：ThreadStart 
        {
            for (int i = 0; i < 10000; i++)
            {
                num++;
                var table = SqlHelper.ExecuteDataset("select * from GMS_Log").Tables[0];
                Console.WriteLine(num + ":查询出数据条数：" + table.Rows.Count);
                int count = 0;
                object counter = SqlHelper.ExecuteScalar(@"INSERT INTO GMS_Log (WorkOrderID, WorkOrderEquipment_ID, MessagePrefix, Message, LogType, CreatTime, WorkSmallStepID, NeedNotify, GMSSign) VALUES (14125, 0, '地线防护2组', '派工单自动同步!', 0, '2018-06-22 09:19:00.483', 1, 0, '');SELECT SCOPE_IDENTITY();");
                if (int.TryParse(counter.ToString(), out count))
                {
                    Console.WriteLine("插入成功：" + count);
                }
                else
                {
                    Console.WriteLine("插入失败！");
                }
                int delcount = SqlHelper.ExecuteNonQuery("DELETE FROM GMS_Log WHERE ID=" + count);
                if (delcount == 1)
                {
                    Console.WriteLine("删除成功！");
                }
                else
                {
                    Console.WriteLine("删除失败！");
                }
            }
        }

        // nlog记录日志
        static void nlog()
        {
            ILogger log = LogManager.GetLogger("NLog");
            log.Debug("Debug调试信息");
            log.Info("Info一般信息");
            log.Error("Error异常");
            try
            {
                int a = 0;
                var b = 1 / a;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public class Person
        {
            public string name { get; set; }
        }
        // 线程安全的操作类
        static void safeThreadClass()
        {
            ConcurrentDictionary<int, Person> bag = new ConcurrentDictionary<int, Person>();
            Person p1 = new Person { name = "1" };
            Person p2 = new Person { name = "2" };
            Person p3 = new Person { name = "3" };
            bag.TryAdd(1, p1);
            bag.TryAdd(2, p2);
            bag.TryAdd(3, p3);
            bag.TryUpdate(1, p2, p1);
            bool state = bag.TryRemove(2, out p2);
            foreach (int key in bag.Keys)
            {
                Person p = null;
                bag.TryGetValue(key, out p);
            }
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
