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
        Task<List<ProductInBrandModel>> getProductsInBrand(int BrandId, int pageindex, int pagesize);
        Task<PageResult<ProductInBrandModel>> getProductPaginated(int BrandId, int pageindex, int pagesize);
        Task<PageResult<ProductInBrandModel>> getProductBySubCategoryPaginated(int BrandId, int SubCategoryId, int pageindex, int pagesize);
    }
}
