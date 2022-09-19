using System.Data.Common;

namespace Mango.Contracts.Connections
{
    public class Connections
    {
        private static readonly object _lock = new object();

        private static string? connectionString = null;

        private Connections() { }

        public static string GetDBConnectionString()
        {
            if (connectionString == null)
            {
                lock (_lock)
                {
                    if (connectionString == null)
                    {
                        var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("db-connection.json")
                        .Build();

                        connectionString = configuration.GetConnectionString("DefaultConnection");
                    }
                }
            }
            return connectionString;

        }

        public static T GetConnectionStringFrom<T>(string file, string key)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(file)
                .Build();

            return configuration.GetValue<T>(key);
        }
    }
}
