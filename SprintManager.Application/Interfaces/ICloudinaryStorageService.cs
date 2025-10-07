using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace SprintManager.Application.Interfaces
{
    public interface ICloudinaryStorageService
    {
        ImageUploadResult Upload(IFormFile file);
    }
}