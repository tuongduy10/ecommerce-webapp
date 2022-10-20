using ECommerce.Application.Repositories.Comment;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ECommerceContext _DbContext;
        private ICommentRepository _comment;

        public RepositoryWrapper(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null)
                {
                    _comment = new CommentRepository(_DbContext);
                }
                return _comment;
            }
        }
        public async Task SaveAsync()
        {
            await _DbContext.SaveChangesAsync();
        }
    }
}
