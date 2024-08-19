namespace UserService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public byte[] Password { get; set; } = [];
        public byte[] Salt { get; set; } = [];
        public RoleId RoleId { get; set; }
        public virtual Role Role { get; set; } = new Role();
    }
}
