using System;

namespace LR_1
{
    internal class Cat : Animals
    {
        public string breed;

        public Cat(string name, string breed, int protectedField, int privateField)
            : base(name, protectedField, privateField)
        {
            this.breed = breed;
        }

        public new void PublicMethod()
        {
            Console.WriteLine($"Name: {name}, Breed: {breed}, ProtectedField: {protectedField}");
        }

        public void CallProtectedMethod()
        {
            ProtectedMethod();
        }
    }

}
