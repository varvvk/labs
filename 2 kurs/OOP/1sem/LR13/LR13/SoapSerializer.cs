using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace LR13
{
    public class SoapSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var formatter = new DataContractSerializer(typeof(T));
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                formatter.WriteObject(fs, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            var formatter = new DataContractSerializer(typeof(T));
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                return (T)formatter.ReadObject(fs);
            }
        }
    }

}
