using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
namespace PortProxy.WinService
{
    public class GrpcServerStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<PortProxy.Connector.Services.PortService>();                
            });
        }
    }
}