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
        Task<int> Create(CategoryCreateRequest request);
        Task<int> Update(CategoryCreateRequest request);
        Task<int> Delete(int CategoryId);
        Task<List<CategoryModel>> getAll();

    }
}
