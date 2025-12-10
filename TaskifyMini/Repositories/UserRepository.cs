using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskifyMini.Data;
using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;
using TaskifyMini.Repositories.Interfaces;

namespace TaskifyMini.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskifyContext _context;

        public UserRepository(TaskifyContext context)
        {
            _context = context;
        }

        public async Task<User?> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string Email)
        {
            return await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.Email == Email && !u.IsDeleted && u.IsActive);
        }

        public async Task<User?> GetUserByIdAsync(int UserId)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == UserId && !u.IsDeleted && u.IsActive);
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
