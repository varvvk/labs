using LR3.Lec03LibN;
using LR3.PP03;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Лабораторная работа #3");

        IFactory l1 = Lec03BLib.GetL1();

        Employee employee1 = new Employee(l1.getA(25));
        Console.WriteLine(string.Format($"Bonus-L1-A = {employee1.CalcBonus(4)}"));

        Employee employee2 = new Employee(l1.getB(25, 1.1f));
        Console.WriteLine(string.Format($"Bonus-L1-B = {employee2.CalcBonus(4)}"));

        Employee employee3 = new Employee(l1.getC(25, 1.1f, 5.0f));
        Console.WriteLine(string.Format($"Bonus-L1-C = {employee3.CalcBonus(4)}"));

        IFactory l2 = Lec03BLib.GetL2(1);

        Employee employee4 = new Employee(l2.getA(25));
        Console.WriteLine(string.Format($"Bonus-L2-A = {employee4.CalcBonus(4)}"));

        Employee employee5 = new Employee(l2.getB(25, 1.1f));
        Console.WriteLine(string.Format($"Bonus-L2-B = {employee5.CalcBonus(4)}"));
            
        Employee employee6 = new Employee(l2.getC(25, 1.1f, 5.0f));
        Console.WriteLine(string.Format($"Bonus-L2-C = {employee6.CalcBonus(4)}"));

        IFactory l3 = Lec03BLib.GetL3(1, 0.5f);

        Employee employee7 = new Employee(l3.getA(25));
        Console.WriteLine(string.Format($"Bonus-L3-A = {employee7.CalcBonus(4)}"));

        Employee employee8 = new Employee(l3.getB(25, 1.1f));
        Console.WriteLine(string.Format($"Bonus-L3-B = {employee8.CalcBonus(4)}"));

        Employee employee9 = new Employee(l3.getC(25, 1.1f, 0.5f));
        Console.WriteLine(string.Format($"Bonus-L3-C = {employee9.CalcBonus(4)}"));
    }
}