
using REPO;

namespace DALJSONX
{
    public class Repository : IRepository<WSRef, Comment>
    {
        private JSONContext _context;

        private Repository()
        {
            _context = new JSONContext("WSRefX.json");
        }

        public static IRepository<WSRef, Comment> Create()
        {
            return new Repository();
        }

        public List<WSRef> getAllWSRef()
        {
            return _context.WSRefs;
        }

        public List<Comment> getAllComment()
        {
            return _context.Comments;
        }

        public bool addWSRef(WSRef wsRef)
        {
            wsRef.Id = (_context.WSRefs.Max(w => (int?)w.Id) ?? 0) + 1;
            _context.WSRefs.Add(wsRef);
            _context.SaveChanges();
            return true;
        }

        public bool addComment(Comment comment)
        {
            bool rc = false;
            if (_context.addComment(comment)) rc = (_context.SaveChanges() > 0);
            return rc;
        }

        public Comment? GetCommentById(int id)
        {
            return _context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
