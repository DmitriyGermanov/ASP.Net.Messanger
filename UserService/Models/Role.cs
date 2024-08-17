namespace UserService.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual IEnumerable<User> Users { get; set; } = [];
    }
}
