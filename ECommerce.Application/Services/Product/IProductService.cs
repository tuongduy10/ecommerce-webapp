using ECommerce.Application.Dtos;
using ECommerce.Application.Services.Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product
{
    public interface IProductService
    {
        Task<PageResult<ProductInBrandModel>> getProductPaginated(ProductGetRequest request);
        Task<PageResult<ProductInBrandModel>> getProductByOptionValuePaginated(ProductGetRequest request);
    }
}
