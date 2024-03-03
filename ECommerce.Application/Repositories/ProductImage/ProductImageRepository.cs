using ECommerce.Application.Repositories.User;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.ProductImage
{
    public class ProductImageRepository : RepositoryBase<Data.Entities.ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ECommerceContext DbContext) : base(DbContext)
        {

        }
    }
}
