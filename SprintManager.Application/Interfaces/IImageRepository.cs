using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IImageRepository
    {
        Task<List<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(Guid id);
        Task AddAsync(Image image);
    }
}