using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECommerce.WebApp.Dtos.Common
{
    public class UploadRequest
    {
        public List<IFormFile> files { get; set; }
        public string uploadType { get; set; }
    }
    public class RemoveUploadRequest
    {
        public List<string> fileNames { get; set; }
        public string uploadType { get; set; }
    }
    public class UploadTypeConstant
    {
        public const string PRODUCT = "product";
        public const string BRAND = "brand";
        public const string RATING = "rating";
    }
}
