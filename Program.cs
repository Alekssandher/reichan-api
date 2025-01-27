public class Program {

    public static void Main(string[] args) {
        
        
        Database.Instance.ConnectToMongoDb();

        var builder= WebApplication.CreateBuilder(args);

        builder.Services.RegisterServices();
        builder.Services.RegisterFilters();
        
        builder.Services.AddControllers();     

        var app = builder.Build();
        
        app.MapControllers();

        app.Run();
    }
}