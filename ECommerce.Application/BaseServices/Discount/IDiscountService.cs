using ECommerce.Application.BaseServices.Discount.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Discount
{
    public interface IDiscountService
    {
        Task<DiscountGetModel> getDisount(int shopId, int brandId);
        Task<DiscountValue> getDiscountValue(string code);
    }
}
