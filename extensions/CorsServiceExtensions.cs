namespace reichan_api.Extensions
{
    public static class CorsServiceExtensions
    {
        public static void AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .WithMethods("GET", "POST", "PATCH")
                        .WithHeaders("X-CaptchaCode");
                });

                options.AddPolicy("AllowWithCredentials", builder =>
                {
                    builder.WithOrigins(
                        "http://localhost:4200", 
                        "https://alekssandher.github.io/reichan-web-client/", 
                        "http://127.0.0.1:8080", 
                        "https://alekssandher.github.io/angular-blog/"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}
