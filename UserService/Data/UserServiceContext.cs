using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserServiceContext(DbContextOptions<UserServiceContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id).HasName("PK_User_Id");
                user.HasIndex(u => u.Email)
                    .IsUnique();
                user.HasOne(user => user.Role)
                    .WithMany(role => role.Users)
                    .HasForeignKey(user => user.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>(role =>
            {
                role.HasKey(r => r.Id).HasName("PK_Role_Id");
                role.HasIndex(r => r.Name)
                    .IsUnique();
                role.HasMany(role => role.Users)
                    .WithOne(user => user.Role)
                    .HasForeignKey(user => user.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
