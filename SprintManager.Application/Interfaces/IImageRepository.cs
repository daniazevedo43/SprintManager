using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IImageRepository
    {
        Task<List<Image>> GetAllAsync();
        Task AddAsync(Image image);
    }
}