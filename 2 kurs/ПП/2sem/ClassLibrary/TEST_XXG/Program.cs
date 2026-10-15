
internal class Program
{
    private static void Main(string[] args)
    {
        DALMSQLXG.Init.Execute();

        DALJSONXG.Init.Execute();

        Console.WriteLine("\nТестирование DALMSQLXG:");
        using (var repo = DALMSQLXG.Repository.Create())
        {
            Console.WriteLine("\nВсе WSRef:");
            repo.getAllWSRef().ForEach(wsRef =>
            {
                Console.WriteLine($"{wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });

            Console.WriteLine("\nВсе Comment:");
            repo.getAllComment().ForEach(comment =>
            {
                Console.WriteLine($"{comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId}");
            });

            if (repo.addWSRef(new DALMSQLXG.WSRef() { Url = "https://www.belstu.by/", Description = "БГТУ", Minus = 0, Plus = 1 })) Console.WriteLine("Add new WSRef");
            else Console.WriteLine("WSRef add: ERROR");

            if (repo.addComment(new DALMSQLXG.Comment() { WSRefId = 4, Commtext = "test", Stamp = DateTime.Now })) Console.WriteLine("Comments: Add");
            else Console.WriteLine("Comments: Error Add");

            Console.WriteLine("-----------------------------------------------------------------");
            repo.getAllWSRef().ForEach(wsRef =>
            {
                Console.WriteLine($"{wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });
            repo.getAllComment().ForEach(comment =>
            {
                Console.WriteLine($"{comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId} ");
            });

            Console.WriteLine("\nПолучение комментария по id");
            DALMSQLXG.WSRef? firstWSRef = repo.getAllWSRef().FirstOrDefault();
            if (firstWSRef != null)
            {
                DALMSQLXG.Comment? foundComment = repo.GetCommentById(1);
                if (foundComment != null)
                {
                    Console.WriteLine($"Найденный комментарий: {foundComment.Id}, {foundComment.Commtext}, {foundComment.Stamp}, {foundComment.WSRefId}");
                }
                else
                {
                    Console.WriteLine("Коменнтариев не найдено");
                }
            }
        }

        Console.WriteLine("\n\nТестирование DALJSONXG:");
        using (var repo = DALJSONXG.Repository.Create())
        {
            Console.WriteLine("\nВсе WSRef:");
            repo.getAllWSRef().ForEach(wsRef =>
            {
                Console.WriteLine($"{wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });

            Console.WriteLine("\nВсе Comment:");
            repo.getAllComment().ForEach(comment =>
            {
                Console.WriteLine($"{comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId}");
            });

            if (repo.addWSRef(new DALJSONXG.WSRef() { Url = "https://www.belstu.by/", Description = "БГТУ", Minus = 0, Plus = 1 })) Console.WriteLine("Add new WSRef");
            else Console.WriteLine("WSRef add: ERROR");

            if (repo.addComment(new DALJSONXG.Comment() { WSRefId = 4, Commtext = "test", Stamp = DateTime.Now })) Console.WriteLine("Comments: Add");
            else Console.WriteLine("Comments: Error Add");

            Console.WriteLine("-----------------------------------------------------------------");
            repo.getAllWSRef().ForEach(wsRef =>
            {
                Console.WriteLine($"{wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });
            repo.getAllComment().ForEach(comment =>
            {
                Console.WriteLine($"{comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId} ");
            });

            Console.WriteLine("\nПолучение комментария по id");
            DALJSONXG.WSRef? firstWSRef = repo.getAllWSRef().FirstOrDefault();
            if (firstWSRef != null)
            {
                DALJSONXG.Comment? foundComment = repo.GetCommentById(1);
                if (foundComment != null)
                {
                    Console.WriteLine($"Найденный комментарий: {foundComment.Id}, {foundComment.Commtext}, {foundComment.Stamp}, {foundComment.WSRefId}");
                }
                else
                {
                    Console.WriteLine("Коменнтариев не найдено");
                }
            }
        }

        Console.WriteLine("\nТестирование завершено. Нажмите Enter для выхода...");
        Console.ReadLine();
    }
}