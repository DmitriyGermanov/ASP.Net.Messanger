namespace UserService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public RoleId? RoleId { get; set; }
        public virtual Role? Role { get; set; }
    }
}
