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
        public void AddFile(IFormFile file, string fileName, string path)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, path);
            string filePath = Path.Combine(uploadsFolder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
        public void DeleteFiles()
        {

        }
        public void DeleteFile(string fileName, string path)
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
        public List<string> GetFilesName(List<IFormFile> files, string prefix)
        {
            var listFileName = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var extension = new FileInfo(file.FileName).Extension;
                var fileName = prefix + Guid.NewGuid().ToString() + extension;
                listFileName.Add(fileName);
            }
            return listFileName;
        }
        public string GetFileName(IFormFile file, string prefix)
        {
            var extension = new FileInfo(file.FileName).Extension;
            var fileName = prefix + Guid.NewGuid().ToString() + extension;

            return fileName;
        }
    }
}
