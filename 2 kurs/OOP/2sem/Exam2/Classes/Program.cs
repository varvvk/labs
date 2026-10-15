using Classes;

public class Program
{
    static void Main(string[] args)
    {
        List<Books> books = new List<Books>
        {
            new Books("a", "a", "a"),
            new Books("b", "b", "b"),
            new Books("c", "c", "c")
        };

        foreach (Books book in books)
        {
            book.PrintBooks();
        }


        Car car = new Car();

        car.Drive();

        Rectangle rec = new Rectangle(2, 4);
        rec.GetPerimeter(rec);
        rec.GetArea(rec);

        Person person1 = new Person(16, "not adult");
        Person person2 = new Person(19, "adult");
        person1.IsAdult();
        person2.IsAdult();

        Circle circle = new Circle(2);
        Console.WriteLine($"Circle length:{circle.Lenth():F2}m, area: {circle.Square()}m^2");

        BankAccount bankAccount = new BankAccount("Varya", 200);
        bankAccount.Deposit(100);
        bankAccount.Withdraw(10);
        bankAccount.GetBalance();

        Movie movie = new Movie("a", "a", new DateTime(2010, 7, 7));
        movie.PrintInfo();

        Product product = new Product("Ноутбук", 50000);
        product.ApplyDiscount(50);
        product.PrintInfo();

        List<int> grades = new List<int> { 4, 5, 4, 5 };
        Student student = new Student("Алексей", 20, grades);
        Console.WriteLine($"Average: {student.CalculateAverage()}"); 
    }
}