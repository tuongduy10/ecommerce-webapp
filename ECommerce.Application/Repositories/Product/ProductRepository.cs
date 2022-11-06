using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Product
{
    public class ProductRepository : RepositoryBase<Data.Models.Product>, IProductRepository
    {
        public ProductRepository(ECommerceContext DbContext) : base(DbContext)
        {

        }
    }
}
