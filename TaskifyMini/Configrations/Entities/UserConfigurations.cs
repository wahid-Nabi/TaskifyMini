using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskifyMini.Models.Entities;
using TaskifyMini.Models.Enums;

namespace TaskifyMini.Configrations.Entities
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
           
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Property(u => u.PasswordHash).IsRequired();          
            builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(u => u.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(u=>u.Phone).HasMaxLength(15);
            builder.HasOne(u => u.Role)
                   .WithMany(u=>u.Users)
                   .HasForeignKey(u => u.RoleId)
                   .IsRequired();
            //builder.HasData(
            //    new User
            //    {
            //        Id = 1,
            //        UserName = "admin",
            //        Email = "admin@yopmail.com",
            //        PasswordHash = hasher.HashPassword(null,"Admin@123"), // Placeholder for hashed password
            //        CreatedAt = DateTime.UtcNow,
            //        UpdatedAt = DateTime.UtcNow,
            //        IsActive = true,
            //        IsDeleted = false,
            //        RoleId = 1 // Assuming 1 is the Id for Admin role
            //    });
        }
    }
    public class RolesConfigurations : IEntityTypeConfiguration<Roles>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Roles> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.RoleName).IsRequired().HasMaxLength(50);
            builder.HasMany(u=>u.Users)
                   .WithOne(r=>r.Role)
                   .HasForeignKey(u=>u.RoleId).OnDelete(DeleteBehavior.Restrict);

            //builder.HasData(
            //    new Roles
            //    {
            //        Id = 1,
            //        RoleName = UserRoles.Admin.ToString(),
            //    },
            //    new Roles
            //    {
            //        Id = 2,
            //        RoleName = UserRoles.User.ToString(),
            //    });
        }
    }
}
