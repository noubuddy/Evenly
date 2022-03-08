using Evenly.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Renci.SshNet;

namespace Evenly.Contexts
{
    public class DataContext : DbContext
    {
        SshClient client = new SshClient("157.230.107.52", "root", "1209348756");
        ForwardedPortLocal port = new ForwardedPortLocal("127.0.0.1", 5432, "127.0.0.1", 5432);

        public DataContext(DbContextOptions options) : base(options)
        {
            client.Connect();
            client.AddForwardedPort(port);
            port.Start();
            
        }

        //~DataContext()
        //{
        //    client.Disconnect();
        //    port.Stop();
        //}


        public DbSet<DataModel> Data { get; set; }

    }
}
