using Microsoft.Extensions.FileProviders;

public class Program {

    public static void Main(string[] args) {
        
        
        Database.Instance.ConnectToMongoDb();

        var builder= WebApplication.CreateBuilder(args);

        builder.Services.RegisterServices();
        builder.Services.RegisterFilters();
        
        builder.Services.AddControllers();     

        var app = builder.Build();

        app.MapControllers();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "storage", "images")),
            RequestPath = "/storage/images"
        });
        app.UseCors();
        app.Run();
    }
}