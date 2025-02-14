using AspNetCoreRateLimit;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;
using reichan_api.Filters;
using reichan_api.src.Interfaces;
using reichan_api.src.Models.Posts;
using reichan_api.src.Modules.Posts;
using reichan_api.src.Repositories;
using Scalar.AspNetCore;

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
                    .WithMethods("GET", "POST", "PATCH") 
                    .WithHeaders("X-CaptchaCode"); 
            });

            options.AddPolicy("AllowWithCredentials", builder =>
            {
                builder.WithOrigins("https://alekssandher.github.io/reichan-web-client/", "http://127.0.0.1:8080", "http://localhost:6565")
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
            options.Limits.MaxConcurrentConnections = 20000;
            
        });
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new()
                {
                    Title = "Reichan API Documentation",
                    Version = "v1",
                    Description = "An API to interact with a forum."
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
        DatabaseConfig databaseConfig = new DatabaseConfig
        {
            DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "default_database",
            DatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb://localhost:27017",
            PostsCollection = Environment.GetEnvironmentVariable("POSTS_COLLECTION") ?? "posts",
            UsersCollection = Environment.GetEnvironmentVariable("USERS_COLLECTION") ?? "users",
            RepliesCollection = Environment.GetEnvironmentVariable("REPLIES_COLLECTION") ?? "replies"
        };

        services.AddSingleton(databaseConfig);
        services.AddSingleton<IMongoClient>(_ => 
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(databaseConfig.DatabaseUrl));
            settings.MaxConnectionPoolSize = 500;  
            settings.MinConnectionPoolSize = 50;  
            settings.WaitQueueTimeout = TimeSpan.FromSeconds(300);  
            return new MongoClient(settings);
        });
        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase(databaseConfig.DatabaseName)
        );
        services.AddSingleton<IMongoCollection<PostModel>>(sp =>
        {
            IMongoDatabase database = sp.GetRequiredService<IMongoDatabase>();
            DatabaseConfig databaseConfig = sp.GetRequiredService<DatabaseConfig>();
            return database.GetCollection<PostModel>(databaseConfig.PostsCollection);
        });
        services.AddIdGen(123);

    
        services.AddScoped<IPostService, PostsService>();
        services.AddScoped<IPostRepository, PostsRepository>();

        services.AddHttpContextAccessor();
    }

    public static void RegisterFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidateCaptcha>();
        services.AddScoped<ValidateIdAttribute>();
        services.AddScoped<ValidateQueryAttribute>();
        
        services.AddControllers(options =>
        {
            options.Filters.Add(new RequestSizeLimitAttribute(5 * 1024 * 1024)); // 5MB
            //options.Filters.Add<ModelStateCheck>();
    
        }); 
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<PostDtoValidator>();
        
        // .ConfigureApiBehaviorOptions(options =>
        // {
        //     options.SuppressModelStateInvalidFilter = true;
        // });
    }
}
