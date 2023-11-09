using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Comment.Request
{
    public class PostCommentRequest
    {
        public int productId { get; set; }
        public int userId { get; set; }
        public string comment { get; set; }
        public int value { get; set; }
        public List<string> fileNames { get; set; }
    }
}
