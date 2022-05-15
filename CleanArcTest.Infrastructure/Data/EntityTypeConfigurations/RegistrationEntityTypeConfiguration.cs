using CleanArcTest.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArcTest.Infrastructure.Data.EntityTypeConfigurations
{
    public class RegistrationEntityTypeConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.ToTable("Registrations");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.CountryIso).HasMaxLength(50).IsRequired();
            builder.Property(i => i.RenewalPrice).HasPrecision(2, 14).IsRequired();
            builder.Ignore(i => i.DomainEvents);

            builder.HasOne(i => i.Trademark).WithMany(i => i.Registrations);
        }
    }
}
