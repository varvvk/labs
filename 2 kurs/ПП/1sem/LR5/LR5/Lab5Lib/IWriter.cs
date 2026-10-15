using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable

namespace LR5.Lab5Lib
{
    public interface IWriter
    {
        string? Save(string? message);
    }
}
