using Evenly.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Renci.SshNet;

namespace Evenly.Contexts
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<DataModel> data { get; set; }

    }
}
