using ECommerce.Application.Services.ShopBank.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ShopBank
{
    public interface IShopBankService
    {
        Task<int> Create(ShopBankCreateRequest request);
        Task<int> Update(ShopBankModel request);
        Task<int> Delete(int ShopBankId);
        Task<List<ShopBankModel>> getAll();
    }
}
