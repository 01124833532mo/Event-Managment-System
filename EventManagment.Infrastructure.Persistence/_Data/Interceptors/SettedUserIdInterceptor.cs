using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EventManagment.Infrastructure.Persistence._Data.Interceptors
{
    public class SettedUserIdInterceptor : AuditInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public SettedUserIdInterceptor(ILoggedInUserService loggedInUserService) : base(loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);


        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {

            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private new void UpdateEntities(DbContext? context)
        {

            if (context is null) return;

            var Entries = context.ChangeTracker.Entries<IBaseUserId>()
                                .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in Entries)
            {
                if (string.IsNullOrEmpty(_loggedInUserService.UserId))
                {
                    _loggedInUserService.UserId = "";
                }

                if (entry.State is EntityState.Added)
                {

                    entry.Entity.AttendeeId = _loggedInUserService.UserId;

                }

            }
        }

    }
}
