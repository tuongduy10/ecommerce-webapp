using ECommerce.Application.Common;
using ECommerce.Application.Services.Category.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Category
{
    public interface ICategoryService
    {
        Task<ApiResponse> Create(CategoryCreateRequest request);
        Task<ApiResponse> Update(CategoryModel request);
        Task<ApiResponse> Delete(int CategoryId);
        Task<List<CategoryModel>> getAll();
        Task<CategoryModel> Detail(int id);
    }
}
