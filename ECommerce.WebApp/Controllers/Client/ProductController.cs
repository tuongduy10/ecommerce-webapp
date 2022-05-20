using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Configurations;
using ECommerce.Application.Services.FilterProduct;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    public class ProductController : Controller
    {
        private readonly IConfigurationService _configurationService;
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IBrandService _brandService;
        private readonly IFilterProductService _filterService;
        private readonly IRateService _rateService;
        public ProductController(
            IConfigurationService configurationService,
            IProductService productService, 
            ISubCategoryService subCategoryService, 
            IBrandService brandService, 
            IFilterProductService filterService,
            IRateService rateService)
        {
            _configurationService = configurationService;
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
            _filterService = filterService;
            _rateService = rateService;
        }
        public async Task<IActionResult> ProductInBrand(ProductGetRequest request)
        {
            var products = await _productService.getProductPaginated(request);
            var listSubCategory = await _subCategoryService.getSubCategoryInBrand(request.BrandId);
            var brand = await _brandService.getBrandById(request.BrandId);
            var filter = await _filterService.listFilterModel(request.BrandId);

            ProductRecordModel listProduct = new ProductRecordModel();
            listProduct.CurrentPage = products.CurrentPage;
            listProduct.CurrentRecord = products.CurrentRecord;
            listProduct.TotalPage = products.TotalPage;
            listProduct.TotalRecord = products.TotalRecord;
            listProduct.Items = productListFormated(products.Items);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory,
                brand = brand,
                listFilterModel = filter,
            };

            return View(model);
        }
        public async Task<IActionResult> ProductDetail(int ProductId)
        {
            var product = await _productService.getProductDeatil(ProductId);
            var rates = await _rateService.getRatesByProductId(ProductId);
            var suggestion = await _productService.getProductSuggestion();
            var phone = await _configurationService.getPhoneNumber();
            var options = await _productService.getProductOption(ProductId);

            var model = new ProductDetailViewModel
            {
                product = product,
                rates = rates,
                suggestion = productListFormated(suggestion),
                phone = phone,
                options = options,
            };
            return View(model);
        }
        public async Task<IActionResult> ProductAvaliable()
        {
            return View();
        }
        public async Task<IActionResult> ProductPreOrder()
        {
            return View();
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
