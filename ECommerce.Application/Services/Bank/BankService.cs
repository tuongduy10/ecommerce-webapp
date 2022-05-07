using ECommerce.Application.Common;
using ECommerce.Application.Services.Bank.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Bank
{
    public class BankService : IBankService
    {
        private readonly ECommerceContext _DbContext;

        public BankService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<ApiResponse> deleteBank(int bankId)
        {
            var bank = await _DbContext.Banks.Where(i => i.BankId == bankId).FirstOrDefaultAsync();
            if (bank != null)
            {
                _DbContext.Banks.Remove(bank);
                _DbContext.SaveChangesAsync().Wait();
                return new ApiSuccessResponse("Xóa ngân hàng thành công!");
            }
            return new ApiFailResponse("Xóa ngân hàng thất bại!");
        }

        public async Task<List<BankGetModel>> getListBank()
        {
            var listBank = await _DbContext.Banks.Select(i=> new BankGetModel()
            {
                BankId = i.BankId,
                BankAccountNumber = i.BankAccountNumber,
                BankAccountName = i.BankAccountName,
                BankImage = i.BankImage,
                BankName = i.BankName
            }).ToListAsync();
            return listBank;
        }
    }
}
