using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public partial class Test : Trial, ICloneable
    {
        public override string ToString()
        {
            return base.ToString() + " (Type: Test)";
        }

        public override bool Equals(object obj)
        {
            if (obj is Test otherTest)
            {
                return Name == otherTest.Name && Date == otherTest.Date && Duration == otherTest.Duration;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Date, Duration);
        }
    }
}
