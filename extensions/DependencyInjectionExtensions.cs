using IdGen.DependencyInjection;
using reichan_api.filters.Posts;
using reichan_api.Filters;
using reichan_api.Filters.captcha;
using reichan_api.src.Interfaces;
using reichan_api.src.Modules.Posts;
using reichan_api.src.Repositories;

namespace reichan_api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddIdGen(123);
            services.AddScoped<IPostService, PostsService>();
            services.AddScoped<IPostRepository, PostsRepository>();

            services.AddScoped<ValidateQueryAttribute>();
            services.AddScoped<ValidateIdAttribute>();
            services.AddScoped<ValidateGetMedia>();
            services.AddScoped<ValidateCaptcha>();
            
            services.AddHttpContextAccessor();
        }
    }
}
