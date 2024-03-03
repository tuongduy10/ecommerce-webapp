using ECommerce.Application.Repositories.Category;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Brand
{
    public class BrandRepository : RepositoryBase<Data.Entities.Brand>, IBrandRepository
    {
        public BrandRepository(ECommerceContext DbContext) : base(DbContext)
        {

        }
    }
}
