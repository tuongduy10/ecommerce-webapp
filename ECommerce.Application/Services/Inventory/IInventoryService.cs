using ECommerce.Application.BaseServices.FilterProduct.Dtos;
using ECommerce.Application.Common;
using ECommerce.Application.Services.Inventory.Dtos;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Data.Entities;
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
        Task<Response<List<CategoryModel>>> getCategories();
        Task<Response<CategoryModel>> getCategory(int id);
        Task<Response<Category>> updateCategory(CategoryModelRequest req);
        Task<Response<Category>> addCategory(CategoryModelRequest req);
        Task<Response<List<SubCategoryModel>>> getSubCategories(InventoryRequest request);
        Task<Response<SubCategory>> updateSubCategory(SubCategoryModel request);
        Task<Response<SubCategory>> addSubCategory(SubCategoryModel request);
        Task<Response<List<OptionModel>>> getOptions(InventoryRequest request);
        Task<Response<List<OptionModel>>> getProductOptions(InventoryRequest request);
        Task<Response<List<AttributeModel>>> getAttributes(InventoryRequest request);
        Task<Response<bool>> saveAttributes(InventoryRequest request);
        Task<Response<List<AttributeModel>>> getProductAttributes(InventoryRequest request);
        Task<Response<List<SizeGuideModel>>> getProductSizeGuides();
    }
}
