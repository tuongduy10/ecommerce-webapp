using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Bank
{
    public class BankRepository : RepositoryBase<Data.Entities.Bank>, IBankRepository
    {
        public BankRepository(ECommerceContext DbContext) : base(DbContext)
        {

        }
    }
}
