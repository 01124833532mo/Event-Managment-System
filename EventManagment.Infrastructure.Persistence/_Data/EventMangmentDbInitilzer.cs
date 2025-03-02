using EventManagment.Core.Domain._Identity;
using EventManagment.Core.Domain.Contracts.Persestence.DbInitializers;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));

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


