using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserServiceContext(string connectionString) : DbContext
    {
        private readonly string _connectionString = connectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_connectionString
                              ?? throw new NullReferenceException("Connection string can't be Null."))
                              .UseLazyLoadingProxies();
            }
        }

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
