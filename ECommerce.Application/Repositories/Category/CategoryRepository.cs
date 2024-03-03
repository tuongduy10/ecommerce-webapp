using ECommerce.Application.Repositories.Bank;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Category
{
    public class CategoryRepository : RepositoryBase<Data.Entities.Category>, ICategoryRepository
    {
        public CategoryRepository(ECommerceContext DbContext) : base(DbContext)
        {

        }
    }
}
