using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;
using ShopApp.DAL.Repositories;
using ShopApp.Client.UI.MainForm;
using System.Windows.Forms;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Configuration/appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var dataAccessMode = configuration["DataAccessMode"];

        if (dataAccessMode == "Database")
        {
            var databasePath = configuration["DatabaseSettings:DatabasePath"];
            var initScriptPath = configuration["DatabaseSettings:InitScriptPath"];
            var connectionString = configuration["DatabaseSettings:ConnectionString"];

            if (!File.Exists(databasePath))
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    if (File.Exists(initScriptPath))
                    {
                        var initScript = await File.ReadAllTextAsync(initScriptPath);
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = initScript;
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                var repository = new SqlProductRepository(connectionString);
            }
        }
        else if (dataAccessMode == "File")
        {
            var productFilePath = configuration["FileSettings:ProductFilePath"];
            var repository = new FileProductRepository(productFilePath);
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}