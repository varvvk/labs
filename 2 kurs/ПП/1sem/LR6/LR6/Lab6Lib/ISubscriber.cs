using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6.Lab6Lib
{
    public interface ISubscriber
    {
        void update(string eventname);
    }
}
