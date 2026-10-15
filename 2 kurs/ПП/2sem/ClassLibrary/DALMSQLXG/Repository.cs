using GREPO;

namespace DALMSQLXG
{
    public class Repository : IRepository<WSRef, Comment>
    {
        private readonly Context _context;

        private Repository()
        {
            _context = new Context();
        }

        public static IRepository<WSRef, Comment> Create() { return new Repository(); }

        public List<WSRef> getAllWSRef()
        {
            return _context.WSRefs.ToList();
        }

        public List<Comment> getAllComment()
        {
            return _context.Comments.ToList();
        }

        public bool addWSRef(WSRef wsRef)
        {
            _context.WSRefs.Add(wsRef);
            return _context.SaveChanges() > 0;
        }
        public bool addComment(Comment comment)
        {
            _context.Comments.Add(comment);
            return _context.SaveChanges() > 0;
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
