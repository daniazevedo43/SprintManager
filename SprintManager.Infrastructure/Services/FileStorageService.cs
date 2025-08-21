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

        public string GetFilePath(string subfolder, string fileName)
        {
            return Path.Combine(subfolder, fileName);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            var directoryPath = Path.Combine(_storagePath, subfolder);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = GetFilePath(directoryPath, $"{file.FileName}");
            
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(subfolder, $"{file.FileName}");
        }

        public void DeleteFile(string subfolder, string fileNameWithExtension)
        {
            if (string.IsNullOrEmpty(fileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(fileNameWithExtension));
            }

            var directoryPath = Path.Combine(_storagePath, subfolder, fileNameWithExtension);

            if (!File.Exists(directoryPath))
            {
                throw new FileNotFoundException($"Invalid file path");
            }
            File.Delete(directoryPath);
        }
    }
}