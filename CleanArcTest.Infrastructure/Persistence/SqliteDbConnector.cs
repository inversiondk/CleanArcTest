using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;
using CleanArcTest.Core.Persistence;

namespace CleanArcTest.Infrastructure.Persistence
{
    public abstract class SqliteDbConnector : IDbConnector
    {
        private readonly IConfiguration _configuration;

        protected SqliteDbConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("SqliteConnection");
            return new SqliteConnection(connectionString);
        }
    }
    
}
