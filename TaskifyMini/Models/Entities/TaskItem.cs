using System.ComponentModel.DataAnnotations;
using TaskifyMini.Models.Enums;
using TaskStatus = TaskifyMini.Models.Enums.TaskStatus;

namespace TaskifyMini.Models.Entities
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; } 
        public TaskStatus Status { get; set; } = TaskStatus.New;
        public TaskPriorities Priority { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
