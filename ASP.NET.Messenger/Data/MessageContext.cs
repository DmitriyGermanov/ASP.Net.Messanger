using Microsoft.EntityFrameworkCore;

namespace MessagingService.Data
{
    public class MessageContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
    }
}
