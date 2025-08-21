using Microsoft.AspNetCore.Http;

namespace SprintManager.Application.Interfaces
{
    public interface IFileStorageService
    {
        string GetFilePath(string subfolder, string fileName);
        Task<string> SaveFileAsync(IFormFile file, string subfolder);
        void DeleteFile(string subfolder, string fileNameWithExtension);
    }
}