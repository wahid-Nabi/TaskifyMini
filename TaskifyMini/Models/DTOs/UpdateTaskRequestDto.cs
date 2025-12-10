using TaskifyMini.Models.Enums;
using TaskStatus = TaskifyMini.Models.Enums.TaskStatus;

namespace TaskifyMini.Models.DTOs
{
    public class UpdateTaskRequestDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
    }
}
