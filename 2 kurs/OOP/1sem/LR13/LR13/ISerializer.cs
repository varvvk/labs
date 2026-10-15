using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR13
{
    public interface ISerializer
    {
        void Serialize<T>(T obj, string filePath);
        T Deserialize<T>(string filePath);
    }
}
