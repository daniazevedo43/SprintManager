using Microsoft.AspNetCore.Http;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        public async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("images", fileName);
        }
    }
}