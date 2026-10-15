using System;
using System.Text.Json;
using System.IO;

namespace LR13
{
    public class JsonnSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            string json = JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath, json);
        }

        public T Deserialize<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
