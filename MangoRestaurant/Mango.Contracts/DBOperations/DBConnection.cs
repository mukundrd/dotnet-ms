namespace Mango.Contracts.DBOperations
{
    public class DBConnection
    {
        public static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("db-connection.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }
    }
}
