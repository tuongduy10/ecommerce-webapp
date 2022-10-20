using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate.Dtos
{
    public class ReplyCommentRequest
    {
        public int productId { get; set; }
        public int userId { get; set; }
        public int userRepliedId { get; set; }
        public int repliedId { get; set; }
        public int parentId { get; set; }
        public string comment { get; set; }
        public int level { get; set; }
        public List<IFormFile> files { get; set; }
        public List<string> fileNames { get; set; }
    }
}
