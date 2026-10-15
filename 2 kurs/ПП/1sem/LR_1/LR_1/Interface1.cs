using System;

namespace LR_1
{
    internal interface Interface1
    {
        string Address { get; set; }
        void PrintInfo();

        event EventHandler Event;
        string this[int index] { get; set; }
    }
}
