using EventManagment.Core.Domain.Entities.FeedBacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Data
{
    public class FeedBacksConfigurations : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(x => x.Rate)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.Comment)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");


        }
    }
}
