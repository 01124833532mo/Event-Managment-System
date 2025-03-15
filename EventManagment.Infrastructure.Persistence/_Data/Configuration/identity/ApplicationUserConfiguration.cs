using EventManagment.Core.Domain._Identity;
using EventManagment.Core.Domain.Entities._Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.identity
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(user => user.Types)
                .HasConversion
                (
                (UStatus) => UStatus.ToString(),
                (UStatus) => (Types)Enum.Parse(typeof(Types), UStatus)
                );

        }
    }
}
