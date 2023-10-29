using ECommerce.Application.BaseServices.FilterProduct.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.FilterProduct
{
    public interface IFilterProductService
    {
        Task<List<FilterModel>> listFilterModel(int brandId);
    }
}
