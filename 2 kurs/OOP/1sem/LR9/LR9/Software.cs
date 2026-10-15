using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR9
{
    public class Software
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public Software(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            return $"{Name} (Version: {Version})";
        }
    }
}
