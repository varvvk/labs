using System;
using System.Collections.Generic;
using System.IO;
using System.IO;
using System.Runtime.Serialization;

namespace LR13
{
    public class BinarySerializer : ISerializer
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
