using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskifyMini.Models.Entities;

namespace TaskifyMini.Configrations.Entities
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t=>t.Title).IsRequired().HasMaxLength(200);
            builder.Property(t=>t.Description).HasMaxLength(1000);
            builder.Property(t=>t.DueDate).IsRequired(false);
        }
    }
}
