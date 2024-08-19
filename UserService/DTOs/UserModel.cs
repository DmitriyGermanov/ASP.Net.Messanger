using UserService.Models;

namespace UserService.DTOs
{
    public class UserModel
    {
        public string Email { get; set; } = string.Empty;
        public virtual string? RoleName { get; set; }
    }
}
