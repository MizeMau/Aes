using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Aes.Database.Table
{
    public class Service<T> where T : class
    {
        protected static DBContext CreateContext()
        {
            // Create a new instance of the DbContext
            // You can configure options globally in DBContext itself (e.g. OnConfiguring)
            return new DBContext();
        }

        protected DbContext _context;

        public virtual IQueryable<T> GetQuery()
        {
            _context = CreateContext();
            return _context.Set<T>().AsQueryable();
        }

        public virtual List<T> GetAll()
        {
            using var context = CreateContext();
            return context.Set<T>().ToList();
        }

        public virtual T? GetById(long id)
        {
            using var context = CreateContext();
            return context.Set<T>().Find(id);
        }

        public virtual T Create(T entity)
        {
            using var context = CreateContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual IEnumerable<T> CreateRange(IEnumerable<T> entity)
        {
            using var context = CreateContext();
            context.Set<T>().AddRange(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual T Update(T entity)
        {
            using var context = CreateContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual bool Delete(long id)
        {
            using var context = CreateContext();
            var entity = context.Set<T>().Find(id);
            if (entity == null) return false;

            context.Set<T>().Remove(entity);
            context.SaveChanges();
            return true;
        }
    }
}
