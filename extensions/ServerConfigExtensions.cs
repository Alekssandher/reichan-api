using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace reichan_api.Extensions
{
    public static class ServerConfigExtensions
    {
        public static void ConfigureServer(this IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 1 * 1024 * 1024; // 1 MB
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 1 * 1024 * 1024;
                options.Limits.MaxConcurrentConnections = 20000;
            });
        }
    }
}
