using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Enums;
using EventManagment.Infrastructure.Persistence._Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Data
{
    internal class EventConfigurations : BaseAuditableEntityConfigurations<Event, int>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {



            base.Configure(builder);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);



            builder.Property(p => p.Title)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);

            builder.Property(p => p.Description)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(200);

            builder.Property(p => p.Location)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);
            builder.Property(p => p.MaxAttendees)
                    .HasDefaultValue(20);
            builder.Property(s => s.Status)
               .HasConversion(
               Sstatus => Sstatus.ToString(),
               (UStatus) => (EventStatus)Enum.Parse(typeof(EventStatus), UStatus)
               );



            builder.HasOne(p => p.Organizer)
                    .WithMany(p => p.Events)
                    .HasForeignKey(p => p.OrganizerId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Registrations)
                    .WithOne(p => p.Event)
                    .HasForeignKey(p => p.Eventid)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Notifications)
                    .WithOne(p => p.Event)
                    .HasForeignKey(p => p.eventid)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Category)
                 .WithMany(p => p.Events)
                 .HasForeignKey(p => p.Categoryid)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
