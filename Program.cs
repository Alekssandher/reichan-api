using AspNetCoreRateLimit;

namespace reichan_api {
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterServices(builder.Configuration);
            builder.Services.RegisterFilters();
            builder.Services.AddControllers();
           


            var app = builder.Build();
            
            app.UseRouting();
            app.UseSession();
            app.UseIpRateLimiting();
            app.UseCors("AllowWithCredentials");

            app.MapControllers();

            app.Run();
        }
    }

}