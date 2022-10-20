using ECommerce.Application.Repositories.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
{
    public interface IRepositoryWrapper
    {
        ICommentRepository Comment { get; }
        Task SaveAsync();
    }
}
