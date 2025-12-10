namespace TaskifyMini.Models.DTOs
{
    public class UpdateTaskResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
