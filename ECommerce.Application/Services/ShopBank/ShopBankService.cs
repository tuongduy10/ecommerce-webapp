using ECommerce.Application.Services.ShopBank.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ShopBank
{
    public class ShopBankService : IShopBankService
    {
        public async Task<int> Create(ShopBankCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int ShopBankId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShopBankModel>> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ShopBankModel request)
        {
            throw new NotImplementedException();
        }
    }
}
