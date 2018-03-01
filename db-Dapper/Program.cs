using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_Dapper
{
    class OuUser
    {
        public int id { get; set; }
        public int ouid { get; set; }
        public int userid { get; set; }

        public Ou ou { get; set; }
    }

    class Ou
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string name { get; set; }
        public List<OuUser> users { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }

        static void baseSql()
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=OA;Integrated Security=True;MultipleActiveResultSets=True");
            string sql = "INSERT INTO AuthorityOuUser (ouId, userId) VALUES (@ouid, @userid)";
            OuUser model = new OuUser { ouid = 86, userid = 31 };
            //int result = connection.Execute(sql, model);    // 单行插入

            List<OuUser> models = new List<OuUser>();
            models.Add(model);
            models.Add(model);
            //int result2 = connection.Execute(sql, models);    // 多行插入

            int result4 = connection.Execute("DELETE FROM AuthorityOuUser WHERE id=@id", new { id = 12 });  // 删除

            int result3 = connection.Execute("UPDATE AuthorityOuUser SET ouId=10086 WHERE id=@id", new { id = 11 }); // 修改

            // 查询
            List<OuUser> data = connection.Query<OuUser>("SELECT id, ouId, userId FROM AuthorityOuUser").ToList();
            List<OuUser> data2 = connection.Query<OuUser>("SELECT id, ouId, userId FROM AuthorityOuUser WHERE ouId=@ouId", new { ouid = 86 }).ToList();
            List<OuUser> data3 = connection.Query<OuUser>("SELECT id, ouId, userId FROM AuthorityOuUser WHERE ouId in @ouId", new { ouid = new int[2] { 86, 1 } }).ToList();
            var data4 = connection.Query("SELECT * FROM AuthorityOu o LEFT JOIN AuthorityOuUser u ON u.ouId=o.id").ToList();
            foreach (var item in data4)
            {
                int id = item.id;
            }

            var data5 = connection.Query<OuUser, Ou, OuUser>("SELECT * FROM AuthorityOuUser u LEFT JOIN AuthorityOu o ON u.ouId=o.id",
                                    (OuUser, Ou) =>
                                    {
                                        OuUser.ou = Ou;
                                        return OuUser;
                                    }, splitOn: "userid").ToList();

            // 查询不出来
            var data6 = connection.Query<Ou, List<OuUser>, Ou>("SELECT * FROM AuthorityOu o LEFT JOIN AuthorityOuUser u ON u.ouId=o.id",
                                    (Ou, OuUser) =>
                                    {
                                        Ou.users = OuUser;
                                        return Ou;
                                    }, splitOn: "name").ToList();
        }

        static void transation()
        {
            string sql = "INSERT INTO AuthorityOuUser (ouId, userId) VALUES (@ouid, @userid)";
            using (IDbConnection dbConnection = new SqlConnection("Data Source=.;Initial Catalog=OA;Integrated Security=True;MultipleActiveResultSets=True"))
            {
                dbConnection.Open();
                IDbTransaction transaction = dbConnection.BeginTransaction();
                try
                {
                    dbConnection.Execute(sql, new OuUser { ouid = 1, userid = 1 }, transaction);
                    // throw new Exception("23");   // 引发异常，回滚事件
                    dbConnection.Execute(sql, new OuUser { ouid = 2, userid = 2 }, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}