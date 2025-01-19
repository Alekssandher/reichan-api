using MongoDB.Driver;
using System;
using DotNetEnv;
using MongoDB.Bson;

public class Database
{
    private static Database _instance;
    private MongoClient _client;

    private Database() { }

    public static Database Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Database();
            }
            return _instance;
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
