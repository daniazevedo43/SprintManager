using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class CloudinaryStorageService : ICloudinaryStorageService
    {
        private readonly IConfiguration _config;

        public CloudinaryStorageService(IConfiguration config) 
        { 
            _config = config;
        }

        public ImageUploadResult Upload(IFormFile file)
        {
            Account account = new Account(
                _config["Cloudinary:CloudName"],
                _config["Cloudinary:ApiKey"],
                _config["Cloudinary:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult;
        }
    }
}