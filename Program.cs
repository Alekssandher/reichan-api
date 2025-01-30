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

        //app.UseResponseCompression();
        app.UseIpRateLimiting();
        app.UseCors();
        app.Run();
    }
}