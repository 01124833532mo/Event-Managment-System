using EventManagment.Core.Domain.Entities._Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.identity
{
    public class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
    {
        public void Configure(EntityTypeBuilder<Organizer> builder)
        {
            builder.Property(x => x.Age)
                    .IsRequired(false);


            builder.Property(p => p.Address)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);


            builder.Property(p => p.CompanyName)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);
        }
    }
}
