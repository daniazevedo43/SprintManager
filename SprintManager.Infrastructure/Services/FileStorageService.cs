using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _config;

        public FileStorageService(IConfiguration config) 
        { 
            _config = config;
        }

        public string GetFilePath(string folder, string fileName)
        {
            return Path.Combine(folder, fileName).Replace('\\', '/');
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder, string publicId)
        {
            Account account = new Account(
                _config["CloudinarySettings:CloudName"],
                _config["CloudinarySettings:ApiKey"],
                _config["CloudinarySettings:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                PublicId = publicId,
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.PublicId;
        }

        public void DeleteFile(string publicId)
        {
            Account account = new Account(
                _config["CloudinarySettings:CloudName"],
                _config["CloudinarySettings:ApiKey"],
                _config["CloudinarySettings:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            var deletionParams = new DeletionParams(publicId);

            cloudinary.Destroy(deletionParams);
        }
    }
}