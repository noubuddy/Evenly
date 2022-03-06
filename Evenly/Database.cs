using Npgsql;
using Renci.SshNet;

namespace Evenly
{
    public class Database
    {
        public async static void Connect()
        {
            using (var client = new SshClient("157.230.107.52", "root", "1209348756"))
            {
                client.Connect();

                if (!client.IsConnected)
                {
                    Console.WriteLine("Client not connected!");
                }
                else
                {
                    Console.WriteLine("Client connected!");
                }

                var port = new ForwardedPortLocal("127.0.0.1", "127.0.0.1", 5432);
                client.AddForwardedPort(port);
                port.Start();

                string connString =
                    $"Server={port.BoundHost};Database=evenly;Port={port.BoundPort};" +
                     "User Id=noubuddy;Password=1209348756;";

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    await using var cmd = new NpgsqlCommand("select * from public.\"Data\"", conn);
                    await using var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(reader.GetValue(0));
                        Console.WriteLine(reader.GetValue(1));
                        Console.WriteLine(reader.GetValue(2));
                    }
                }

                port.Stop(); 
                client.Disconnect();
            }
        }
    }
}
