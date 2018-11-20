using common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(ThreadFun);
                thread.Start();
            }
            Console.ReadKey();
        }

        #region 枚举操作

        public static void forEachEnum()
        {
            foreach (UserType item in Enum.GetValues(typeof(UserType)))
            {
                Console.WriteLine(item);                // Admin
                Console.WriteLine((int)item);           // 0
                Console.WriteLine(GetEnumDesc(item));   // 管理员
            }
        }

        public enum UserType
        {
            [Description("管理员")]
            Admin = 0,

            [Description("教师")]
            Teacher = 1,

            [Description("学生")]
            Student = 2,
        }

        /// <summary>
        /// 得到枚举的注释
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumDesc(Enum e)
        {
            FieldInfo EnumInfo = e.GetType().GetField(e.ToString());
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])EnumInfo.
                GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
            {
                return EnumAttributes[0].Description;
            }
            return e.ToString();
        }

        #endregion

        #region 多线程测试

        public static int num = 0;
        static void ThreadFun() // 来自委托：ThreadStart 
        {
            HttpPost();

            //for (int i = 0; i < 10000; i++)
            //{
            //    num++;
            //    var table = SqlHelper.ExecuteDataset("select * from GMS_Log").Tables[0];
            //    Console.WriteLine(num + ":查询出数据条数：" + table.Rows.Count);
            //    int count = 0;
            //    object counter = SqlHelper.ExecuteScalar(@"INSERT INTO GMS_Log (WorkOrderID, WorkOrderEquipment_ID, MessagePrefix, Message, LogType, CreatTime, WorkSmallStepID, NeedNotify, GMSSign) VALUES (14125, 0, '地线防护2组', '派工单自动同步!', 0, '2018-06-22 09:19:00.483', 1, 0, '');SELECT SCOPE_IDENTITY();");
            //    if (int.TryParse(counter.ToString(), out count))
            //    {
            //        Console.WriteLine("插入成功：" + count);
            //    }
            //    else
            //    {
            //        Console.WriteLine("插入失败！");
            //    }
            //    int delcount = SqlHelper.ExecuteNonQuery("DELETE FROM GMS_Log WHERE ID=" + count);
            //    if (delcount == 1)
            //    {
            //        Console.WriteLine("删除成功！");
            //    }
            //    else
            //    {
            //        Console.WriteLine("删除失败！");
            //    }
            //}

            //string path = "1.jpg";
            //Image img = Image.FromFile(path);
            //int width = img.Width;
            //int height = img.Height;
            //Console.WriteLine(width + ":" + height);
        }

        static void HttpPost()
        {
            string url = "http://localhost:58949/api/Workorder";
            string data = @"[
{'oUCode':'0012413401','remark':'','groupType':'007-002','groupName':'车梯1组','leader':'顾明','members':'陈国世,测量2',
'skylightDate':'2018-11-22','uniqueCode':'8a2997146336347401355d6d8954782','workticketCode': '18-11-22','material':[{'materialTypeId':'1238','count':1.0,'name':'弹簧垫片','model':'M12','unit':'个'},{'materialTypeId':'1601','count':1.0,'name':'承力索','model':'JTHM - 120','unit':'个'},{'materialTypeId':'0','count':4.0,'name':'地线','model':'','unit':'组'},{ 'materialTypeId':'4249','count':1.0,'name':'车梯（主）','model':'1号整体','unit':'台'}]},

{'oUCode':'40','remark':'','groupType':'004-000','groupName':'地线1组','leader':'吕伟华','members':'吴水生,吴惠垣','skylightDate':'2018-11-19T00:00:00Z','uniqueCode':'8a2997146336347401355d6d8954782','workticketCode': '18-19-01','material':[{'materialTypeId':'3339','count':1.0,'name':'测温片','model':'','unit':'片'},{'materialTypeId':'1600','count':1.0,'name':'承力索','model':'JTHM-95','unit':'个'},{'materialTypeId':'1422','count':2.0,'name':'螺母','model':'不锈钢六角M10','unit':'个'},{'materialTypeId':'0','count':1.0,'name':'验电器','model':'','unit':'把'},{'materialTypeId':'1207','count':1.0,'name':'对讲机','model':'','unit':'台'},{'materialTypeId':'881','count':1.0,'name':'钢丝钳','model':'200','unit':'把'},{'materialTypeId':'892','count':6.0,'name':'扭力扳手','model':'20-100N.m','unit':'把'}]},
{'oUCode':'40','remark':'','groupType':'007-006','groupName':'巡视1组','leader':'张晓红','members':'','skylightDate':'2018-11-19T00:00:00Z','uniqueCode':'8a2997146336347401355d6d8954782','workticketCode': '18-19-01','material':[{'materialTypeId':'881','count':1.0,'name':'钢丝钳','model':'200','unit':'把'},{'materialTypeId':'1601','count':1.0,'name':'承力索','model':'JTHM-120','unit':'个'},{'materialTypeId':'0','count':4.0,'name':'地线','model':'','unit':'组'}]},
{'oUCode':'40','remark':'','groupType':'007-005','groupName':'测量1组','leader':'张晓红','members':'','skylightDate':'2018-11-19T00:00:00Z','uniqueCode':'8a2997146336347401355d6d8954782','workticketCode': '18-19-01','material':[{'materialTypeId':'0','count':1.0,'name':'工具包','model':'双肩','unit':'个'},{'materialTypeId':'881','count':1.0,'name':'钢丝钳','model':'200','unit':'把'},{'materialTypeId':'1601','count':1.0,'name':'承力索','model':'JTHM-120','unit':'个'},{'materialTypeId':'0','count':4.0,'name':'地线','model':'','unit':'组'}]},
{'oUCode':'40','remark':'','groupType':'007-010','groupName':'待定1组','leader':'张晓红','members':'张三丰,小五','skylightDate':'2018-11-19T00:00:00Z','uniqueCode':'8a2997146336347401355d6d8954782','workticketCode': '18-19-01','material':[{'materialTypeId':'0','count':1.0,'name':'工具包','model':'双肩','unit':'个'},{'materialTypeId':'881','count':1.0,'name':'钢丝钳','model':'200','unit':'把'},{'materialTypeId':'1601','count':1.0,'name':'承力索','model':'JTHM-120','unit':'个'},{'materialTypeId':'0','count':4.0,'name':'地线','model':'','unit':'组'}]}]]
";
            //命名空间System.Net下的HttpWebRequest类
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //参照浏览器的请求报文 封装需要的参数 这里参照ie9
            //浏览器可接受的MIME类型
            request.Accept = "text/plain, */*; q=0.01";
            //浏览器类型，如果Servlet返回的内容与浏览器类型有关则该值非常有用
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
            request.ContentType = "application/json;charset=UTF-8";
            //请求方式
            request.Method = "POST";
            //是否保持长连接
            request.KeepAlive = false;
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            //表示请求消息正文的长度
            //request.ContentLength = data.Length;
            Stream postStream = request.GetRequestStream();
            byte[] postData = Encoding.UTF8.GetBytes(data);
            //将传输的数据，请求正文写入请求流
            postStream.Write(postData, 0, postData.Length);
            postStream.Dispose();
            //响应
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.ContentEncoding == "gzip")
            {//判断响应的信息是否为压缩信息 若为压缩信息解压后返回
                MemoryStream ms = new MemoryStream();
                System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                byte[] buffer = new byte[1024];
                int l = zip.Read(buffer, 0, buffer.Length);
                while (l > 0)
                {
                    ms.Write(buffer, 0, l);
                    l = zip.Read(buffer, 0, buffer.Length);
                }
                ms.Dispose();
                zip.Dispose();
                Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
            }
            else
            {
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                StringBuilder sb = new StringBuilder();
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    sb.Append(StrDate + "\r\n");
                }
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff:::") + sb.ToString());
            }
        }

        #endregion

        #region nlog记录日志
        static void nlog()
        {
            LogHelper.Debug("Debug调试信息");
            LogHelper.Info("Info一般信息");
            LogHelper.Error("Error异常");
            try
            {
                int a = 0;
                var b = 1 / a;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

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

        #region github网址是否存在

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

        #endregion

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
