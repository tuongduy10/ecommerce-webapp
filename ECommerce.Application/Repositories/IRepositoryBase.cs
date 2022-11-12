using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        // Custom
        IQueryable<T> Query(Expression<Func<T, bool>> expression = null);
        // Single obj
        Task<T> FindAsyncWhere(Expression<Func<T, bool>> expression);
        // List
        Task<IEnumerable<T>> ToListAsync();
        Task<IEnumerable<T>> ToListAsyncWhere(Expression<Func<T, bool>> expression);
        // Add
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        // Update
        void Update(T entity);
        // Remove
        Task RemoveAsyncWhere(Expression<Func<T, bool>> expression);
        Task RemoveRangeAsyncWhere(Expression<Func<T, bool>> expression);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        // Save changes
        Task SaveChangesAsync();
    }
}
