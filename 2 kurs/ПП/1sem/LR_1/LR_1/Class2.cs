using System;

namespace LR_1
{
    internal class C2 : Student, Interface1
    {
        public string this[int index]
        {
            get { return $"Элемент {index}"; }
            set { Console.WriteLine($"Установлено: {value} для индекса {index}"); }
        }

        public event EventHandler Event;
        public void TriggerEvent()
        {
            Event?.Invoke(this, EventArgs.Empty);
        }
        public void StudentEvent_Handler(object sender, EventArgs e)
        {
            Console.WriteLine("Событие TriggerEvent было вызвано!");
        }
    }
}
