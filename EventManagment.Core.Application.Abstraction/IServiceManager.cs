using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Application.Abstraction.Services.Registrations;

namespace EventManagment.Core.Application.Abstraction
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }
        public IEventServices EventServices { get; }
        public ICategoryService CategoryService { get; }
        public IRegistrationService RegistrationService { get; }
    }
}
