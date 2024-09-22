using Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Repositories.GenericRepository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T GetById<T1>(T1 id)
        {
            return _dbSet.Find(id);
        }

        public T GetById(Expression<Func<T, bool>> filter)
        {
            if(filter is not null)
            {
                return _dbSet.FirstOrDefault(filter);
            }

            return null;
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter != null
                ? _dbSet.Where(filter).ToList()
                : _dbSet.ToList();
        }

        public List<T> GetAllOrdered<TKey>(
            Expression<Func<T, bool>> filter = null,
            Expression<Func<T, TKey>> orderByKeySelector = null,
            bool orderByDescending = false)
        {
            if (filter is not null)
            {
                if (orderByKeySelector is null)
                {
                    return _dbSet.Where(filter).ToList();
                }

                if (orderByDescending)
                {
                    return _dbSet.Where(filter).OrderByDescending(orderByKeySelector).ToList();
                }

                return _dbSet.Where(filter).OrderBy(orderByKeySelector).ToList();
            }

            if (orderByKeySelector is not null)
            {
                if (orderByDescending)
                {
                    return _dbSet.OrderByDescending(orderByKeySelector).ToList();
                }

                return _dbSet.OrderBy(orderByKeySelector).ToList();
            }

            return _dbSet.ToList();
        }


        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
