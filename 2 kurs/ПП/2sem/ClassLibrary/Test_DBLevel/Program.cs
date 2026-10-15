using DBLevel;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("------>Start \n");
        Init.Execute();

        using (IRepository repo = Repository.Create())
        {
            repo.getAllWSRef().ForEach(wsRef => {
                Console.WriteLine($"{wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });
            repo.getAllComment().ForEach(comment => {
                Console.WriteLine($"{comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId} ");
            });
            
            if (repo.addWSRef(new WSRef() { Url = "https://www.belstu.by/", Description = "БГТУ", Minus = 0, Plus = 1 })) Console.WriteLine("Add new WSRef");
            else Console.WriteLine("WSRef add: ERROR");

            if (repo.addComment(new Comment() { WSRefId = 4, Commtext = "test", Stamp = DateTime.Now })) Console.WriteLine("Comments: Add");
            else Console.WriteLine("Comments: Error Add");

            Console.WriteLine("-----------------------------------------------------------------");
            repo.getAllWSRef().ForEach(wsRef => {
                Console.WriteLine($"{wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });
            repo.getAllComment().ForEach(comment => {
                Console.WriteLine($"{comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId} ");
            });
        }

        Console.ReadKey();
    }
}