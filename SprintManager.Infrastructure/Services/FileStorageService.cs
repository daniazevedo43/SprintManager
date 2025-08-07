using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        public FileStorageService(IConfiguration configuration)
        {
            _storagePath = configuration.GetValue<string>("ImageSettings:StoragePath")!;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            var directoryPath = Path.Combine(_storagePath, subfolder);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, $"{file.FileName}");
            
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(subfolder, $"{file.FileName}");
        }
    }
}