using EventManagment.Core.Domain.Entities.Waitlists;
using EventManagment.Shared.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Data
{
    public class WaitListConfigurations : IEntityTypeConfiguration<WaitList>
    {
        public void Configure(EntityTypeBuilder<WaitList> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.JoinDate)
                .IsRequired(false);

            builder.Property(w => w.Status)
                .IsRequired()
                .HasConversion(
                  Sstatus => Sstatus.ToString(),
               (UStatus) => (WaitListStatus)Enum.Parse(typeof(WaitListStatus), UStatus));

            builder.Property(w => w.IsNotified)
                .IsRequired();

            builder.HasOne(w => w.Event)
                .WithMany(e => e.WaitLists)
                .HasForeignKey(w => w.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Attendde)
                .WithMany(a => a.WaitLists)
                .HasForeignKey(w => w.AttendeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
