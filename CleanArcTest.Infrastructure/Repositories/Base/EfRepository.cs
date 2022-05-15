using CleanArcTest.Core.Base.Contracts;
using CleanArcTest.Core.Persistence;

namespace CleanArcTest.Infrastructure.Repositories.Base
{
    public class EfRepository<T> where T : IAggregateRoot
    {
        protected readonly IApplicationDataContext Context;

        protected EfRepository(IApplicationDataContext context)
        {
            Context = context;
        }
    }
}
