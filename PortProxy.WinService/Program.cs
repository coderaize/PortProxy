using PortProxy.Connector.Services;
using PortProxy.WinService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();
await host.RunAsync();

//var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());
//// Additional configuration is required to successfully run gRPC on macOS.
//// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
//// Add services to the container.
//builder.Services.AddGrpc();
//var app = builder.Build();
//// Configure the HTTP request pipeline.
//app.UseAuthentication();
//app.MapGrpcService<PortService>();

//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
//app.Run();

