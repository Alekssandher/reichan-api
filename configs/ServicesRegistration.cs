using FiltersChangeDefaultReturnErrors.Filters;

public static class ServicesRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostsService>();
        services.AddScoped<IReplyService, RepliesService>();
        services.AddSingleton(new DatabaseConfig
        {
            DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "default_database",
            DatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb://localhost:27017",
            PostsCollection = Environment.GetEnvironmentVariable("POSTS_COLLECTION") ?? "posts",
            UsersCollection = Environment.GetEnvironmentVariable("USERS_COLLECTION") ?? "users",
            RepliesCollection = Environment.GetEnvironmentVariable("REPLIES_COLLECTION") ?? "replies"
        });
    }

    public static void RegisterFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidateSignature>();
        
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ModelStateCheck));
    
        })  
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}