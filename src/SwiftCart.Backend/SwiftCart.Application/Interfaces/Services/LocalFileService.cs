using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SwiftCart.Application.Interfaces;

namespace SwiftCart.Infrastructure.Services
{
    public class LocalFileService(IWebHostEnvironment environment) : IFileService
    {
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // LOGIC: Use ContentRootPath to save in the project root ./Uploads folder
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads", folderName);

            // Create directory if missing
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for DB (e.g., "products/abc-123.jpg")
            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }

        public bool DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Uploads", folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
    }
}