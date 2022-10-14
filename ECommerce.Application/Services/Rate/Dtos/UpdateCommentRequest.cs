using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate.Dtos
{
    public class UpdateCommentRequest
    {
        public int id { get; set; }
        public string content { get; set; }
        public int rateValue { get; set; }
        public List<IFormFile> files { get; set; }
        public List<string> fileNames { get; set; }
    }
}
