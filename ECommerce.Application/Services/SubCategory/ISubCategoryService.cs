using ECommerce.Application.Services.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.SubCategory
{
    public interface ISubCategoryService
    {
        Task<int> Create(SubCategoryCreateRequest request);
        Task<int> Update(SubCategoryCreateRequest request);
        Task<int> Delete(int CategoryId);
        Task<List<SubCategoryModel>> getAll();
        Task<List<SubCategoryModel>> getSubCategoryInBrand(int BrandId);
        Task<List<SubCategoryModel>> getSubCategoryByUser(int userId);
        Task<List<SubCategoryModel>> getSubCategoryByCategoryId(int id);
        Task<List<OptionGetModel>> getOptionBySubCategoryId(int id);
        Task<List<AttributeGetModel>> getAttributeBySubCategoryId(int id);
        Task<List<string>> getOptionValueNameByOptionId(int id);
        Task<List<OptionValueGetModel>> getOptionValueByOptionId(int id);
    }
}
