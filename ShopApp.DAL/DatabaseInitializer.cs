using Microsoft.Data.Sqlite;
using System.IO;
using System.Threading.Tasks;

namespace ShopApp.DAL
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(string connectionString, string databasePath, string initScriptPath)
        {
            if (!File.Exists(databasePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(databasePath));
            }

            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            string initScript = await File.ReadAllTextAsync(initScriptPath);
            using var command = connection.CreateCommand();
            command.CommandText = initScript;
            await command.ExecuteNonQueryAsync();
        }
    }
}
