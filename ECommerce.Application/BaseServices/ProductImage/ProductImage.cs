using ECommerce.Application.BaseServices.ProductImage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.ProductImage
{
    public class ProductImage : IProductImage
    {
        public async Task<int> Create(ProductImageCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int ProductImageId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductImageModel>> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductImageModel request)
        {
            throw new NotImplementedException();
        }
    }
}
