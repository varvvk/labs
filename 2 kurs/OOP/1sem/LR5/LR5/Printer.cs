using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public class Printer
    {
        public void IAmPrinting(Trial trialObj)
        {
            Console.WriteLine("Printing object type: " + trialObj.GetType().Name);
            Console.WriteLine(trialObj.ToString());
        }
    }
}
