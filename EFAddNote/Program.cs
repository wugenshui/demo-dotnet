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
            SqlHelper.connStr = "data source =.; initial catalog = CRM; persist security info = True; user id = sa; password = sa; ";

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
                    foreach (XmlNode entity in entitys)
                    {
                        string tablename = entity.Attributes["Name"].Value;
                        DataTable table = GetDocument(tablename);
                        XmlNodeList props = ((XmlElement)entity).GetElementsByTagName("Property");
                        foreach (XmlNode prop in props)
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
                }
            }
            Console.ReadKey();
        }

        public static DataTable GetDocument(string tablename)
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
    }
}
