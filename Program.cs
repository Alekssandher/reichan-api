using AspNetCoreRateLimit;
using DotNetEnv;
using reichan_api.Extensions;
using Scalar.AspNetCore;

namespace reichan_api {
    public class Program
    {
        
        public static void Main(string[] args)
        {
            Env.Load();
            
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCorsPolicies();
            builder.Services.ConfigureServer();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddCustomSession();
            builder.Services.AddRateLimiting(builder.Configuration);
            builder.Services.AddCloudinaryService();
            builder.Services.AddDatabaseServices();
            builder.Services.RegisterDependencies();

            builder.Services.AddHealthChecks();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
            {
                app.MapOpenApi()
                    .CacheOutput();
                app.MapScalarApiReference("/docs");
                app.MapHealthChecks("/health");
            }

            app.UseRouting();
            app.UseSession();
            app.UseIpRateLimiting();
            app.UseCors("AllowWithCredentials");
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }

}