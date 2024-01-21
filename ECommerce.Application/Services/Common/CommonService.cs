using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ECommerce.Application.Services.Common
{
    public class CommonService : ICommonService
    {
        public void AddFiles(IWebHostEnvironment webHostEnviroment, List<IFormFile> files, List<string> filesName, string path)
        {
            if (files != null && filesName != null && !String.IsNullOrEmpty(path))
            {
                string uploadsFolder = Path.Combine(webHostEnviroment.WebRootPath, path);
                for (int i = 0; i < files.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, filesName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        files[i].CopyTo(fileStream);
                    }
                }
            }
        }
        public void DeleteFiles(IWebHostEnvironment webHostEnviroment, List<string> fileNames, string path)
        {
            if (fileNames != null && fileNames.Count > 0 && !String.IsNullOrEmpty(path))
            {
                string uploadsFolder = Path.Combine(webHostEnviroment.WebRootPath, path);
                // remove previous image
                DirectoryInfo uploadDirectory = new DirectoryInfo(uploadsFolder);
                foreach (var fileName in fileNames)
                {
                    foreach (FileInfo file in uploadDirectory.GetFiles())
                    {
                        if (file.Name == fileName)
                        {
                            file.Delete();
                        }
                    }
                }
            }
        }
    }
}
