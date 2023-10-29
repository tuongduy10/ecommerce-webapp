using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.Bank.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Bank
{
    public interface IBankService
    {
        Task<List<BankGetModel>> getListBank();
        Task<ApiResponse> deleteBank(int bankId);
        Task<ApiResponse> updateBank(BankUpdateRequest request);
        Task<ApiResponse> addBank(BankAddRequest request);
    }
}
