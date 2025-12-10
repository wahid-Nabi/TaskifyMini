using System.ComponentModel.DataAnnotations;

namespace TaskifyMini.Models.DTOs
{
    public class LoginEntityDto 
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;       
        public bool RememberMe { get; set; } = false;
    }
}
