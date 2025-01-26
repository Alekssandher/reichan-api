using MongoDB.Driver;
using System;
using DotNetEnv;
using MongoDB.Bson;

public class Database
{
    private static readonly Lazy<Database> _instance = new Lazy<Database>(() => new Database());    private MongoClient _client;

    public static Database Instance
    {
        get
        {
            return _instance.Value;
        }
    }

    public void ConnectToMongoDb()
    {
        Env.Load();

        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Database URL not set. Check your .env file.");
            Environment.Exit(0);
        }

        Console.WriteLine("Connecting to database...");
        _client = new MongoClient(connectionString);
        Console.WriteLine("Connected");
    }

    public MongoClient GetClient()
    {
        if (_client == null)
        {
            throw new InvalidOperationException("MongoClient is not initialized. Call ConnectToMongoDb() first.");
        }
        return _client;
    }

}
