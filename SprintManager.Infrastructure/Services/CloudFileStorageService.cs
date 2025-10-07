using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class CloudinaryFileStorageService : IFileStorageService
    {
        private readonly IConfiguration _config;

        public CloudinaryFileStorageService(IConfiguration config) 
        { 
            _config = config;
        }

        public string GetFilePath(string subfolder, string fileName)
        {
            return Path.Combine(subfolder, fileName);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subfolder)
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

            return uploadResult.PublicId;
        }

        public void DeleteFile(string subfolder, string fileNameWithExtension)
        {
            throw new NotImplementedException();
        }
    }
}