using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR
{
    internal class Program
    {
        //1.1
        public enum MyEnum
        {
            january = 1,
            february,
            march,
            april,
            may,
            june,
            july,
            august,
            september,
            october,
            november,
            december
        }

        //2
        public interface IComparable
        {
            int CompareTo(object obj);
        }

        public class Computer: IComparable
        {
            public string name;

            public Computer(string name)
            {
                this.name = name;
            }

            public int CompareTo(object obj) 
            {
                return 0;
            }
        }

        //3
        public interface IGood
        {
            void Fine();
        }
        public abstract class Something
        {
            public abstract void ItsOk();
        }

        public class Case: Something, IGood
        {
            public override void ItsOk()
            {
                Console.WriteLine("Its not ok");
            }
            public void Fine()
            {
                Console.WriteLine("Its fine)))");
            }
        }
        static void Main(string[] args)
        {
            //1.1
            /*foreach (int item in MyEnum)
            {
                Console.WriteLine(item);
            }*/

            //1.2
            string str1 = "123.456.789";
            Console.WriteLine(str1.Replace(".", "\n"));

            //2
            Computer computer = new Computer("First");
            Computer computer2 = new Computer("Second");
            Console.WriteLine(computer.CompareTo(computer2));


            //3
            Case case1 = new Case();
            case1.Fine();
            case1.ItsOk();
        }
    }
}
