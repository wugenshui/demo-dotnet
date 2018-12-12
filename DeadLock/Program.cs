using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadLock
{
    /// <summary>
    /// 测试和解决死锁
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Lock01();
            Console.ReadLine();
        }

        static void Lock01()
        {
            string sql = @"begin tran
                           begin try
                             insert into A values (1)
                             insert into B values (null)
                           end try
                           begin catch
                              if @@trancount > 0
                                 rollback tran
                           end catch

                           if @@trancount > 0
                              commit tran
                           go";
            var value = SqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine(value);
        }
    }
}
