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
        public int UserId { get; set; }
        public int UserRepliedId { get; set; }
        public string UserRepliedName { get; set; }
        public string UserName {get;set;}
        public string ShopName { get; set; }
        public string ProductName { get; set; }
        public string HtmlPosition { get; set; }
        public bool IsAdmin { get; set; }
        public int ParentId { get; set; }
        public DateTime CreateDate { get; set; }
        public List<string> Images { get; set; }
        public List<RateGetModel> Replies { get; set; }
    }
}
