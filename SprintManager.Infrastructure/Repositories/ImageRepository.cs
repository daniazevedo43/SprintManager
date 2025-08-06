using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using SprintManager.Infrastructure.Data;

namespace SprintManager.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Image image)
        {
            var workItem = await _context.WorkItems.FindAsync(image.WorkItemId);
            var user = await _context.WorkItems.FindAsync(image.UserId);
            
            if (!string.IsNullOrWhiteSpace(image.WorkItemId.ToString()) && workItem == null) 
                throw new SprintManagerNotFoundException($"Work item with ID {image.WorkItemId} not found.");
            
            if (!string.IsNullOrWhiteSpace(image.UserId.ToString()) && user == null) 
                throw new SprintManagerNotFoundException($"User with ID {image.UserId} not found.");
           
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }
    }
}