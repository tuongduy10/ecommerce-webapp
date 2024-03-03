using ECommerce.Application.BaseServices.Rate.Dtos;
using ECommerce.Application.BaseServices.Rate.Models;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Interest
{
    public class InterestRepository : RepositoryBase<Data.Entities.Interest>, IInterestRepository
    {
        public InterestRepository(ECommerceContext DbContext): base(DbContext) { }

        public async Task<LikeAndDislike> LikeAndDislikeCount(int rateId)
        {
            var interestObj = await ToListAsyncWhere(item => item.RateId == rateId);
            var result = new LikeAndDislike
            {
                RateId = rateId,
                LikeCount = interestObj == null ? 0 : interestObj.Where(item => item.Liked == true).Count(),
                DislikeCount = interestObj == null ? 0 : interestObj.Where(item => item.Liked == false).Count()
            };
            return result;
        }
        public async Task<LikeAndDislike> LikeComment(LikeAndDislikeCount request)
        {
            var currObj = await GetAsyncWhere(i => i.UserId == request.userId && i.RateId == request.rateId);
            int rateId = 0;
            if (currObj == null)
            {
                var _obj = new Data.Entities.Interest
                {
                    RateId = request.rateId,
                    UserId = request.userId,
                    Liked = request.liked
                };
                await AddAsync(_obj);
                rateId = _obj.RateId;
            }
            else
            {
                if (currObj.Liked == request.liked) currObj.Liked = null;
                else currObj.Liked = request.liked;

                rateId = currObj.RateId;
            }
            await SaveChangesAsync();
            var interest = await LikeAndDislikeCount(rateId);
            return interest;
        }
    }
}
