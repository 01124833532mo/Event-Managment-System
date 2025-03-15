using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Enums;
using EventManagment.Infrastructure.Persistence._Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Data
{
    internal class RegistrationConfigurations : BaseAuditableEntityConfigurations<Registration, int>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {

            base.Configure(builder);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.Property(s => s.PaymentStatus)
             .HasConversion(
             Sstatus => Sstatus.ToString(),
             (UStatus) => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), UStatus)
             );

            builder.HasOne(s => s.Attendee)
               .WithMany(u => u.Registrations)
               .HasForeignKey(s => s.AttendeeId)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
