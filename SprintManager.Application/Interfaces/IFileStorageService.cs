using Microsoft.AspNetCore.Http;

namespace SprintManager.Application.Interfaces
{
    public interface IFileStorageService
    {
        string GetFilePath(string folder, string fileName);
        Task<string> SaveFileAsync(IFormFile file, string folder);
        void DeleteFile(string folder, string fileNameWithExtension);
    }
}