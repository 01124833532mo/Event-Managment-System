using EventManagment.Core.Domain._Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManagment.Infrastructure.Persistence._Data
{
    public class EventManagmentDbContext : IdentityDbContext<ApplicationUser>
    {
        public EventManagmentDbContext(DbContextOptions<EventManagmentDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly);
        }
    }
}
