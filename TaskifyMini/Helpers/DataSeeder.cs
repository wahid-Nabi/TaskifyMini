using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskifyMini.Data;
using TaskifyMini.Models.Entities;
using TaskifyMini.Models.Enums;

namespace TaskifyMini.Helpers
{
    public class DataSeeder
    {
        private readonly TaskifyContext _context;
        private readonly PasswordHasher<User> _hasher = new();
        public DataSeeder(TaskifyContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {

            // Seed roles
            if (!await _context.Roles.AnyAsync())
            {
                _context.Roles.AddRange(
                    new Roles {  RoleName = UserRoles.Admin.ToString() },
                    new Roles {  RoleName = UserRoles.User.ToString() }
                );
            }

            // Seed admin user
            if (!await _context.Users.AnyAsync(u => u.UserName == UserRoles.Admin.ToString()))
            {
                var admin = new User
                {
                    UserName = "Admin",
                    Email = "admin@yopmail.com",
                    RoleId = 1,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                admin.PasswordHash = _hasher.HashPassword(admin, "Admin@123");
                _context.Users.Add(admin);
            }
            await _context.SaveChangesAsync();
        }

    }
}
