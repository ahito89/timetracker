using System.Linq;

namespace timetracker.Interfaces
{
    public interface IRepository<T>
    {
        T Get (long id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
    }
}