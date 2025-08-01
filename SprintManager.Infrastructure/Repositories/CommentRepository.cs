using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Infrastructure.Data;

namespace SprintManager.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.WorkItem)
                .Include(c => c.User)
                .OrderBy(c => c.WorkItem)
                .ThenByDescending(c => c.CreationDate)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.WorkItem)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}