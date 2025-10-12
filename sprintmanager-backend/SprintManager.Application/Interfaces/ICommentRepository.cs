using SprintManager.Domain.Entities;

namespace SprintManager.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(Guid id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment? comment);
        Task DeleteAsync(Comment comment);
    }
}