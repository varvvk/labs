using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LR8
{
    public class Class1
    {

        public delegate string MyDelegate(int x);

        public event MyDelegate MyEvent;

        public string Start(int x)
        {
            foreach (MyDelegate handler in MyEvent.GetInvocationList())
            {
                string result = handler(x);  
                Console.WriteLine(result);  
            }
            return "Finished";
        }
        public string Method(int x)
        {
            int res = x + x;
            return $"add {res}";
        }

        public string BBB(int x)
        {
            int res = x * x;
            return $"not add {res}";
        }
        
    }
}
