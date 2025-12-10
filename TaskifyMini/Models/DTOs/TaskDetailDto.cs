using TaskifyMini.Models.Enums;
using TaskStatus = TaskifyMini.Models.Enums.TaskStatus;

namespace TaskifyMini.Models.DTOs
{
    public class TaskDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriorities Priority { get; set; }
    }
}
