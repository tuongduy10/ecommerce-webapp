using ECommerce.Application.BaseServices.ProductUserImage.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.ProductUserImage
{
    public class ProductUserImage : IProductUserImage
    {
        public async Task<int> Create(ProductUserImageCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int ProductUserImageId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductUserImageModel>> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductUserImageModel request)
        {
            throw new NotImplementedException();
        }
    }
}
