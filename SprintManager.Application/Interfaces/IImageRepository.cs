using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface IImageRepository
    {
        Task AddAsync(Image image);
    }
}