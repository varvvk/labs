using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR7
{
    public interface IInterface<T>
    {
        void Add(T item);
        bool Remove(T item);
        void View();
    }
}
