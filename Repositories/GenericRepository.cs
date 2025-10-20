using ActiveControlApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace ActiveControlApi.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
           _context = context; 

        }
        public async Task<T?> Get(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }

        public IQueryable<T> GetQueryble()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<bool> Any(Expression<Func<T,bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
    }
}
