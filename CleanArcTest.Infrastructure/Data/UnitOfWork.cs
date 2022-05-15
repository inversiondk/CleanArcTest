using CleanArcTest.Core.Base.Contracts;
using CleanArcTest.Core.Persistence;
using CleanArcTest.Core.Repositories.Command;
using CleanArcTest.Infrastructure.Repositories.Command;

namespace CleanArcTest.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDataContext _context;

        public UnitOfWork(IApplicationDataContext context)
        {
            _context = context;
        }

        public ITrademarkCommandRepository Trademarks => new TrademarkCommandRepository(_context);


        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await _context.DisposeAsync();
            }
        }

    }
}
