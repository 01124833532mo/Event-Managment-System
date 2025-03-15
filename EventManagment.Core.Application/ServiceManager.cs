using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Application.Abstraction.Services.Registrations;

namespace EventManagment.Core.Application
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IEventServices> _eventServices;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IRegistrationService> _registrationService;

        public ServiceManager(Func<IAuthService> authfactory, Func<IEventServices> eventservice, Func<ICategoryService> categoryfactory, Func<IRegistrationService> registrationfactory)
        {
            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _eventServices = new Lazy<IEventServices>(eventservice, LazyThreadSafetyMode.ExecutionAndPublication);
            _categoryService = new Lazy<ICategoryService>(categoryfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _registrationService = new Lazy<IRegistrationService>(registrationfactory, LazyThreadSafetyMode.ExecutionAndPublication);

        }


        public IAuthService AuthService => _authService.Value;

        public IEventServices EventServices => _eventServices.Value;

        public ICategoryService CategoryService => _categoryService.Value;

        public IRegistrationService RegistrationService => _registrationService.Value;
    }
}
