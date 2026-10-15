using System;
using DALJSONX;
using REPO;

internal class Program
{
    private static void Main(string[] args)
    {
        DALJSONX.Init.Execute();

        Console.WriteLine("Start");
        using (IRepository<WSRef, Comment> repo = Repository.Create())
        {
            repo.getAllWSRef().ForEach(wsRef => {
                Console.WriteLine($"WSRefs: {wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });

            repo.getAllComment().ForEach(comment => {
                Console.WriteLine($"Comments {comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId} ");
            });

            if (repo.addWSRef(new WSRef() { Url = "https://www.belstu.by/", Description = "БГТУ", Minus = 0, Plus = 0 })) Console.WriteLine("WSRefs: Add");
            else Console.WriteLine("WSRefs: Error Add");

            if (repo.addComment(new Comment() { WSRefId = 4, Commtext = "test", Stamp = DateTime.Now })) Console.WriteLine("Comments: Add");
            else Console.WriteLine("Comments: Error Add");

            Console.WriteLine("After addWSRef, addComment ");

            repo.getAllWSRef().ForEach(wsRef => {
                Console.WriteLine($"WSRefs: {wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });

            repo.getAllComment().ForEach(comment => {
                Console.WriteLine($"Comments {comment.Id}: {comment.Commtext}, {comment.Stamp}, {comment.WSRefId} ");
            });

            Console.WriteLine("\nTesting GetCommentById...");
            WSRef? firstWSRef = repo.getAllWSRef().FirstOrDefault();
            if (firstWSRef != null)
            {
                Comment? foundComment = repo.GetCommentById(1);
                if (foundComment != null)
                {
                    Console.WriteLine($"Found Comment: {foundComment.Id}, {foundComment.Commtext}, {foundComment.Stamp}, {foundComment.WSRefId}");
                }
                else
                {
                    Console.WriteLine("No comments found for the given WSRef.");
                }
            }
            else
            {
                Console.WriteLine("No WSRef entries available.");
            }
        }

        Console.WriteLine("Finish");
        Console.ReadLine();
    }
}
