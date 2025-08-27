using Microsoft.EntityFrameworkCore;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Infrastructure.Data;

namespace SprintManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.OrderBy(u => u.UserName).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid? id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}