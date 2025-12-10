using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;

namespace TaskifyMini.Repositories.Interfaces
{
    public interface IUserRepository
    {
         Task<User?> GetUserByEmailAsync(string Email);
         Task<User?> GetUserByIdAsync(int UserId);
        Task<User?> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(User user);

    }
}
