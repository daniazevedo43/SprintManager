using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(Guid id);
    }
}