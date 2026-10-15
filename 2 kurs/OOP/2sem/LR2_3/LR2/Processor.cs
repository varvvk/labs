using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    public class Processor
    {
        public string Manufacturer { get; set; }
        public string Series { get; set; }
        public string Model { get; set; }
        public int Cores { get; set; }
        public double Frequency { get; set; }
        public int Architecture { get; set; }
        public int CacheL1 { get; set; }
        public int CacheL2 { get; set; }
        public int CacheL3 { get; set; }
    }

}
