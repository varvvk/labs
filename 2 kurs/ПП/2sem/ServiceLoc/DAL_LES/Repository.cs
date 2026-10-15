using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_LES
{
    public class Repository : IRepository
    {
        private readonly Context _context;

        public Repository()
        {
            _context = new Context();
        }

        // Celebrity methods
        public List<Celebrity> GetAllCelebrities()
        { return _context.Celebrities.ToList(); }

        public Celebrity? GetCelebrityById(int id)
        {
            return _context.Celebrities.Find(id);
        }

        public bool AddCelebrity(Celebrity celebrity)
        {
            _context.Celebrities.Add(celebrity);
            return _context.SaveChanges() > 0;
        }

        public bool DelCelebrity(int id)
        {
            var celeb = _context.Celebrities.Find(id);
            if (celeb == null) return false;
            _context.Celebrities.Remove(celeb);
            return _context.SaveChanges() > 0;
        }

        public bool UpdCelebrity(int id, Celebrity celebrity)
        {
            var existing = _context.Celebrities.Find(id);
            if (existing == null) return false;

            existing.FullName = celebrity.FullName;
            existing.Nationality = celebrity.Nationality;
            existing.ReqPhotoPath = celebrity.ReqPhotoPath;

            return _context.SaveChanges() > 0;
        }

        // Lifeevent methods
        public List<Lifeevent> GetAllLifeevents()
        {return _context.Lifeevents.ToList(); }

        public Lifeevent? GetLifeeventById(int id)
        {
            return _context.Lifeevents.Find(id);
        }

        public bool AddLifeevent(Lifeevent lifeevent)
        {
            _context.Lifeevents.Add(lifeevent);
            return _context.SaveChanges() > 0;
        }

        public bool DelLifeevent(int id)
        {
            var lifeevent = _context.Lifeevents.Find(id);
            if (lifeevent == null) return false;
            _context.Lifeevents.Remove(lifeevent);
            return _context.SaveChanges() > 0;
        }

        public bool UpdLifeevent(int id, Lifeevent lifeevent)
        {
            var existing = _context.Lifeevents.Find(id);
            if (existing == null) return false;

            existing.Description = lifeevent.Description;
            existing.Date = lifeevent.Date;
            existing.CelebrityId = lifeevent.CelebrityId;

            return _context.SaveChanges() > 0;
        }

        // Common methods
        public List<Lifeevent> GetLifeeventsByCelebrityId(int celebrityId)
        { return _context.Lifeevents.Where(le => le.CelebrityId == celebrityId).ToList(); }

        public Celebrity GetCelebrityByLifeeventId(int lifeeventId)
        {
            var lifeevent = _context.Lifeevents.Find(lifeeventId);
            if (lifeevent == null) return null;

            return _context.Celebrities.Find(lifeevent.CelebrityId);
        }

        public void Dispose() => _context.Dispose();
    }

}
