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
    public partial class BDAL
    {
        private BInfo DataRowToEntity(DataRow row)
        {
            BInfo info = new BInfo();
            info.ID = Convert.ToInt32(row["ID"]);
            info.Value = Convert.ToInt32(row["Value"]);

            return info;
        }

        public bool IsExist(object id)
        {
            int count = 0;
            string sql = "SELECT count(*) FROM B WHERE ID=@id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("id",id),
            };
            object counter = SqlHelper.ExecuteScalar(sql, param);
            int.TryParse(counter.ToString(), out count);

            return count > 0;
        }

        public BInfo FindByID(object id)
        {
            BInfo result = null;
            string sql = @"SELECT ID,Value
                            FROM B
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

        public List<BInfo> Find(string filterStr = null, SqlParameter[] param = null, string orderFields = null)
        {
            List<BInfo> result = new List<BInfo>();
            string sql = @"SELECT ID,Value
                            FROM B";
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

        public int Insert(BInfo model, SqlTransaction trans = null)
        {
            int count = 0;
            string sql = @"INSERT INTO B
                        (Value)
                        VALUES (@Value);
                        SELECT SCOPE_IDENTITY();";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("Value",model.Value),
            };
            object counter = null;
            if (trans == null)
            {
                counter = SqlHelper.ExecuteScalar(sql, param);
            }
            else
            {
                counter = SqlHelper.ExecuteScalar(trans, sql, param);
            }
            int.TryParse(counter.ToString(), out count);
            return count;
        }

        public int Update(BInfo model, SqlTransaction trans = null)
        {
            string sql = @"UPDATE B
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
            string sql = "DELETE FROM B WHERE ID=@id";
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
