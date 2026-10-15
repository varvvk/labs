using System;

class Program
{
    static void Main(string[] args)
    {
        
        CheckedFunction();
        
        UncheckedFunction();
    }

    static void CheckedFunction()
    {
        try
        {
            int maxValue = int.MaxValue; 
            int result = checked(maxValue + 1);
            Console.WriteLine($"Checked result: {result}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"Overflow occurred in CheckedFunction: {ex.Message}");
        }
    }

    static void UncheckedFunction()
    {
        int maxValue = int.MaxValue; 
        int result = unchecked(maxValue + 1);
        Console.WriteLine($"Unchecked result: {result}");
    }
}
