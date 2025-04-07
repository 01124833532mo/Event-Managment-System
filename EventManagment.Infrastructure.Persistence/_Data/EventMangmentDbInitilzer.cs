using EventManagment.Core.Domain._Identity;
using EventManagment.Core.Domain.Contracts.Persestence.DbInitializers;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Speakers;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EventManagment.Infrastructure.Persistence._Data
{
    public class EventMangmentDbInitilzer(EventManagmentDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IEventManagmentDbInitializer
    {

        public async Task InitializeAsync()
        {
            var pendingmigration = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingmigration.Any())
            {
                await dbContext.Database.MigrateAsync();
            }
        }

        public async Task SeedAsync()
        {
            var roles = new[] { Roles.Attendee, Roles.Admin, Roles.Organizer };

            if (!dbContext.Roles.Any())
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));

                }
            }

            if (!dbContext.Speakers.Any())
            {
                var seedPath = Path.Combine("..", "EventManagment.Infrastructure.Persistence", "_Data", "Seeds", "Speakers.json");
                var speakerdata = await File.ReadAllTextAsync(seedPath);
                var speakers = JsonSerializer.Deserialize<List<Speaker>>(speakerdata);

                if (speakers?.Count > 0)
                {
                    await dbContext.Speakers.AddRangeAsync(speakers);
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Users.Any())
            {
                var user = new ApplicationUser
                {
                    FullName = "Mohamed Hamdy",
                    UserName = "Mohammedhamdi726@gmail.com",
                    Email = "Mohammedhamdi726@gmail.com",
                    PhoneNumber = "01029442023",
                    Types = Types.Admin,

                };

                await userManager.CreateAsync(user, "01124833532");
                await userManager.AddToRoleAsync(user, Roles.Admin);


            }


        }
    }
}


