using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Dtos.Common
{
    public class UploadRequest
    {
        public List<IFormFile> files { get; set; }
        public string uploadType { get; set; }
    }
}
