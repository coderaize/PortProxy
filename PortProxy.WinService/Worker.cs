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
                                       options.ListenAnyIP(5000, listenOptions =>
                                       {                                           
                                           listenOptions.Protocols = HttpProtocols.Http2;
                                           listenOptions.UseHttps();                                           
                                       });
                                   })
                                   .UseKestrel()
                                   .UseStartup<GrpcServerStartup>();
                           })
                           .Build()
                           .StartAsync(stoppingToken);
            /////
            var c = Cache.Configs;
            foreach (string k in c.Keys)
            {
                ProxyThread proxyThread = new ProxyThread(k, c[k]);
                proxyThread.StartProxy();
                Cache.ActiveSession.Add(new ActiveSession(k, c[k], new List<string>(), new List<string>(), proxyThread));
            }

            int i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                i++;
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                if (i % 60 == 0)
                {
                    Console.WriteLine($"Active Sesison {DateTime.Now}");
                    Cache.ActiveSession.ForEach(X =>
                    {
                        Console.WriteLine($"{X.ProxyName}  Tcp:{string.Join(',', X.TcpClients.ToArray())}  Udp:{string.Join(',', X.UdpClients.ToArray())}");
                    });
                }
            }
        }



    }
}