using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace EFAddNote
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlHelper.connStr = "data source=.; initial catalog=CRM; persist security info = True; user id = sa; password = sa;";

            string dirpath = Environment.CurrentDirectory;
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name.Contains(".edmx"))
                {
                    Console.WriteLine(file.Name);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file.FullName);
                    XmlElement root = doc.DocumentElement;
                    XmlElement runtime = root.GetElementsByTagName("edmx:Runtime")[0] as XmlElement;
                    XmlElement models = runtime.GetElementsByTagName("edmx:ConceptualModels")[0] as XmlElement;
                    XmlElement schema = models.GetElementsByTagName("Schema")[0] as XmlElement;
                    XmlNodeList entitys = schema.GetElementsByTagName("EntityType");
                    foreach (XmlElement entity in entitys)
                    {
                        string tablename = entity.Attributes["Name"].Value;
                        string tableAttr = GetTableAttr(tablename);
                        DataTable table = GetColumAttr(tablename);
                        XmlNodeList props = entity.GetElementsByTagName("Property");
                        foreach (XmlElement prop in props)
                        {
                            string rowname = prop.Attributes["Name"].Value;
                            DataRow[] rows = table.Select("column_name='" + rowname + "'");
                            if (rows.Length > 0)
                            {
                                DataRow row = rows[0];
                                if (row["column_description"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["column_description"].ToString()))
                                {
                                    Console.WriteLine(tablename + ":" + rowname + ":" + row["column_description"]);
                                }
                            }
                        }
                    }
                    break;
                }
            }
            Console.WriteLine("输出完成！等待退出！");
            Console.ReadKey();
        }

        /// <summary>
        /// 查询列注释
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static DataTable GetColumAttr(string tablename)
        {
            DataTable result = new DataTable();
            string sql = @"SELECT
                            A.name AS table_name,
                            B.name AS column_name,
                            C.value AS column_description
                            FROM sys.tables A
                            INNER JOIN sys.columns B ON B.object_id = A.object_id
                            LEFT JOIN sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id
                            WHERE A.name = '" + tablename + "'";
            result = SqlHelper.ExecuteDataset(sql).Tables[0];
            return result;
        }

        /// <summary>
        /// 查询表注释
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string GetTableAttr(string tablename)
        {
            string sql = @"select isnull(value,'') 
                            from sys.extended_properties ex_p 
                            where ex_p.minor_id=0 and ex_p.major_id in 
                                (select id from sys.sysobjects a where a.name='" + tablename + "')";
            object result = SqlHelper.ExecuteScalar(sql);
            return result == null ? "" : result.ToString();
        }
    }
}
