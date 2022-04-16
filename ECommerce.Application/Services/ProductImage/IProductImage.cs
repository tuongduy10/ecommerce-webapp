using ECommerce.Application.Services.ProductImage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ProductImage
{
    public interface IProductImage
    {
        Task<int> Create(ProductImageCreateRequest request);
        Task<int> Update(ProductImageModel request);
        Task<int> Delete(int ProductImageId);
        Task<List<ProductImageModel>> getAll();
    }
}
