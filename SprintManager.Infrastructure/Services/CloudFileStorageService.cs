using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class CloudFileStorageService : IFileStorageService
    {
        private readonly IConfiguration _config;

        public CloudFileStorageService(IConfiguration config) 
        { 
            _config = config;
        }

        public string GetFilePath(string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            Account account = new Account(
                _config["Cloudinary:CloudName"],
                _config["Cloudinary:ApiKey"],
                _config["Cloudinary:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.PublicId;
        }

        public void DeleteFile(string subfolder, string fileNameWithExtension)
        {
            throw new NotImplementedException();
        }
    }
}