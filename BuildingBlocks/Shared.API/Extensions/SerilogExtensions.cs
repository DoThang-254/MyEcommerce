using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Shared.API.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostBuilder UseSharedSerilog(this IHostBuilder hostBuilder, string serviceName)
        {
            return hostBuilder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Service", serviceName);
            });
        }

        public static IApplicationBuilder UseSharedSerilogRequestLogging(this IApplicationBuilder app)
        {
            return app.UseSerilogRequestLogging();
        }
    }
}
