using ECommerce.Application.BaseServices.Rate.Dtos;
using ECommerce.Application.BaseServices.Rate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Interest
{
    public interface IInterestRepository : IRepositoryBase<Data.Entities.Interest>
    {
        Task<LikeAndDislike> LikeAndDislikeCount(int rateId);
        Task<LikeAndDislike> LikeComment(LikeAndDislikeCount request);
    }
}
