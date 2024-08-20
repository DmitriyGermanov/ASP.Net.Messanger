using MessagingService.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Data
{
    public class MessageContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(message =>
            {
                message.HasKey(message => message.Id)
                       .HasName("PK_Message_Id");
                message.Property(m => m.Id)
                       .ValueGeneratedOnAdd();
            });
        }
    }
}
