using System.Linq.Expressions;
namespace ActiveControlApi.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
        Task<T?> Get(Expression<Func<T, bool>> predicate); 
        T Create(T  entity);
        T Update(T entity);
        T Delete(T entity);
        IQueryable<T> GetQueryble();
        Task<bool> Any(Expression<Func<T, bool>> predicate); 

    }
}
