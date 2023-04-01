using ECommerce.Application.Repositories;
using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Configurations;
using ECommerce.Application.Services.Discount;
using ECommerce.Application.Services.FilterProduct;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Product.Enum;
using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.Products;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IDiscountService _discountService;
        private readonly ICommentService _commentService;
        public ProductController(
            IConfigurationService configurationService,
            IProductService productService, 
            ISubCategoryService subCategoryService, 
            IBrandService brandService, 
            IFilterProductService filterService,
            IRateService rateService,
            IDiscountService discountService,
            ICommentService commentService
        ) {
            _configurationService = configurationService;
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
            _filterService = filterService;
            _rateService = rateService;
            _discountService = discountService;
            _commentService = commentService;
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
            listProduct.Items = ProductListFormated(products.Items);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory,
                brand = brand,
                listFilterModel = filter,
            };

            return View(model);
        }
        public async Task<IActionResult> ProductDetail(int ProductId, bool isScrolledTo = false, int commentId = 0)
        {
            var _userId = User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ?
                Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;

            var product = await _productService.getProductDetail(ProductId);
            if (product == null)
                return NotFound();
            //var rates = await _rateService.GetRatesByProductId(ProductId, _userId);
            var rates = await _commentService.GetAllByProductId(ProductId, _userId);
            var suggestion = await _productService.getProductSuggestion();
            var phone = await _configurationService.getPhoneNumber();
            var options = await _productService.getProductOption(ProductId);
            var discount = await _discountService.getDisount(product.ShopId, product.Brand.BrandId);
            var sizeGuides = await _productService.SizeGuideList();

            var model = new ProductDetailViewModel
            {
                product = product,
                rates = rates.Data,
                suggestion = ProductListFormated(suggestion),
                phone = phone,
                options = options,
                discount = discount,
                sizeGuides = sizeGuides
            };

            ViewBag.isScrolledTo = isScrolledTo;
            ViewBag.CommentId = commentId;

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
        public async Task<IActionResult> ProductHotSale()
        {
            var products = await _productService.getProductInPagePaginated(new ProductGetRequest() { GetBy = "Discount", PageIndex = 1 });
            ProductRecordModel listProduct = new ProductRecordModel();
            listProduct.CurrentPage = products.CurrentPage;
            listProduct.CurrentRecord = products.CurrentRecord;
            listProduct.TotalPage = products.TotalPage;
            listProduct.TotalRecord = products.TotalRecord;
            listProduct.Items = ProductListFormated(products.Items);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
            };

            return View(model);
        }
        public async Task<IActionResult> ProductNewest()
        {
            var products = await _productService.getProductInPagePaginated(new ProductGetRequest() { GetBy = "Newest", PageIndex = 1 });
            ProductRecordModel listProduct = new ProductRecordModel();
            listProduct.CurrentPage = products.CurrentPage;
            listProduct.CurrentRecord = products.CurrentRecord;
            listProduct.TotalPage = products.TotalPage;
            listProduct.TotalRecord = products.TotalRecord;
            listProduct.Items = ProductListFormated(products.Items);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
            };

            return View(model);
        }
        
        private List<ProductModel> ProductListFormated(List<ProductInBrandModel> list)
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
                pro.Status = item.Status;

                foreach (var price in item.Price)
                {
                    // Hàng đặt trước
                    if (price.ProductTypeId == (int)enumProductType.PreOrder && price.Price != null)
                    {
                        pro.ProductTypeName = ProductTypeConst.PreOrderName;
                        pro.Price = price.Price;
                        pro.PriceOnSell = price.PriceOnSell == null ? null : price.PriceOnSell;
                    }
                    // Hàng có sẵn
                    else
                    {
                        if (price.ProductTypeId == (int)enumProductType.Available && price.Price != null)
                        {
                            pro.ProductTypeName = ProductTypeConst.AvailableName;
                            pro.Price = price.Price;
                            pro.PriceOnSell = price.PriceOnSell == null ? null : price.PriceOnSell;
                        }
                    } 
                }

                //if (item.Type.Count == 1)
                //{
                //    pro.ProductTypeName = item.Type[0].ProductTypeName;
                //}
                //if (item.Type.Count == 2)
                //{
                //    foreach (var type in item.Type)
                //    {
                //        if (type.ProductTypeId == 1)
                //        {
                //            pro.ProductTypeName = type.ProductTypeName;
                //        }
                //    }
                //}

                //// price
                //if (item.Price.Count == 2)
                //{
                //    foreach (var price in item.Price)
                //    {
                //        if (price.ProductTypeId == 2)
                //        {
                //            if (price.PriceOnSell == null)
                //            {
                //                pro.Price = price.Price;
                //                pro.PriceOnSell = null;
                //            }
                //            else
                //            {
                //                pro.Price = price.Price;
                //                pro.PriceOnSell = price.PriceOnSell;
                //            }
                //        }
                //    }
                //}
                //if (item.Price.Count == 1)
                //{
                //    if (item.Price[0].PriceOnSell == null)
                //    {
                //        pro.Price = item.Price[0].Price;
                //        pro.PriceOnSell = null;
                //    }
                //    else
                //    {
                //        pro.Price = item.Price[0].Price;
                //        pro.PriceOnSell = item.Price[0].PriceOnSell;
                //    }
                //}

                _list.Add(pro);
            }
            return _list;
        }
    }
}
