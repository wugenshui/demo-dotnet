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

    //[SugarTable("AuthorityOu")]
    public class AuthorityOu
    {
        //[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public int pid { get; set; }
        public string name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //创建SqlSugarClient对象
            var db = SqlSugarHelper.GetInstance();

            //var sss = db.SqlQueryable<AuthorityOu>("select * from AuthorityOu").ToPageList(1, 2);

            var _AuthorityOu = db.GetSimpleClient<AuthorityOu>();
            var sss = _AuthorityOu.GetList();
            var firstOu = _AuthorityOu.GetById(1);
            var booll = _AuthorityOu.InsertReturnIdentity(new AuthorityOu()
            {
                name = "1",
                pid = 1,
                id = 1
            });
        }
    }
}
