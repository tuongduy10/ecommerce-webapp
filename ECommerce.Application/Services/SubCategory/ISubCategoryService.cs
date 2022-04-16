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
    }
}
