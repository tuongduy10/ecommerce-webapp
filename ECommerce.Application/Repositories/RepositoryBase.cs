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
        protected DbSet<T> _DbSet { get; set; }
        public RepositoryBase(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public virtual async Task<IEnumerable<T>> FindAll()
        {
            return await _DbSet.ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await _DbSet.Where(expression).ToListAsync();
        }
        public virtual async Task<bool> Create(T entity)
        {
            await _DbSet.AddAsync(entity);
            return true;
        }
        public virtual async Task<bool> Update(T entity)
        {
            _DbSet.Update(entity);
            return true;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            return true;
        }
    }
}
