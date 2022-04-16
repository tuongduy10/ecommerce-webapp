using ECommerce.Application.Services.ProductUserImage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ProductUserImage
{
    public interface IProductUserImage
    {
        Task<int> Create(ProductUserImageCreateRequest request);
        Task<int> Update(ProductUserImageModel request);
        Task<int> Delete(int ProductUserImageId);
        Task<List<ProductUserImageModel>> getAll();
    }
}
