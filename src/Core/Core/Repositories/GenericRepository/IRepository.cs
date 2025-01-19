using System.Linq.Expressions;

namespace Core.Repositories.GenericRepository
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        T GetById<T1>(T1 id);
        T GetById(Expression<Func<T, bool>> filter);
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        List<T> GetAllOrdered<TKey>(Expression<Func<T, bool>> filter = null, Expression<Func<T, TKey>> orderByKeySelector = null, bool orderByDescending = false);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

}
