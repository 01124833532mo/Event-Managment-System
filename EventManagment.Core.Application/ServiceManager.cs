using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Application.Abstraction.Services.FeedBacks;
using EventManagment.Core.Application.Abstraction.Services.Registrations;
using EventManagment.Core.Application.Abstraction.Services.Speakers;
using EventManagment.Core.Application.Abstraction.Services.Sponsers;

namespace EventManagment.Core.Application
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IEventServices> _eventServices;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IRegistrationService> _registrationService;
        private readonly Lazy<IFeedBackService> _feedBackService;
        private readonly Lazy<ISponserService> _sponserService;
        private readonly Lazy<ISpeakerService> _speakerService;

        public ServiceManager(Func<IAuthService> authfactory,
            Func<IEventServices> eventservice,
            Func<ICategoryService> categoryfactory,
            Func<IRegistrationService> registrationfactory,
            Func<IFeedBackService> Feedbackfactory,
             Func<ISponserService> sponserfactory,
             Func<ISpeakerService> speakerfactory)
        {
            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _eventServices = new Lazy<IEventServices>(eventservice, LazyThreadSafetyMode.ExecutionAndPublication);
            _categoryService = new Lazy<ICategoryService>(categoryfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _registrationService = new Lazy<IRegistrationService>(registrationfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _feedBackService = new Lazy<IFeedBackService>(Feedbackfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _sponserService = new Lazy<ISponserService>(sponserfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _speakerService = new Lazy<ISpeakerService>(speakerfactory, LazyThreadSafetyMode.ExecutionAndPublication);

        }


        public IAuthService AuthService => _authService.Value;

        public IEventServices EventServices => _eventServices.Value;

        public ICategoryService CategoryService => _categoryService.Value;

        public IRegistrationService RegistrationService => _registrationService.Value;

        public IFeedBackService FeedBackService => _feedBackService.Value;

        public ISponserService SponserService => _sponserService.Value;

        public ISpeakerService SpeakerService => _speakerService.Value;
    }
}
