using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class ReviewModel
    {
        public int avgValue { get; set; }
        public int totalRating { get; set; }
        public List<RateModel> rates { get; set; }
    }
    public class RateModel
    {
        public int id { get; set; }
        public int? value { get; set; }
        public string comment { get; set; }
        public int? productId { get; set; }
        public DateTime? createDate { get; set; }
        public int? repliedId { get; set; }
        public int? parentId { get; set; }
        public string htmlPosition { get; set; }
        public int? userId { get; set; }
        public int? userRepliedId { get; set; }
        public string idsToDelete { get; set; }
    }
}
