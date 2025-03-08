using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Events;

namespace EventManagment.Core.Application
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IEventServices> _eventServices;

        public ServiceManager(Func<IAuthService> authfactory, Func<IEventServices> eventservice)
        {
            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _eventServices = new Lazy<IEventServices>(eventservice, LazyThreadSafetyMode.ExecutionAndPublication);

        }


        public IAuthService AuthService => _authService.Value;

        public IEventServices EventServices => _eventServices.Value;
    }
}
