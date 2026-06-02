using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>>? filter,
            bool trackChanges,
            string? includeProperties = null);

        Task<T?> Get(Expression<Func<T, bool>> filter);
        Task<T?> Get(
            Expression<Func<T, bool>> filter,
            bool trackChanges,
            string? includeProperties = null);

        Task Add(T entity);
        void Remove(T entity);
    }
}
