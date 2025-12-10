using System.ComponentModel.DataAnnotations;

namespace TaskifyMini.Models.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Phone]
        [MaxLength(15)]
        public string Phone { get; set; } = null!;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
        [Required]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
