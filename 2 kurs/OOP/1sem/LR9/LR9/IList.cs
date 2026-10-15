using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR9
{
    public interface IList<T>
    {
        void Add(int key, T item);
        void Remove(int key);
        void Find(int key);
        void DisplayAll();
    }
}
