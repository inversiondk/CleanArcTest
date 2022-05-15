using Ardalis.GuardClauses;
using CleanArcTest.Core.Domain;
using CleanArcTest.Core.Persistence;
using CleanArcTest.Core.Repositories.Command;
using CleanArcTest.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CleanArcTest.Infrastructure.Repositories.Command
{
    public class TrademarkCommandRepository : EfRepository<Trademark>, ITrademarkCommandRepository
    {
        public TrademarkCommandRepository(IApplicationDataContext context) : base(context)
        {
        }

        public void CreateTrademark(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name), "Not a valid trademark name");

            var trademark = new Trademark(name);
            Context.Trademarks.Add(trademark);
        }

        public async Task ChangeTrademarkName(int trademarkId, string name)
        {
            Guard.Against.NegativeOrZero(trademarkId, nameof(trademarkId), "Not a valid trademark ID");
            Guard.Against.NullOrEmpty(name, nameof(name), "Not a valid trademark name");

            var trademark = await Context.Trademarks.SingleOrDefaultAsync(i => i.Id == trademarkId);
            if (trademark == null)
                throw new Exception("Trademark not found");

            trademark.ChangeName(name);
        }

        public async Task AddRegistration(int trademarkId, Registration registration)
        {
            Guard.Against.NegativeOrZero(trademarkId, nameof(trademarkId), "Not a valid trademark ID");
            Guard.Against.Null(registration, nameof(registration), "Registration cannot be null");

            var trademark = await Context.Trademarks.SingleOrDefaultAsync(i => i.Id == trademarkId);
            if (trademark == null)
                throw new Exception("Trademark not found");

            trademark.AddRegistration(registration);
        }
    }
}
