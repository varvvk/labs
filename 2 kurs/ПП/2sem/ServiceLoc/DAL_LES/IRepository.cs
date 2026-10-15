using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_LES
{
    public interface IRepository : ICommon<Celebrity, Lifeevent>, ICelebrity, ILifeevent, IDisposable { }
    public interface ICelebrity<T> : IDisposable
    {
        List<T> GetAllCelebrities();
        T? GetCelebrityById(int Id);
        bool DelCelebrity(int id);
        bool AddCelebrity(T celebrity);
        bool UpdCelebrity(int id, T celebrity);

    }

    public interface ILifeevent<T> : IDisposable
    {
        List<T> GetAllLifeevents();
        T? GetLifeeventById(int Id); 
        bool DelLifeevent(int id); 
        bool AddLifeevent(T lifeevent);
        bool UpdLifeevent(int id, T lifeevent);

    }

    public interface ICommon<T1, T2>
    {
        List<T2> GetLifeeventsByCelebrityId(int celebrityId);
        T1 GetCelebrityByLifeeventId(int lifeeventId);
    }
}
