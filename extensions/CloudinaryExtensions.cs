using CloudinaryDotNet;
using reichan_api.Configs;

namespace reichan_api.Extensions
{
    public static class CloudinaryExtensions
    {
        public static void AddCloudinaryService(this IServiceCollection services)
        {
            CloudinarySettings cloudinarySettings = new()
            {
                CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_NAME") ?? "",
                ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_KEY") ?? "",
                ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_SECRET") ?? ""
            };

            Account cloudinaryAccount = new (
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            );

            Cloudinary cloudinary = new(cloudinaryAccount);
            
            services.AddSingleton(cloudinary);
        }
    }
}
