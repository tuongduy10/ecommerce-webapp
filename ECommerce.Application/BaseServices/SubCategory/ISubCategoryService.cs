using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.SubCategory
{
    public interface ISubCategoryService
    {
        Task<ApiResponse> Create(SubCategoryCreateRequest request);
        Task<ApiResponse> Update(SubCategoryUpdateRequest request);
        Task<ApiResponse> Delete(int CategoryId);
        Task<List<SubCategoryModel>> getAll();
        Task<List<SubCategoryModel>> getSubCategoryInBrand(int BrandId);
        Task<List<SubCategoryModel>> getSubCategoryByUser(int userId);
        Task<List<SubCategoryModel>> getSubCategoryByCategoryId(int id);
        Task<List<OptionGetModel>> getOptionBySubCategoryId(int id);
        Task<List<AttributeGetModel>> getAttributeBySubCategoryId(int id);
        Task<List<string>> getOptionValueNameByOptionId(int id);
        Task<List<OptionValueGetModel>> getBaseOptionValueByOptionId(int id);
        Task<List<OptionValueGetModel>> getOptionValueByOptionId(int id);
        Task<List<SubCategoryModel>> getSubCategoryByCategoryWithOptsAndAttrs(int id);
        Task<ApiResponse> UpdateOptionForSub(SubListUpdateRequest request);
        Task<ApiResponse> UpdateAttributeForSub(SubListUpdateRequest request);
        Task<List<OptionGetModel>> getAllOptions();
        Task<ApiResponse> AddOptionBaseValue(OptionBaseValueAddRequest request);
        Task<ApiResponse> UpdateOptionBaseValue(OptionBaseValueUpdateRequest request);
        Task<ApiResponse> DeleteOption(int id);
    }
}
