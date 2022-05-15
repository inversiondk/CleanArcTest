using CleanArcTest.Core.Repositories.Command;

namespace CleanArcTest.Core.Base.Contracts
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        ITrademarkCommandRepository Trademarks { get; }
        Task<int> CommitAsync();
    }
}
