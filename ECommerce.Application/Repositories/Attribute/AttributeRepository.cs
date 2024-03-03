using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Attribute
{
    public class AttributeRepository : RepositoryBase<Data.Entities.Attribute>, IAttributeRepository
    {
        public AttributeRepository(ECommerceContext DbContext) : base(DbContext)
        {
            
        }
    }
}
