using Microsoft.AspNetCore.Http;

namespace SprintManager.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder, string publicId);
        void DeleteFile(string publicId);
    }
}