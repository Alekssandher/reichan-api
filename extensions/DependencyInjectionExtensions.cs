using IdGen.DependencyInjection;
using reichan_api.filters.threads;
using reichan_api.Filters;
using reichan_api.Filters.captcha;
using reichan_api.Filters.threads;
using reichan_api.src.Interfaces;
using reichan_api.src.Interfaces.replies;
using reichan_api.src.Modules.Threads;
using reichan_api.src.Modules.replies;
using reichan_api.src.Repositories;

namespace reichan_api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddIdGen(123);
            services.AddScoped<IThreadService, ThreadsService>();
            services.AddScoped<IThreadRepository, PostsRepository>();
            services.AddScoped<IReplyService, RepliesService>();
            services.AddScoped<IReplyRepository, ReplyRepository>();

            services.AddScoped<ValidateQueryAttribute>();
            services.AddScoped<ValidateIdAttribute>();
            services.AddScoped<ValidateGetMedia>();
            services.AddScoped<ValidateCaptcha>();
            
            services.AddHttpContextAccessor();
        }
    }
}
