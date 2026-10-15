using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPO
{
    public interface IRepository<WSRef, Comment> : IDisposable             //  наследует  IDisposable
    {
        List<WSRef> getAllWSRef();                      // получить весь перечень Интернет-ресурсов 
        List<Comment> getAllComment();                    // получить все комментарии  
        bool addWSRef(WSRef wsRef);
        bool addComment(Comment comment);
        Comment? GetCommentById(int id);
    }
}
