using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelPlannerAPI.Models.Data;

namespace TravelPlannerAPI.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T?>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
        public async Task RemoveByUserIdAsync(int userId)
        {
            var property = typeof(T).GetProperty("UserId");
            if (property == null)
                throw new InvalidOperationException($"{typeof(T).Name} does not have a UserId property.");

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var constant = Expression.Constant(userId);
            var equality = Expression.Equal(propertyAccess, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            var entities = await _dbSet.Where(lambda).ToListAsync();
            _dbSet.RemoveRange(entities);
        }

    }
}
