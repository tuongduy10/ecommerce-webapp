using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECommerce.WebApp.Dtos.Common
{
    public class RemoveUploadRequest
    {
        public List<string> fileNames { get; set; }
        public string uploadType { get; set; }
        public List<int?> ids { get; set; }
    }
    public class UploadTypeConstant
    {
        public const string PRODUCT = "products";
        public const string BRAND = "brand";
        public const string RATING = "rates";
    }
}
