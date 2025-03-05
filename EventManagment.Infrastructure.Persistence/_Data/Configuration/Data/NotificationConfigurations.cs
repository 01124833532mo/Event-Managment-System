using EventManagment.Core.Domain.Entities.Notifications;
using EventManagment.Infrastructure.Persistence._Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Data
{
    internal class NotificationConfigurations : BaseAuditableEntityConfigurations<Notification, int>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            base.Configure(builder);


            builder.Property(p => p.Message)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(200);

            builder.HasOne(s => s.Attendee)
                .WithMany(u => u.Notifications)
                .HasForeignKey(s => s.AttendeeId)
                .OnDelete(DeleteBehavior.NoAction);


        }

    }
}
