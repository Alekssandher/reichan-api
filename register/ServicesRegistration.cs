using FiltersChangeDefaultReturnErrors.Filters;

public static class ServicesRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // Registre aqui todos os seus serviços
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostsService>();
        
        // Se tiver mais serviços, adicione-os aqui
    }

    public static void RegisterFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidateSignature>();
        
        // Registre aqui seus filtros globais ou personalizados
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ModelStateCheck)); // Exemplo de filtro personalizado
    
        })  
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true; // Desativa a validação automática do model
        });
    }
}