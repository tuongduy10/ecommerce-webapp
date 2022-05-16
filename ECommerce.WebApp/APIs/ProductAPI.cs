using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.FilterProduct;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPI : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IBrandService _brandService;
        private readonly IFilterProductService _filterService;
        public ProductAPI(IProductService productService, ISubCategoryService subCategoryService, IBrandService brandService, IFilterProductService filterService)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
            _filterService = filterService;
        }
        [HttpPost("getProductInBrand")]
        public async Task<IActionResult> getProductInBrand([FromBody] ProductGetRequest request)
        {
            var products = await _productService.getProductPaginated(request);
            var listSubCategory = await _subCategoryService.getSubCategoryInBrand(request.BrandId);
            var brand = await _brandService.getBrandById(request.BrandId);
            var filter = await _filterService.listFilterModel(request.BrandId);

            var list = new List<ProductModel>();
            foreach (var product in products.Items)
            {
                ProductModel pro = new ProductModel();
                pro.ProductImages = product.ProductImages;
                pro.DiscountPercent = product.DiscountPercent;
                pro.New = product.New;
                pro.Highlights = product.Highlights;
                pro.ShopName = product.ShopName;
                pro.ProductName = product.ProductName;
                pro.BrandName = product.BrandName;
                pro.ProductImportDate = product.ProductImportDate;

                if (product.Type.Count == 1)
                {
                    pro.ProductTypeName = product.Type[0].ProductTypeName;
                }
                if (product.Type.Count == 2)
                {
                    foreach (var type in product.Type)
                    {
                        if (type.ProductTypeId == 1)
                        {
                            pro.ProductTypeName = type.ProductTypeName;
                        }
                    }
                }

                // price
                if (product.Price.Count == 2)
                {
                    foreach (var price in product.Price)
                    {
                        if (price.ProductTypeId == 2)
                        {
                            if (price.PriceOnSell == null)
                            {
                                pro.Price = price.Price;
                                pro.PriceOnSell = null;
                            }
                            else
                            {
                                pro.Price = price.Price;
                                pro.PriceOnSell = price.PriceOnSell;
                            }
                        }
                    }
                }
                if (product.Price.Count == 1)
                {
                    if (product.Price[0].PriceOnSell == null)
                    {
                        pro.Price = product.Price[0].Price;
                        pro.PriceOnSell = null;
                    }
                    else
                    {
                        pro.Price = product.Price[0].Price;
                        pro.PriceOnSell = product.Price[0].PriceOnSell;
                    }
                }

                list.Add(pro);
            }

            if (request.OrderBy == "Descending")
            {
                list = list.OrderByDescending(i => i.Price).ToList();
            }
            if (request.OrderBy == "Ascending")
            {
                list = list.OrderBy(i => i.Price).ToList();
            }

            ProductRecordModel listProduct = new ProductRecordModel();
            listProduct.CurrentPage = products.CurrentPage;
            listProduct.CurrentRecord = products.CurrentRecord;
            listProduct.TotalPage = products.TotalPage;
            listProduct.TotalRecord = products.TotalRecord;
            listProduct.Items = list;

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory,
                brand = brand,
                listFilterModel = filter,
            };

            return Ok(new { status = "success", data = model });
        }
    }
}
