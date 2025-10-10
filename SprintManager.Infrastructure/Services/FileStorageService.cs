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
        private readonly Cloudinary _cloudinary;

        public FileStorageService(IConfiguration config) 
        { 
            _config = config;

            Account account = new Account(
                _config["CloudinarySettings:CloudName"],
                _config["CloudinarySettings:ApiKey"],
                _config["CloudinarySettings:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder, string publicId)
        {
            var uploadParams = new ImageUploadParams()
            {
                PublicId = publicId,
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.PublicId;
        }

        public void DeleteFile(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            _cloudinary.Destroy(deletionParams);
        }
    }
}