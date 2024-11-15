using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ShopApp.Configuration
{
    public class AppConfiguration
    {
        public string DataAccessMode { get; set; }
        public FileSettings FileSettings { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }

        public static AppConfiguration Load(string filePath)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(filePath, optional: false, reloadOnChange: true)
                .Build();

            var appConfig = new AppConfiguration();
            configuration.Bind(appConfig);
            return appConfig;
        }
    }

    public class FileSettings
    {
        public string ShopFilePath { get; set; }
        public string ProductFilePath { get; set; }
    }

    public class DatabaseSettings
    {
        public string DatabasePath { get; set; }
        public string InitScriptPath { get; set; }
        public string ConnectionString { get; set; }
    }
}
