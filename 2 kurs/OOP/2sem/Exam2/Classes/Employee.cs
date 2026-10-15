using System;

class Employee
{
    // Поля
    private string name;
    private string position;
    private double salary;

    // Конструктор
    public Employee(string name, string position, double salary)
    {
        this.name = name;
        this.position = position;
        this.salary = salary;
    }

    // Метод для вывода информации
    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {name}");
        Console.WriteLine($"Должность: {position}");
        Console.WriteLine($"Зарплата: {salary} руб.");
    }
}