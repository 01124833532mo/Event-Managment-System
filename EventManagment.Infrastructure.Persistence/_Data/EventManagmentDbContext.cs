using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Notifications;
using EventManagment.Core.Domain.Entities.Registrations;
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

        public DbSet<Event> Events { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<Notification> Notifications { get; set; }

    }
}
