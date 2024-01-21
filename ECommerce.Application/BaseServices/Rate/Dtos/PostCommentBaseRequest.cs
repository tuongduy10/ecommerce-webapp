using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Rate.Dtos
{
    public class PostCommentBaseRequest
    {
        public int productId { get; set; }
        public int userId { get; set; }
        public string comment { get; set; }
        public int value { get; set; }
        public List<IFormFile> files { get; set; }
        public List<string> fileNames { get; set; }
    }
}
