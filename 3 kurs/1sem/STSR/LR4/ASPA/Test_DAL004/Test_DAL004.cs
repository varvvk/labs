using DAL004.Repositories;
using DAL004.Interfaces;

namespace DAL004.Test_DAL004
{
    class Test_DAL004
    {
        private static void Main(string[] args)
        {
            Repository.JSONFileName = "Celebrities.json";    // имя файла

            using (IRepository repository = Repository.Create("Celebrities"))  // директорий (в текущем директории приложения)
            {
                void print(string label)
                {
                    Console.WriteLine("--- " + label + " ---");
                    foreach (Celebrity celebrity in repository.getAllCelebrities())
                    {
                        Console.WriteLine($"Id = {celebrity.Id}, Firstname = {celebrity.Firstname}, " +
                            $"Surname = {celebrity.Surname}, PhotoPath = {celebrity.PhotoPath}");
                    }
                };

                print("start");

                int? testdel1 = repository.AddCelebrity(new Celebrity(8, "TestDel1", "TestDel1", "Photo/TestDel1.jpg"));
                int? testdel2 = repository.AddCelebrity(new Celebrity(9, "TestDel2", "TestDel2", "Photo/TestDel2.jpg"));
                int? testupd1 = repository.AddCelebrity(new Celebrity(10, "TestUpd1", "TestUpd1", "Photo/TestUpd1.jpg"));
                int? testupd2 = repository.AddCelebrity(new Celebrity(11, "TestUpd2", "TestUpd2", "Photo/TestUpd2.jpg"));
                print("add 4");

                if (testdel1 != null)
                    if (repository.DelCelebrityById((int)testdel1))
                        Console.WriteLine($" delete {testdel1} ");
                    else
                        Console.WriteLine($"delete {testdel1} error");

                if (testdel2 != null)
                    if (repository.DelCelebrityById((int)testdel2))
                        Console.WriteLine($" delete {testdel2} ");
                    else
                        Console.WriteLine($"delete {testdel2} error");

                if (repository.DelCelebrityById(1000))
                    Console.WriteLine($" delete {1000} ");
                else
                    Console.WriteLine($"delete {1000} error");

                repository.SaveChanges();
                print("del 2");

                if (testupd1 != null)
                {
                    int? result1 = repository.UpdCelebrityById((int)testupd1, new Celebrity(12, "Updated1", "Updated1", "Photo/Updated1.jpg"));
                    if (result1 != null)
                        Console.WriteLine($" update {testupd1} ");
                    else
                        Console.WriteLine($"update {testupd1} error");
                }

                if (testupd2 != null)
                {
                    int? result2 = repository.UpdCelebrityById((int)testupd2, new Celebrity(13, "Updated2", "Updated2", "Photo/Updated2.jpg"));
                    if (result2 != null)
                        Console.WriteLine($" update {testupd2} ");
                    else
                        Console.WriteLine($"update {testupd2} error");
                }

                int? result3 = repository.UpdCelebrityById(1000, new Celebrity(0, "Updated1000", "Updated1000", "Photo/Updated1000.jpg"));
                if (result3 != null)
                    Console.WriteLine($" update {1000} ");
                else
                    Console.WriteLine($"update {1000} error");

                repository.SaveChanges();
                print("upd 2");
            }
        }
    }
}
