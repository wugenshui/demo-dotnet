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
    class ou_user
    {
        public int id { get; set; }
        public int ouid { get; set; }
        public int userid { get; set; }
    }

    class ou
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=OA;Integrated Security=True;MultipleActiveResultSets=True");
            string sql = "INSERT INTO AuthorityOuUser (ouId, userId) VALUES (@ouid, @userid)";
            ou_user model = new ou_user { ouid = 86, userid = 31 };
            //int result = connection.Execute(sql, model);    // 单行插入

            List<ou_user> models = new List<ou_user>();
            models.Add(model);
            models.Add(model);
            //int result2 = connection.Execute(sql, models);    // 多行插入

            int result4 = connection.Execute("DELETE FROM AuthorityOuUser WHERE id=@id", new { id = 12 });  // 删除

            int result3 = connection.Execute("UPDATE AuthorityOuUser SET ouId=10086 WHERE id=@id", new { id = 11 }); // 修改

            // 查询
            List<ou_user> data = connection.Query<ou_user>("SELECT id, ouId, userId FROM AuthorityOuUser").ToList();
            List<ou_user> data2 = connection.Query<ou_user>("SELECT id, ouId, userId FROM AuthorityOuUser WHERE ouId=@ouId", new { ouid = 86 }).ToList();
            List<ou_user> data3 = connection.Query<ou_user>("SELECT id, ouId, userId FROM AuthorityOuUser WHERE ouId in @ouId", new { ouid = new int[2] { 86, 1 } }).ToList();
            var data4 = connection.Query("SELECT * FROM AuthorityOu o LEFT JOIN AuthorityOuUser u ON u.ouId=o.id").ToList();
            foreach (var item in data4)
            {
                int id = item.id;
            }
        }
    }
}
