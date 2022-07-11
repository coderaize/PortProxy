using Microsoft.AspNetCore.Builder;
using PortProxy.Service;
using PortProxy;


var ch = ConfigHandler.GetMappedProxies;
if (ch == null)
{
    Console.WriteLine("Proxies not found");
}
else
{
    var tasks = ch.SelectMany(c => ProxyThreads.StartProxy(c.Key, c.Value));
    //Task.WhenAll(tasks).Wait(); 
    Task.Run(() =>
    {
        Task.WhenAll(tasks).Wait();
    });
    
}

var builder = WebApplication.CreateBuilder(args);
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
// Add services to the container.
builder.Services.AddGrpc();
var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapGrpcService<PortProxy.gRPC.Services.PortService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();



