using AspNetCoreRateLimit;

public class Program {

    public static void Main(string[] args) {
        
        
        Database.Instance.ConnectToMongoDb();

        var builder= WebApplication.CreateBuilder(args);

        builder.Services.RegisterServices(builder.Configuration);
        builder.Services.RegisterFilters();
        
        builder.Services.AddControllers();     
        
        var app = builder.Build();

        app.MapControllers();

<<<<<<< HEAD
=======
        
        app.UseSession();
>>>>>>> 10236a3 (feat: session)
        app.UseRouting();
        app.UseSession();

        // app.UseMiddleware<CooldownMiddleware>();
        // app.UseMiddleware<SecureApiMiddleware>();
        app.UseWhen(context => context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase), appBuilder =>
        {
            appBuilder.UseMiddleware<ValidateCaptchaMiddleware>();
        });
        
        app.UseIpRateLimiting();
        app.UseCors();
        app.Run();
    }
}