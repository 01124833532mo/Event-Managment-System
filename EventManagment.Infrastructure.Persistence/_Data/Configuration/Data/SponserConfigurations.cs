using EventManagment.Core.Domain.Entities.Sponsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Data
{
    public class SponserConfigurations : IEntityTypeConfiguration<Sponser>
    {
        public void Configure(EntityTypeBuilder<Sponser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");
            builder.Property(x => x.LogoUrl)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");
            builder.Property(x => x.Website)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.HasMany(p => p.Events)
                    .WithOne(p => p.Sponser)
                    .HasForeignKey(p => p.SponserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
