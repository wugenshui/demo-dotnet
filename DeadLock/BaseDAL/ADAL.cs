//***************************************************
//Author : chenbo
//Description : 本文件由CodeSmith工具自动生成，请勿修改
//***************************************************
using common;
using DeadLock.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DeadLock.DAL
{
    public partial class ADAL
    {
        private AInfo DataRowToEntity(DataRow row)
        {
            AInfo info = new AInfo();
            info.ID = Convert.ToInt32(row["ID"]);
            info.Value = Convert.ToInt32(row["Value"]);

            return info;
        }

        public bool IsExist(object id)
        {
            int count = 0;
            string sql = "SELECT count(*) FROM A WHERE ID=@id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("id",id),
            };
            object counter = SqlHelper.ExecuteScalar(sql, param);
            int.TryParse(counter.ToString(), out count);

            return count > 0;
        }

        public AInfo FindByID(object id)
        {
            AInfo result = null;
            string sql = @"SELECT ID,Value
                            FROM A
                            WHERE ID=@id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("id",id),
            };
            DataTable table = SqlHelper.ExecuteDataset(sql, param).Tables[0];
            if (table != null && table.Rows.Count > 0)
            {
                result = DataRowToEntity(table.Rows[0]);
            }

            return result;
        }

        public List<AInfo> Find(string filterStr = null, SqlParameter[] param = null, string orderFields = null)
        {
            List<AInfo> result = new List<AInfo>();
            string sql = @"SELECT ID,Value
                            FROM A";
            if (filterStr != null)
            {
                sql += " Where " + filterStr;
            }
            if (orderFields != null)
            {
                sql += " Order By " + orderFields;
            }
            DataTable table = SqlHelper.ExecuteDataset(sql, param).Tables[0];
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    result.Add(DataRowToEntity(row));
                }
            }

            return result;
        }

        public int Insert(AInfo model, SqlTransaction trans = null)
        {
            string sql = @"INSERT INTO A
                        (ID,Value)
                        VALUES (@ID,@Value)";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("ID",model.ID),
                new SqlParameter("Value",model.Value),
            };
            if (trans == null)
            {
                SqlHelper.ExecuteScalar(sql, param);
            }
            else
            {
                SqlHelper.ExecuteScalar(trans, sql, param);
            }
            return model.ID;
        }

        public int Update(AInfo model, SqlTransaction trans = null)
        {
            string sql = @"UPDATE A
                            SET Value=@Value
                            WHERE ID=@ID";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("ID",model.ID),
                new SqlParameter("Value",model.Value),
            };

            if (trans == null)
            {
                return SqlHelper.ExecuteNonQuery(sql, param);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(trans, sql, param);
            }
        }

        public int Delete(object id, SqlTransaction trans = null)
        {
            string sql = "DELETE FROM A WHERE ID=@id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("id",id),
            };

            if (trans == null)
            {
                return SqlHelper.ExecuteNonQuery(sql, param);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(trans, sql, param);
            }
        }
    }
}
