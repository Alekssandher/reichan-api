using MongoDB.Driver;
using reichan_api.Configs;
using reichan_api.src.Models.Posts;

namespace reichan_api.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services)
        {
            DatabaseConfig databaseConfig = new()
            {
                DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "default_database",
                DatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb://localhost:27017",
                PostsCollection = Environment.GetEnvironmentVariable("POSTS_COLLECTION") ?? "posts",
                UsersCollection = Environment.GetEnvironmentVariable("USERS_COLLECTION") ?? "users",
                RepliesCollection = Environment.GetEnvironmentVariable("REPLIES_COLLECTION") ?? "replies"
            };

            services.AddSingleton(databaseConfig);
            services.AddSingleton<IMongoClient>(_ =>
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(databaseConfig.DatabaseUrl));
                settings.MaxConnectionPoolSize = 500;
                settings.MinConnectionPoolSize = 50;
                settings.WaitQueueTimeout = TimeSpan.FromSeconds(300);
                return new MongoClient(settings);
            });

            services.AddSingleton<IMongoDatabase>(sp =>
                sp.GetRequiredService<IMongoClient>().GetDatabase(databaseConfig.DatabaseName)
            );

            services.AddSingleton<IMongoCollection<ThreadModel>>(sp =>
            {
                IMongoDatabase database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<ThreadModel>(databaseConfig.PostsCollection);
            });
        }
    }
}
