using ECommerce.Application.Services.Discount.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Discount
{
    public interface IDiscountService
    {
        Task<DiscountGetModel> getDisount(int shopId, int brandId);
        Task<DiscountValue> getDiscountValue(string code);
    }
}
