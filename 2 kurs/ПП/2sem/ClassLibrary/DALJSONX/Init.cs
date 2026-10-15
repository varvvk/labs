using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALJSONX
{
    public class Init
    {
        public static void Execute()
        {
            using var context = JSONContext.Create("WSRefX.json");
            if (!context.WSRefs.Any())
            {
                var refs = new List<WSRef>
            {
                new WSRef { Description = "Oracle", Url = "https://www.oracle.com", Minus = 0, Plus = 0 },
                new WSRef { Description = "Java", Url = "https://jakarta.ee/", Minus = 0, Plus = 0 },
                new WSRef { Description = "JavaScript", Url = "https://ecma-international.org/", Minus = 0, Plus = 0 }
            };
                refs.ForEach(r => context.addWSRef(r));
                context.SaveChanges();

                var comments = new List<Comment>();
                foreach (var wsRef in refs)
                {
                    comments.Add(new Comment { WSRefId = wsRef.Id, Stamp = DateTime.Now, Commtext = $"{wsRef.Id}-Comment1" });
                    comments.Add(new Comment { WSRefId = wsRef.Id, Stamp = DateTime.Now, Commtext = $"{wsRef.Id}-Comment2" });
                }
                comments.ForEach(c => context.addComment(c));
                context.SaveChanges();
            }
        }
    }
}
