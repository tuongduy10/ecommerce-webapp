using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Common
{
    public interface ICommonService
    {
        void AddFiles(IWebHostEnvironment webHostEnviroment, List<IFormFile> files, List<string> filesName, string path);
        void DeleteFiles(IWebHostEnvironment webHostEnviroment, List<string> fileNames, string path);
    }
}
