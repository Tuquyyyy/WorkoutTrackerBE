using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Workout.Application.Common.Interfaces;
using Workout.Infrastructure.Data;

namespace Workout.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter)
        {
            return await dbSet
                .AsNoTrackingWithIdentityResolution()
                .Where(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>>? filter,
            bool trackChanges,
            string? includeProperties = null)
        {
            IQueryable<T> query = trackChanges
                ? dbSet
                : dbSet.AsNoTrackingWithIdentityResolution();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(prop.Trim());

            return await query.ToListAsync();
        }

        public async Task<T?> Get(Expression<Func<T, bool>> filter)
        {
            return await dbSet
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(filter);
        }

        public async Task<T?> Get(
            Expression<Func<T, bool>> filter,
            bool trackChanges,
            string? includeProperties = null)
        {
            IQueryable<T> query = trackChanges
                ? dbSet
                : dbSet.AsNoTrackingWithIdentityResolution();

            if (!string.IsNullOrEmpty(includeProperties))
                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(prop.Trim());

            return await query.FirstOrDefaultAsync(filter);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
