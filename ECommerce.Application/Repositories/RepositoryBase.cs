using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ECommerceContext _DbContext { get; set; }
        public RepositoryBase(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        // Custom
        public virtual IQueryable<T> Query(Expression<Func<T, bool>> expression = null)
        {
            if (expression != null)
                return _DbContext.Set<T>().Where(expression).AsNoTracking();
            return _DbContext.Set<T>().AsNoTracking();
        }
        // Single Obj
        public virtual async Task<T> FindAsyncWhere(Expression<Func<T, bool>> expression)
        {
            return await _DbContext.Set<T>().Where(expression).FirstOrDefaultAsync();
        }
        // List
        public virtual async Task<IEnumerable<T>> ToListAsync()
        {
            return await _DbContext.Set<T>().ToListAsync();
        } 
        public virtual async Task<IEnumerable<T>> ToListAsyncWhere(Expression<Func<T, bool>> expression)
        {
            return await _DbContext.Set<T>().Where(expression).ToListAsync();
        }
        // Add
        public virtual async Task AddAsync(T entity)
        {
            if(entity != null)
                await _DbContext.Set<T>().AddAsync(entity);
        }
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if(entities != null && entities.Count() > 0)
                await _DbContext.Set<T>().AddRangeAsync(entities);
        }
        // Update
        public virtual void Update(T entity)
        {
            _DbContext.Set<T>().Update(entity);
        }
        // Remove
        public virtual async Task RemoveAsyncWhere(Expression<Func<T, bool>> expression)
        {
            var entity = await FindAsyncWhere(expression);
            if(entity != null)
                Remove(entity);
        }
        public virtual async Task RemoveRangeAsyncWhere(Expression<Func<T, bool>> expression)
        {
            var entities = await ToListAsyncWhere(expression);
            if(entities != null && entities.Count() > 0)
                RemoveRange(entities);
        }
        public virtual void Remove(T entity)
        {
            if(entity != null)
                _DbContext.Set<T>().Remove(entity);
        }
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if(entities != null && entities.Count() > 0)
                _DbContext.Set<T>().RemoveRange(entities);
        }
        // Save changes
        public virtual async Task SaveChangesAsync()
        {
            await _DbContext.SaveChangesAsync();
        }
    }
}
