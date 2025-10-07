using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        public LocalFileStorageService(IConfiguration configuration)
        {
            _storagePath = configuration.GetValue<string>("ImageSettings:StoragePath")!;
        }

        public string GetFilePath(string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var directoryPath = Path.Combine(_storagePath, folder);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = GetFilePath(directoryPath, $"{file.FileName}");
            
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folder, $"{file.FileName}");
        }

        public void DeleteFile(string folder, string fileNameWithExtension)
        {
            if (string.IsNullOrEmpty(fileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(fileNameWithExtension));
            }

            var directoryPath = Path.Combine(_storagePath, folder, fileNameWithExtension);

            File.Delete(directoryPath);
        }
    }
}