namespace TaskifyMini.Models.DTOs
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;  
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
