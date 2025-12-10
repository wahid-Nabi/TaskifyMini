using System.ComponentModel.DataAnnotations;
using TaskifyMini.Models.Enums;

namespace TaskifyMini.Models.DTOs
{
    public class CreateTaskRequestDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriorities Priority { get; set; }
    }
}
