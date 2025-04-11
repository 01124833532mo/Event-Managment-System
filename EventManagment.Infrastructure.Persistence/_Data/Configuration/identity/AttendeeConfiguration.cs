using EventManagment.Core.Domain.Entities._Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.identity
{
    public class AttendeeConfiguration : IEntityTypeConfiguration<Attendde>
    {
        public void Configure(EntityTypeBuilder<Attendde> builder)
        {
            builder.Property(x => x.BirthDate)
                      .HasColumnType("date")
                      .IsRequired(false);

            builder.HasMany(p => p.Feedbacks)
                .WithOne(p => p.Attendde)
                .HasForeignKey(p => p.AttenddeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);



        }
    }
}
