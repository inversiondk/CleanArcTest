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
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.Entity is AuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var softDeletedEntries = ChangeTracker.Entries().Where(x => x.Entity is DeletableEntity && x.State == EntityState.Deleted);

            // TODO: Load from ICurrentUserService or similar once implemented
            //var userId = currentUser.Id;
            //var userName = currentUser.Name;

            // LOOP MODIFIED ENTRIES
            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is AuditableEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        //entity.CreatedBy = userId;
                        entity.Created = DateTime.UtcNow;
                        //entity.CreatedBy = userName;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.Created).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    }

                    //entity.ModifiedBy = userId;
                    entity.Modified = DateTime.UtcNow;
                    //entity.ModifiedBy = userName;
                }
            }

            // LOOP ENTITIES TO DELETE
            foreach (var softDeletedEntry in softDeletedEntries)
            {
                if (softDeletedEntry.Entity is DeletableEntity entity)
                {
                    entity.Deleted = DateTime.UtcNow;
                    softDeletedEntry.State = EntityState.Modified; // Needed to ensure that the item in the DB gets updated and not hard deleted
                }
            }

            // Save changes
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
