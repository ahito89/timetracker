using timetracker.Models;
using timetracker.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace timetracker.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        protected DbSet<T> _entities;

        public Repository(TimeTrackerDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }


        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            Save();
        }

        public T Get(long id)
        {
            return _entities.SingleOrDefault(s => s.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return _entities.AsQueryable();
        }
        
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            Save();
        }
        private void Save()
        {
            _context.SaveChanges();
        }
    }
}