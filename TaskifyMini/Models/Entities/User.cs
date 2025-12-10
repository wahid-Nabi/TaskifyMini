namespace TaskifyMini.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int RoleId { get; set; }
        public Roles Role { get; set; } = null!;
        public string Phone { get; set; } = null!;
        // Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
    public class Roles
    {
        public int Id {  get; set;}
        public string RoleName { get; set; } = null!;
        public List<User> Users { get; set; } = new List<User>();
    }
}
