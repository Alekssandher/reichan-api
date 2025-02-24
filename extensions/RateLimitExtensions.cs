using AspNetCoreRateLimit;

namespace reichan_api.Extensions
{
    public static class RateLimitExtensions
    {
        public static void AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
