using PortProxy.Connector.Services;
using PortProxy.WinService;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();
await host.RunAsync();
