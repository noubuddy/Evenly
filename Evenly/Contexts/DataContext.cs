using Evenly.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Evenly.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DataModel> Data { get; set; }

    }
}
