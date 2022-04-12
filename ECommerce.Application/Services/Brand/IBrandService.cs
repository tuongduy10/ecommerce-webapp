using ECommerce.Application.Services.Brand.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Brand
{
    public interface IBrandService
    {
        Task<int> Create(BrandCreateRequest request);
        Task<int> Update(BrandCreateRequest request);
        Task<int> Delete(int BrandId);
        Task<List<BrandModel>> getAll();
    }
}
