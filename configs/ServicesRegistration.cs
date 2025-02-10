using AspNetCoreRateLimit;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;
using reichan_api.src.Interfaces;
using reichan_api.src.Models.Posts;
using reichan_api.src.Modules.Posts;

public static class ServicesRegistration
{
    
    public static void RegisterServices(this IServiceCollection services, IConfiguration Configuration)
    {
        Env.Load();
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()  
                    .WithMethods("GET", "POST") 
                    .WithHeaders("X-CaptchaCode"); 
            });

            options.AddPolicy("AllowWithCredentials", builder =>
            {
                builder.WithOrigins("https://alekssandher.github.io/reichan-web-client/", "http://127.0.0.1:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); 
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
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new()
                {
                    Title = "Reichan API",
                    Version = "v1",
                    Description = "API for interacting with a web forum."
                };
                return Task.CompletedTask;
            });
        });
        // Session
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(3);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        // Rate limit service configuration
        services.AddOptions();
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        // Configs Database
        var databaseConfig = new DatabaseConfig
        {
            DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "default_database",
            DatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb://localhost:27017",
            PostsCollection = Environment.GetEnvironmentVariable("POSTS_COLLECTION") ?? "posts",
            UsersCollection = Environment.GetEnvironmentVariable("USERS_COLLECTION") ?? "users",
            RepliesCollection = Environment.GetEnvironmentVariable("REPLIES_COLLECTION") ?? "replies"
        };
    
        services.AddSingleton(databaseConfig);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(databaseConfig.DatabaseUrl));
        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase(databaseConfig.DatabaseName)
        );
        services.AddScoped<IMongoCollection<PostModel>>(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            var databaseConfig = sp.GetRequiredService<DatabaseConfig>();
            return database.GetCollection<PostModel>(databaseConfig.PostsCollection);
        });

    
        services.AddScoped<IPostService, PostsService>();
    }

    public static void RegisterFilters(this IServiceCollection services)
    {

        services.AddScoped<ValidateCategory>();
        services.AddScoped<ValidateCaptcha>();
        
        services.AddControllers(options =>
        {
            options.Filters.Add(new RequestSizeLimitAttribute(5 * 1024 * 1024)); // 5MB
            //options.Filters.Add<ModelStateCheck>();
    
        }); 
        // .ConfigureApiBehaviorOptions(options =>
        // {
        //     options.SuppressModelStateInvalidFilter = true;
        // });
    }
}
