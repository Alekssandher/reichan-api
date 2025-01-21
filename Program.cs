using FiltersChangeDefaultReturnErrors.Filters;

public class Program {

    public static void Main(string[] args) {
        
        
        Database.Instance.ConnectToMongoDb();

        var builder= WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddControllers();

        builder.Services.AddControllers(options =>
        {
            // Adding global personalized filter for all actions
            options.Filters.Add(typeof(ModelStateCheck));
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true; // Disable the automatic model validation
        });        

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}