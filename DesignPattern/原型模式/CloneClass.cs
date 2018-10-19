using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    [Serializable]
    class CloneClass : ICloneable
    {
        public int i = 0;
        public int[] iArr = { 1, 2, 3 };

        /// <summary>
        /// 浅拷贝，会复制对象的引用
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone() as CloneClass;
        }

        /// <summary>
        /// 深拷贝，不会复制对象的引用
        /// </summary>
        /// <returns></returns>
        public CloneClass DeepClone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as CloneClass;
        }
    }
}
