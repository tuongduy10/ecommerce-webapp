using ECommerce.Application.BaseServices.FilterProduct.Dtos;
using ECommerce.Application.Common;
using ECommerce.Application.Services.Inventory.Dtos;
using ECommerce.Application.Services.User.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Inventory
{
    public interface IInventoryService
    {
        Task<Response<BrandModel>> getBrand(int brandId);
        Task<Response<List<BrandModel>>> getBrands(BrandGetRequest request);
        Task<Response<List<SubCategoryModel>>> getSubCategories(InventoryRequest request);
        Task<Response<List<OptionModel>>> getProductOptions(InventoryRequest request);
        Task<Response<List<AttributeModel>>> getProductAttributes(InventoryRequest request);
        Task<Response<List<SizeGuideModel>>> getProductSizeGuides();
    }
}
