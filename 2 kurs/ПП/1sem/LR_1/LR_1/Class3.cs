using System;

namespace LR_1
{
    internal class Animals
    {
        private int privateField;
        protected int protectedField;
        public string name;

        public Animals(string name, int protectedField, int privateField)
        {
            this.name = name;
            this.protectedField = protectedField;
            this.privateField = privateField;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private void PrivateMethod()
        {
            Console.WriteLine("Это private метод базового класса.");
        }

        protected void ProtectedMethod()
        {
            Console.WriteLine("Это protected метод базового класса.");
        }

        public void PublicMethod()
        {
            Console.WriteLine($"Name: {name}, {protectedField}, {privateField} ");
        }
    }
}
