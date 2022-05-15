using CleanArcTest.Core.Domain;
using CleanArcTest.Core.Repositories.Query.Base;

namespace CleanArcTest.Core.Repositories.Query
{
    public interface ITrademarkQueryRepository : IQueryRepository<Trademark>
    {
        Task<IReadOnlyCollection<Trademark>> GetAllTrademarksAsync(CancellationToken token);
    }
}
