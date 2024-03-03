using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.ProductUserImage
{
    public class ProductUserImageRepository : RepositoryBase<Data.Entities.ProductUserImage>, IProductUserImageRepository
    {
        public ProductUserImageRepository(ECommerceContext DbContext) : base(DbContext)
        {

        }
    }
}
