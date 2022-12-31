using ECommerce.Application.Services.Rate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate.Dtos
{
    public class RateGetModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; }
        public bool Liked { get; set; }
        public bool Disliked { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool CanAction { get; set; }
        public int UserId { get; set; }
        public int UserRepliedId { get; set; }
        public string UserRepliedName { get; set; }
        public string UserName {get;set;}
        public string ShopName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string HtmlPosition { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSeller { get; set; }
        public bool IsShopOwner { get; set; }
        public int ParentId { get; set; }
        public string ParentCreateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public List<ImageModel> Images { get; set; }
        public List<RateGetModel> Replies { get; set; }
        public List<string> UsersLike { get; set; }
        public List<string> UsersDislike { get; set; }
    }
}
