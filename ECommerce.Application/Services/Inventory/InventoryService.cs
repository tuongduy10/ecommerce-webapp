using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services.Inventory.Dtos;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly ECommerceContext _DbContext;
        private readonly IRepositoryBase<Data.Models.Product> _productRepo;
        private readonly IRepositoryBase<Data.Models.BrandCategory> _brandCategoryRepo;
        private readonly IRepositoryBase<Data.Models.SubCategory> _subCategoryRepo;
        public InventoryService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if (_productRepo == null)
                _productRepo = new RepositoryBase<Data.Models.Product>(_DbContext);
            if (_brandCategoryRepo == null)
                _brandCategoryRepo = new RepositoryBase<Data.Models.BrandCategory>(_DbContext);
            if (_subCategoryRepo == null)
                _subCategoryRepo = new RepositoryBase<Data.Models.SubCategory>(_DbContext);
        }
        public async Task<Response<List<SubCategoryModel>>> subCategoryList(int brandId)
        {
            try
            {
                return new SuccessResponse<List<SubCategoryModel>>("");
            }
            catch (Exception error)
            {
                return new FailResponse<List<SubCategoryModel>>(error.ToString());
            }
        }
    }
}
