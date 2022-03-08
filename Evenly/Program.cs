using Evenly;
using Evenly.Contexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Renci.SshNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//using (var client = new SshClient("157.230.107.52", "root", "1209348756"))
//{
//    client.Connect();

//    if (!client.IsConnected)
//    {
//        Console.WriteLine("Client not connected!");
//    }
//    else
//    {
//        Console.WriteLine("Client connected!");
//    }

//    var port = new ForwardedPortLocal("127.0.0.1", 5432, "127.0.0.1", 5432);
//    client.AddForwardedPort(port);
//    port.Start();

//    string connString =
//        $"Server={port.BoundHost};Database=evenly;Port={port.BoundPort};" +
//         "User Id=noubuddy;Password=1209348756;";

//    using (var conn = new NpgsqlConnection(connString))
//    {
//        conn.Open();
//        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connString));
//    }

//    port.Stop();
//    client.Disconnect();
//}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();