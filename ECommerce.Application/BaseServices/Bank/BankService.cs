using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.Bank.Dtos;
using ECommerce.Data.Models;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Bank
{
    public class BankService : IBankService
    {
        private readonly ECommerceContext _DbContext;

        public BankService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<ApiResponse> addBank(BankAddRequest request)
        {
            var BankName = request.BankName;
            var BankAccountName = request.BankAccountName;
            var BankAccountNumber = request.BankAccountNumber;
            var BankImage = request.BankImage;

            if (BankName == null) return new ApiFailResponse("Tên ngân hàng không được để trống");
            if (BankAccountName == null) return new ApiFailResponse("Tên tài khoản không được để trống");
            if (BankAccountNumber == null) return new ApiFailResponse("Số tài khoản không được để trống");

            Data.Models.Bank bank = new Data.Models.Bank();
            bank.BankAccountName = BankAccountName;
            bank.BankAccountNumber = BankAccountNumber;
            bank.BankImage = BankImage;
            bank.BankName = BankName;

            await _DbContext.Banks.AddAsync(bank);
            await _DbContext.SaveChangesAsync();
            return new ApiSuccessResponse("Thêm thành công", bank.BankId);
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

        public Task<ApiResponse> updateBank(BankUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
