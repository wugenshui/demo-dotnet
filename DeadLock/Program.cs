using common;
using DeadLock.DAL;
using DeadLock.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DeadLock
{
    /// <summary>
    /// 测试和解决死锁
    /// </summary>
    class Program
    {
        private static ADAL _ADAL = new ADAL();
        private static BDAL _BDAL = new BDAL();

        static void Main(string[] args)
        {
            Lock02();
            Console.ReadLine();
        }

        static void Lock01()
        {
            string sql = @"begin tran
                           begin try
                             insert into A values (1,1)
                             insert into B values (1,null)
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

        static void Lock02()
        {
            using (SqlTransaction trans = SqlHelper.BeginTransaction())
            {
                try
                {
                    _ADAL.Insert(new AInfo() { ID = 1, Value = 10086 }, trans);
                    _BDAL.Insert(new BInfo() { ID = 1, Value = 1111111111 }, trans);
                    trans.Commit();
                    Console.WriteLine("事务2执行成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    trans.Rollback();
                }
            }
        }
    }
}
