using AspNetCoreRateLimit;
using FiltersChangeDefaultReturnErrors.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;

public static class ServicesRegistration
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()  
                    .WithMethods("GET", "POST") 
                    .WithHeaders("X-CaptchaCode"); 
            });
        });

        services.Configure<IISServerOptions>(options =>
        {
            options.MaxRequestBodySize = 1 * 1024 * 1024; // 1 MB
        });

        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 1 * 1024 * 1024; // 1 MB
        });
        
        // Session
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(3);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        // Rate limit service configuration
        services.AddOptions();
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        // Interfaces 
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostsService>();
        services.AddScoped<IReplyService, RepliesService>();

        // Singleton
        services.AddSingleton(new DatabaseConfig
        {
            DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "default_database",
            DatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb://localhost:27017",
            PostsCollection = Environment.GetEnvironmentVariable("POSTS_COLLECTION") ?? "posts",
            UsersCollection = Environment.GetEnvironmentVariable("USERS_COLLECTION") ?? "users",
            RepliesCollection = Environment.GetEnvironmentVariable("REPLIES_COLLECTION") ?? "replies"
        });
    }

    public static void RegisterFilters(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(new RequestSizeLimitAttribute(5 * 1024 * 1024)); // 5MB
        });
        services.AddScoped<ValidateSignature>();
        services.AddScoped<ValidateCategory>();
        services.AddScoped<ValidateCategoryPost>();
        
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ModelStateCheck));
    
        })  
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}