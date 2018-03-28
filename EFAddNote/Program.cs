using System;
using System.Collections.Generic;
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
                    XmlNode runtime = root.GetElementsByTagName("edmx:Runtime")[0];
                    XmlNode models = root.GetElementsByTagName("edmx:ConceptualModels")[0];
                    XmlNode schema = root.GetElementsByTagName("Schema")[0];
                    XmlNodeList entitys = root.GetElementsByTagName("EntityType");
                    foreach (XmlNode entity in entitys)
                    {
                        string tablename = entity.Attributes["Name"].Value;
                        XmlNodeList props = root.GetElementsByTagName("Property");
                        foreach (XmlNode prop in props)
                        {
                            string rowname = prop.Attributes["Name"].Value;
                            Console.WriteLine(tablename + ":" + rowname);
                        }
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
