using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Utils
{
    public class ManageFiles
    {
        private IWebHostEnvironment _webHostEnvironment;
        public ManageFiles(IWebHostEnvironment webHostEnvironment) 
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public void AddFiles(List<IFormFile> files, List<string> filesName, string path)
        {
            if (files != null && filesName != null && !String.IsNullOrEmpty(path)) 
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, path);
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
        public void AddFile(IFormFile file, string fileName, string path)
        {
            if (file != null && !String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(path)) 
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, path);
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
        }
        public void DeleteAllFiles(string path)
        {
            if (!String.IsNullOrEmpty(path)) 
            {
                string pathInfo = Path.Combine(_webHostEnvironment.WebRootPath, path);
                DirectoryInfo directInfo = new DirectoryInfo(pathInfo);
                foreach (FileInfo file in directInfo.GetFiles()) 
                {
                    file.Delete();
                }
            }
        }
        public void DeleteFiles(List<string> fileNames, string path)
        {
            if (fileNames != null && fileNames.Count > 0 && !String.IsNullOrEmpty(path))
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, path);
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
        public void DeleteFile(string fileName, string path)
        {
            if (!String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(path))
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, path);
                // remove previous image
                DirectoryInfo uploadDirectory = new DirectoryInfo(uploadsFolder);
                foreach (FileInfo file in uploadDirectory.GetFiles())
                {
                    if (file.Name == fileName)
                    {
                        file.Delete();
                    }
                }
            }
        }
        public List<string> GetFilesName(List<IFormFile> files, string prefix)
        {
            var listFileName = new List<string>();
            if (files != null && !String.IsNullOrEmpty(prefix)) 
            {
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var extension = new FileInfo(file.FileName).Extension;
                    var fileName = prefix + Guid.NewGuid().ToString() + extension;
                    listFileName.Add(fileName);
                }
            }
            return listFileName;
        }
        public string GetFileName(IFormFile file, string prefix)
        {
            if (file != null && !String.IsNullOrEmpty(prefix)) 
            {
                var extension = new FileInfo(file.FileName).Extension;
                var fileName = prefix + Guid.NewGuid().ToString() + extension;
                return fileName;
            }
            return "";
        }
    }
}
