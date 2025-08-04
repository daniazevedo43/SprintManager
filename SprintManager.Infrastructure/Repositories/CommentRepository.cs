using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
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

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment? comment)
        {
            var workItem = await _context.WorkItems.FindAsync(comment?.WorkItemId);
            var user = await _context.Users.FindAsync(comment?.UserId);

            if (comment != null)
            {
                if (!string.IsNullOrWhiteSpace(comment.WorkItemId.ToString()) && workItem == null) throw new SprintManagerNotFoundException($"Work item with ID {comment?.WorkItemId} not found.");
                if (!string.IsNullOrWhiteSpace(comment.UserId.ToString()) && user == null) throw new SprintManagerNotFoundException($"User with ID {comment?.UserId} not found.");

                _context.Comments.Update(comment);
            }

            await _context.SaveChangesAsync();
        }
    }
}