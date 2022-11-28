using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        protected readonly AppDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                List<string> includePropertiesArray = includeProperties.Split(',').ToList();
                foreach (string includeProperty in includePropertiesArray)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                List<string> includePropertiesArray = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                foreach (string includeProperty in includePropertiesArray)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
