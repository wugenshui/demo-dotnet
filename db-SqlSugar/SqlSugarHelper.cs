using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_SqlSugar
{
    public class SqlSugarHelper
    {
        private static SqlSugarClient client;
        public static SqlSugarClient GetInstance()
        {
            client = new SqlSugarClient(new ConnectionConfig() { ConnectionString = "server=.;uid=sa;pwd=sa;database=OA", DbType = DbType.SqlServer, IsAutoCloseConnection = true });
            // 切面处理
            //client.Aop.OnLogExecuting = (sql, pars) =>
            //{
            //    Console.WriteLine(sql + "\r\n" + client.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            //    Console.WriteLine();
            //};
            return client;
        }
    }
}
