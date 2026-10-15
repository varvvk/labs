using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace task_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] myArray = {1,2,3,4};
            string str1 = "Hello, World";

            (int max, int min, int sum, char firstLetter) LocFun ()
            {
                int max = myArray[0];
                int min = myArray[0];
                int sum = 0;

                foreach (int num in myArray)
                {
                    if (num > max) max = num;
                    if (num < min) min = num;
                    sum += num;
                }
                char firstLetter = str1[0];

                return (max, min, sum, firstLetter);
            }

            Console.WriteLine(LocFun());
        }
    }
}
