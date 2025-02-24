using Scalar.AspNetCore;

namespace reichan_api.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "Reichan API Documentation",
                        Version = "v1",
                        Description = "An API to interact with a 4chan-like forum."
                    };
                    return Task.CompletedTask;
                });
            });
        }

        public static void UseSwaggerDocumentation(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference("/docs");
        }
    }
}
