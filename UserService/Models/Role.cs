namespace UserService.Models
{
    public class Role
    {
        public RoleId RoleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual IEnumerable<User> Users { get; set; } = [];
    }
}
