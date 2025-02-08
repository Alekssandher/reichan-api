using AspNetCoreRateLimit;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Adiciona serviços
        builder.Services.RegisterServices(builder.Configuration);
        builder.Services.RegisterFilters();
        builder.Services.AddControllers();

        var app = builder.Build();

        // Middlewares na ordem correta
        app.UseRouting();
        app.UseSession();
        app.UseIpRateLimiting();
        app.UseCors("AllowWithCredentials");

        // Mapeia os controllers
        app.MapControllers();

        app.Run();
    }
}
