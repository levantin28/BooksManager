using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BM.Common.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string rootPath = "Uploads";
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var directoryPath = Path.Combine(rootPath);
            var filePath = Path.Combine(directoryPath, fileName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
