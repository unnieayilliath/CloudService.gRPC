using CloudService.gRPC.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
var port= builder.Configuration.GetValue<int>("port");
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(port, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});
// Add services to the container.
builder.Services.AddGrpc();
//run as windows service
builder.Host.UseWindowsService();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CloudBrokerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
