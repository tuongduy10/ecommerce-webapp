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

            var list = productListFormated(products.Items);
            if (request.OrderBy == "Descending") list = list.OrderByDescending(i => i.Price).ToList();
            if (request.OrderBy == "Ascending") list = list.OrderBy(i => i.Price).ToList();
            
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
        [HttpPost("getProductInPage")]
        public async Task<IActionResult> getProductInPage([FromBody] ProductGetRequest request)
        {
            var products = await _productService.getProductInPagePaginated(new ProductGetRequest() { OrderBy = request.OrderBy, GetBy = request.GetBy, PageIndex = request.PageIndex });

            var list = productListFormated(products.Items);
            if (request.OrderBy == "Descending") list = list.OrderByDescending(i => i.Price).ToList();
            if (request.OrderBy == "Ascending") list = list.OrderBy(i => i.Price).ToList();

            ProductRecordModel listProduct = new ProductRecordModel();
            listProduct.CurrentPage = products.CurrentPage;
            listProduct.CurrentRecord = products.CurrentRecord;
            listProduct.TotalPage = products.TotalPage;
            listProduct.TotalRecord = products.TotalRecord;
            listProduct.Items = list;

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
            };

            return Ok(new { status = "success", data = model });
        }
        private List<ProductModel> productListFormated(List<ProductInBrandModel> list)
        {
            var _list = new List<ProductModel>();
            foreach (var item in list)
            {
                ProductModel pro = new ProductModel();
                pro.ProductId = item.ProductId;
                pro.ProductImages = item.ProductImages;
                pro.DiscountPercent = item.DiscountPercent;
                pro.New = item.New;
                pro.Highlights = item.Highlights;
                pro.ShopName = item.ShopName;
                pro.ProductName = item.ProductName;
                pro.BrandName = item.BrandName;
                pro.ProductImportDate = item.ProductImportDate;

                if (item.Type.Count == 1)
                {
                    pro.ProductTypeName = item.Type[0].ProductTypeName;
                }
                if (item.Type.Count == 2)
                {
                    foreach (var type in item.Type)
                    {
                        if (type.ProductTypeId == 1)
                        {
                            pro.ProductTypeName = type.ProductTypeName;
                        }
                    }
                }

                // price
                if (item.Price.Count == 2)
                {
                    foreach (var price in item.Price)
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
                if (item.Price.Count == 1)
                {
                    if (item.Price[0].PriceOnSell == null)
                    {
                        pro.Price = item.Price[0].Price;
                        pro.PriceOnSell = null;
                    }
                    else
                    {
                        pro.Price = item.Price[0].Price;
                        pro.PriceOnSell = item.Price[0].PriceOnSell;
                    }
                }

                _list.Add(pro);
            }
            return _list;
        }
    }
}
