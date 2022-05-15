using CleanArcTest.Core.Base;
using CleanArcTest.Core.Domain;
using CleanArcTest.Core.Persistence;
using CleanArcTest.Infrastructure.Data.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CleanArcTest.Infrastructure.Persistence
{
    public class CleanArcTestContext : DbContext, IApplicationDataContext
    {
        public CleanArcTestContext(DbContextOptions<CleanArcTestContext> options) : base(options)
        {
            
        }

        public DbSet<Trademark> Trademarks { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // If save was successful - publish domain events to message bus etc.
            var entitiesWithEvents = ChangeTracker.Entries<EntityWithDomainEvents>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    // Publish domain events using e.g. Mediatr, MassTransit etc.
                    Console.WriteLine("Publishing domain event: " + domainEvent.GetType().Name);
                    //await _mediator.Publish(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrademarkEntityTypeConfiguration).Assembly);
        }
    }
}
