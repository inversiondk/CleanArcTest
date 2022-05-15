using CleanArcTest.Core.Domain;
using CleanArcTest.Core.Repositories.Query;
using CleanArcTest.Infrastructure.Repositories.Base;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CleanArcTest.Infrastructure.Repositories.Query
{
    public class TrademarkQueryRepository : DapperRepository<Trademark>, ITrademarkQueryRepository
    {
        public TrademarkQueryRepository(IConfiguration configuration) : base(configuration)
        {
            
        }

        public async Task<IReadOnlyCollection<Trademark>> GetAllTrademarksAsync(CancellationToken token)
        {
            var sql = "SELECT * FROM Trademarks";

            using (var conn = CreateConnection())
            {
                var command = new CommandDefinition(sql, cancellationToken: token);
                conn.Open();

                return (await conn.QueryAsync<Trademark>(command)).ToList();
            }
        }
    }
}
