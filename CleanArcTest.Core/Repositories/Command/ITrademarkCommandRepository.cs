using CleanArcTest.Core.Domain;
using CleanArcTest.Core.Repositories.Command.Base;

namespace CleanArcTest.Core.Repositories.Command
{
    public interface ITrademarkCommandRepository : ICommandRepository<Trademark>
    {
        void CreateTrademark(string name);
        Task ChangeTrademarkName(int trademarkId, string name);
        Task AddRegistration(int trademarkId, Registration registration);
    }
}
