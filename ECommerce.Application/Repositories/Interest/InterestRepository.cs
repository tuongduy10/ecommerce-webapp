using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Application.Services.Rate.Models;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Interest
{
    public class InterestRepository : RepositoryBase<Data.Models.Interest>, IInterestRepository
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
            var currObj = await FindAsyncWhere(i => i.UserId == request.userId && i.RateId == request.rateId);
            int rateId = 0;
            if (currObj == null)
            {
                var _obj = new Data.Models.Interest
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
                currObj.Liked = currObj.Liked == request.liked ? null : request.liked;
                rateId = currObj.RateId;
            }

            var interest = await LikeAndDislikeCount(rateId);
            return interest;
        }
    }
}
