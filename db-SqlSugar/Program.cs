using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_SqlSugar
{
    public class OuUser
    {
        public int id { get; set; }
        public int ouid { get; set; }
        public int userid { get; set; }
    }

    [SugarTable("AuthorityOu")]
    public class AuthorityOu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public int pid { get; set; }
        public string name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //创建SqlSugarClient对象
            var db = GetInstance();
            var sdb = new SimpleClient<AuthorityOu>(db);

            var sss = db.SqlQueryable<AuthorityOu>("select * from AuthorityOu").ToPageList(1, 2);

            var _AuthorityOu = db.GetSimpleClient<AuthorityOu>();
            var firstOu = _AuthorityOu.GetById(1);
            var booll = _AuthorityOu.Insert(new AuthorityOu()
             {
                name = "1",
                pid = 1,
                id = 1
            });
        }

        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = "server=.;uid=sa;pwd=sa;database=OA", DbType = DbType.SqlServer, IsAutoCloseConnection = true });
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            return db;
        }
    }
}
