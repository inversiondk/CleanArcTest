using CleanArcTest.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArcTest.Core.Persistence
{
    public interface IApplicationDataContext
    {
        DbSet<Trademark> Trademarks { get; set; }
        DbSet<Registration> Registrations { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
        void Dispose();
        ValueTask DisposeAsync();
    }
}
