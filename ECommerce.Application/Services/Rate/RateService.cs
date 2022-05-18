
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate
{
    public class RateService : IRateService
    {
        private ECommerceContext _DbContext;
        public RateService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<List<RateGetModel>> getRatesByProductId(int id)
        {
            var rate = await _DbContext.Rates
                .Where(rate => rate.ProductId == id)
                .Select(rate => new RateGetModel(){
                    Value = (int)rate.RateValue,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = rate.CreateDate,
                    Images = _DbContext.RatingImages.Where(img => img.RateId == rate.RateId).Select(i=>i.RatingImagePath).ToList()
                })
                .ToListAsync();

            return rate;
        }
    }
}
