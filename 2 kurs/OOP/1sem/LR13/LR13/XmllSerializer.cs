using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace LR13
{
    public class XmllSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                return (T)xmlSerializer.Deserialize(fs);
            }
        }
    }
}
