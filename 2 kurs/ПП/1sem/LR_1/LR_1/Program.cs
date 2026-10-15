using System;

namespace LR_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Student student1 = new Student();
            Console.WriteLine("Student 1 (Constructor by default):");
            student1.PrintInfo();
            Console.WriteLine();

            Student student2 = new Student("Gavrilkyna", "Lisa", "+375294567890", "Minsk", "Active");
            Console.WriteLine("Student 2 (Constructor with parameters):");
            student2.PrintInfo();
            Console.WriteLine();

            Student student3 = new Student(student2)
            {
                surname = "Ivanova"
            };
            Console.WriteLine("Student 3 (Copy):");
            student3.PrintInfo();
            Console.WriteLine();

            student3.ShowProtectedInfo();
            Console.WriteLine();
            student3.ShowPrivateInfo();
            Console.WriteLine();

            Console.WriteLine($"Значение из свойства: {student3.Address}");
            student3.Address = "Grodno";
            Console.WriteLine($"New addres: {student3.Address}");
            Console.WriteLine();
            Console.WriteLine();


            C2 student2_1 = new C2();
            Console.WriteLine("Информация о студенте класса 2 (по умолчанию):");
            student1.PrintInfo();
            Console.WriteLine();

            Console.WriteLine(student2_1[1]);
            student2_1[1] = "Новое значение";
            Console.WriteLine();

            student2_1.Event += student2_1.StudentEvent_Handler;
            student2_1.TriggerEvent();
            Console.WriteLine();
            Console.WriteLine();


            Animals animal = new Animals("Лев", 5, 10);
            animal.PublicMethod();

            Cat cat = new Cat("Мурка", "Сиамская", 3, 7);
            cat.PublicMethod();
            cat.name = "Пушинка";
            Console.Write("Changed: "); 
            cat.PublicMethod();
            cat.CallProtectedMethod();
        }
        
    }
}