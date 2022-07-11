using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PortProxy;
using PortProxy.Connector.Services;
namespace PortProxy.WinService
{
    public class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Host.CreateDefaultBuilder()
                           .ConfigureWebHostDefaults(builder =>
                           {
                               builder
                                   .ConfigureKestrel(options =>
                                   {
                                       options.ListenAnyIP(0, listenOptions =>
                                       {
                                           listenOptions.Protocols = HttpProtocols.Http2;
                                           
                                       });                                       
                                   })
                                   .UseKestrel()
                                   .UseStartup<GrpcServerStartup>();
                           })
                           .Build()
                           .StartAsync(stoppingToken);
            /////
            var mp = PortProxy.ConfigHandler.GetMappedProxies;
            if (mp != null)
                foreach (string proxyName in mp.Keys)
                {
                    var proxyConfig = mp[proxyName];
                    var pT = PortProxy.ProxyThreads.ProxyThread(proxyName, proxyConfig);
                    pT.Start();
                }

           

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
                
            }
        }

    }
}