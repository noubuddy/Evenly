using Evenly.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Evenly.Contexts
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Data> Data { get; set; }
        public DbSet<User> User { get; set; }

    }
}
