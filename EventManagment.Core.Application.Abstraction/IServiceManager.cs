using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Application.Abstraction.Services.FeedBacks;
using EventManagment.Core.Application.Abstraction.Services.Registrations;
using EventManagment.Core.Application.Abstraction.Services.Speakers;
using EventManagment.Core.Application.Abstraction.Services.Sponsers;

namespace EventManagment.Core.Application.Abstraction
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }
        public IEventServices EventServices { get; }
        public ICategoryService CategoryService { get; }
        public IRegistrationService RegistrationService { get; }
        public IFeedBackService FeedBackService { get; }
        public ISponserService SponserService { get; }
        public ISpeakerService SpeakerService { get; }
    }
}
