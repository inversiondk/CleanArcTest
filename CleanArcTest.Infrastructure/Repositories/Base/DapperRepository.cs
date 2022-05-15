using CleanArcTest.Core.Base;
using CleanArcTest.Core.Repositories.Query.Base;
using CleanArcTest.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

namespace CleanArcTest.Infrastructure.Repositories.Base
{
    public class DapperRepository<T> : SqliteDbConnector, IQueryRepository<T> where T : Entity
    {
        protected DapperRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
