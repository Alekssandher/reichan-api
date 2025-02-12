using AspNetCoreRateLimit;
using Scalar.AspNetCore;

namespace reichan_api {
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterServices(builder.Configuration);
            builder.Services.RegisterFilters();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();
            if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
            {
                app.MapOpenApi()
                    .CacheOutput();
                app.MapScalarApiReference("/docs");
            }
        
            // app.UseHttpsRedirection();
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