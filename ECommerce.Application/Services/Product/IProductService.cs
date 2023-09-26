using ECommerce.Application.Common;
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
        Task<Response<PageResult<ProductModel>>> getProductList(ProductGetRequest request);
        Task<Response<List<ProductModel>>> getManagedProductList(ProductGetRequest request);
        Task<Response<bool>> save(ProductModel request); // add or update
        Task<Response<bool>> delete(List<int> ids);
    }
}
