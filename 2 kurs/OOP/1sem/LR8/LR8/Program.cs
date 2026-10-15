using System;
using System.Linq;

namespace LR8
{
    
    class Technic
    {
        public string Name { get; set; }
        public bool IsFunctional { get; set; }

        public Technic(string name, bool isFunctional)
        {
            Name = name;
            IsFunctional = isFunctional;
        }

        public void Break()
        {
            IsFunctional = false;
            Console.WriteLine($"{Name} сломался!");
        }

        public void Upgrade() =>
            Console.WriteLine(IsFunctional ? $"{Name} улучшен!" : $"{Name} сломан.");

        public void TurnOn(int voltage)
        {
            if (!IsFunctional)
            {
                Console.WriteLine($"{Name} сломан");
                return;
            }

            if (voltage > 220)
            {
                Break();
            }
            else
            {
                Console.WriteLine($"{Name} включен под напряжением {voltage}В.");
            }
        }
    }

    class SemiTechnic
    {
        public string Name { get; set; }
        public bool IsFunctional { get; set; }

        public SemiTechnic(string name, bool isFunctional)
        {
            Name = name;
            IsFunctional = isFunctional;
        }

        public void Break()
        {
            IsFunctional = false;
            Console.WriteLine($"{Name} сломался!");
        }

        public void Upgrade() =>
            Console.WriteLine(IsFunctional ? $"{Name} улучшен!" : $"{Name} сломан.");

        public void TurnOn(int voltage)
        {
            if (!IsFunctional)
            {
                Console.WriteLine($"{Name} уже сломан");
                return;
            }

            if (voltage > 140)
            {
                Break(); 
            }
            else
            {
                Console.WriteLine($"{Name} активирован под напряжением {voltage}В.");
            }
        }
    }

    
    class Program
    {
        delegate void Messege();
        delegate int Operation(int x);
        static void Main(string[] args)
        {
            var technic1 = new Technic("Станок-1", true);
            var semiTechnic1 = new SemiTechnic("Полутехник-А", true);

            var boss = new Boss();

            boss.Upgrade += technic1.Upgrade;
            boss.Upgrade += semiTechnic1.Upgrade;

            boss.TurnOn +=  technic1.TurnOn;
            boss.TurnOn += semiTechnic1.TurnOn;

            boss.HaveUpgrade();
            Console.WriteLine();
            boss.HaveTurnOn(220);
            Console.WriteLine();


            string inputString = "Hello, World!";

            Action<string> removePunctuation = (s) =>
            {
                string cleanedString = new string(s.ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray());
                Console.WriteLine($"Удаление знаков препинания: {cleanedString}");
            };

            Func<string, string, string> insertText = (s, text) =>
            {
                int indexToInsert = s.Length / 2;
                string result = s.Insert(indexToInsert, text);
                Console.WriteLine($"Вставка текста в середину строки: {result}");
                return result;
            };

            Func<string, string> toUpperCase = (s) =>
            {
                string upperCaseString = s.ToUpper();
                Console.WriteLine($"Преобразование строки в верхний регистр: {upperCaseString}");
                return upperCaseString;
            };

            Func<string, string> replaceChars = (s) =>
            {
                string replacedString = s.Replace('O', 'X');
                Console.WriteLine($"Замена 'O' на 'X': {replacedString}");
                return replacedString;
            };

            Predicate<string> startsWithH = (s) =>
            {
                bool startsWithHResult = s.StartsWith("H");
                Console.WriteLine($"Строка начинается с 'H': {startsWithHResult}");
                return startsWithHResult;
            };

            removePunctuation(inputString);
            string result1 = insertText(inputString, "G");
            string result2 = toUpperCase(result1);
            replaceChars(result2);
            startsWithH(result2);








            Class1 myClass = new Class1();
            

            myClass.MyEvent += myClass.Method;
            myClass.MyEvent += myClass.BBB;

            myClass.Start(2);

        }
    }
}
