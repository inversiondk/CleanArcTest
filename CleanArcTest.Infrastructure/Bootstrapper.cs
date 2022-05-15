using CleanArcTest.Core.Base.Contracts;
using CleanArcTest.Core.Persistence;
using CleanArcTest.Core.Repositories.Command;
using CleanArcTest.Core.Repositories.Command.Base;
using CleanArcTest.Core.Repositories.Query;
using CleanArcTest.Core.Repositories.Query.Base;
using CleanArcTest.Infrastructure.Data;
using CleanArcTest.Infrastructure.Persistence;
using CleanArcTest.Infrastructure.Repositories.Base;
using CleanArcTest.Infrastructure.Repositories.Command;
using CleanArcTest.Infrastructure.Repositories.Query;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArcTest.Infrastructure
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool seed = false)
        {
            var connectionString = configuration.GetConnectionString("SqliteConnection");
            services.AddDbContext<CleanArcTestContext>(options => options.UseSqlite(connectionString));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApplicationDataContext, CleanArcTestContext>();

            services.AddScoped(typeof(IQueryRepository<>), typeof(DapperRepository<>));
            services.AddScoped(typeof(ICommandRepository<>), typeof(EfRepository<>));

            services.AddTransient<ITrademarkCommandRepository, TrademarkCommandRepository>();
            services.AddTransient<ITrademarkQueryRepository, TrademarkQueryRepository>();

            if (seed)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    const string createTrademarksTable = @"
                        CREATE TABLE Trademarks (
                            Id INTEGER PRIMARY KEY,
                            Name VARCHAR(50) NOT NULL
                        );
                    ";

                    const string createRegistrationsTable = @"
    CREATE TABLE Registrations (
        Id INTEGER PRIMARY KEY,
        TrademarkId INTEGER NOT NULL,
        CountryIso VARCHAR(50) NOT NULL,
        RenewalPrice decimal(14,2) NOT NULL,
        FOREIGN KEY(TrademarkId) REFERENCES Trademarks(Id)
    );
";

                    // Add tables
                    //connection.Execute(createTrademarksTable);
                    connection.Execute(createRegistrationsTable);
                }
            }

            return services;
        }
        
    }
}
