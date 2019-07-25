using Microsoft.EntityFrameworkCore;

namespace Player_Service.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public DbSet<User> UserItems { get; set; }
    }
}