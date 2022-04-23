using ECommerce.Application.Services.FilterProduct.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.FilterProduct
{
    public class FilterProductService : IFilterProductService
    {
        private readonly ECommerceContext _DbContext;
        public FilterProductService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<List<FilterModel>> listFilterModel(int brandId)
        {
            var query = from subc in _DbContext.SubCategories
                        from brand in _DbContext.Brands
                        where subc.CategoryId == brand.CategoryId && brand.BrandId == brandId
                        select subc;

            var result = await query.Select(i => new FilterModel()
            {
                SubCategoryId = i.SubCategoryId,
                SubCategoryName = i.SubCategoryName,
                CategoryId = i.CategoryId,
                listOption = (
                    from option in _DbContext.Options
                    from subcopt in _DbContext.SubCategories
                    where option.OptionId == subcopt.SubCategoryId && subcopt.SubCategoryId == i.SubCategoryId
                    select option
                ).Select(lo => new Option() { 
                    OptionId = lo.OptionId,
                    OptionName = lo.OptionName,
                    listProductOption = _DbContext.ProductOptions.Where(lop => lop.OptionId == lo.OptionId).Select(lop => new ProductOption() { 
                        OptionId = lop.OptionId,
                        ProductId = lop.ProductId,
                        Value = lop.Value
                    }).ToList()
                }).ToList(),
            }).ToListAsync();

            return null;
        }
    }
}
