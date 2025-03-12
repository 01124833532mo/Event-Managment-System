using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Application.Abstraction.Services.Events;

namespace EventManagment.Core.Application
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IEventServices> _eventServices;
        private readonly Lazy<ICategoryService> _categoryService;

        public ServiceManager(Func<IAuthService> authfactory, Func<IEventServices> eventservice, Func<ICategoryService> categoryfactory)
        {
            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _eventServices = new Lazy<IEventServices>(eventservice, LazyThreadSafetyMode.ExecutionAndPublication);
            _categoryService = new Lazy<ICategoryService>(categoryfactory, LazyThreadSafetyMode.ExecutionAndPublication);

        }


        public IAuthService AuthService => _authService.Value;

        public IEventServices EventServices => _eventServices.Value;

        public ICategoryService CategoryService => _categoryService.Value;
    }
}
