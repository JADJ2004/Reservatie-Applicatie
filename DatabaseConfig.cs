using System;
using System.IO;

public static class DatabaseConfig
{
    private static readonly string databaseFileName = "Mydatabase.db";
    
    public static string GetConnectionString()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string databaseFilePath = Path.Combine(basePath, databaseFileName);
        return $"Data Source={databaseFilePath}";
    }
}
