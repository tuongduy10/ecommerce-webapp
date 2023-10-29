using ECommerce.Application.BaseServices.FilterProduct.Dtos;
using ECommerce.Application.Common;
using ECommerce.Application.Services.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Inventory
{
    public interface IInventoryService
    {
        Task<Response<BrandModel>> getBrand(int brandId);
        Task<Response<List<SubCategoryModel>>> getSubCategories(int brandId);
    }
}
