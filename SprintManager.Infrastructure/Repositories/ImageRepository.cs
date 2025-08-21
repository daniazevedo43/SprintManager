using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
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

        public async Task<List<Image>> GetAllAsync()
        {
            return await _context.Images
                .Include(i => i.WorkItem)
                .Include(i => i.User)
                .OrderBy(i => i.WorkItem)
                .ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(Guid id)
        {
            return await _context.Images
                .Include(c => c.WorkItem)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Image image)
        {
            var images = await _context.Images
                .Where(i => i.FileName.Contains(image.FileName))
                .Select(i => i.FileName)
                .ToListAsync();

            if (images.Count > 0)
            {
                throw new SprintManagerConflictException($"An image with the name {image.FileName} already exists.");
            }

            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Image image)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}