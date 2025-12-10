using Microsoft.EntityFrameworkCore;
using TaskifyMini.Models.Entities;

namespace TaskifyMini.Data
{
    public class TaskifyContext : DbContext
    {
        public TaskifyContext(DbContextOptions<TaskifyContext> options) : base(options)
        {
        }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskifyContext).Assembly);
        }
    }
}
