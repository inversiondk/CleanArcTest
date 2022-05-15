using CleanArcTest.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArcTest.Infrastructure.Data.EntityTypeConfigurations
{
    public class TrademarkEntityTypeConfiguration : IEntityTypeConfiguration<Trademark>
    {
        public void Configure(EntityTypeBuilder<Trademark> builder)
        {
            builder.ToTable("Trademarks");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            builder.Ignore(x => x.DomainEvents);
        }
    }
}
