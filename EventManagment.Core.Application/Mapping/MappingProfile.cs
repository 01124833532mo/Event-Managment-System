using AutoMapper;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.FeedBacks;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Entities.Speakers;
using EventManagment.Core.Domain.Entities.Sponsers;
using EventManagment.Shared.Models.Categories;
using EventManagment.Shared.Models.Events;
using EventManagment.Shared.Models.FeedBacks;
using EventManagment.Shared.Models.Registrations;
using EventManagment.Shared.Models.Speakers;
using EventManagment.Shared.Models.Sponsers;

namespace EventManagment.Core.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventDto, Event>().ReverseMap();

            CreateMap<CreateSponserDto, Sponser>();



            CreateMap<Sponser, SponserToReturn>()
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom<LogoUrlResolver>());


            CreateMap<Speaker, SpeakerToReturn>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom<PhotoUrlSpeakerResolver>());

            CreateMap<Event, EventToreturn>()
               .ForMember(dest => dest.CategoryName, otp => otp.MapFrom(src => src.Category.Name))
               .ForMember(dest => dest.OrganizerName, otp => otp.MapFrom(src => src.Organizer.FullName))
               .ForMember(dest => dest.NameOfSponser, otp => otp.MapFrom(src => src.Sponser!.Name));


            CreateMap<Feedback, FeedBackToRetuen>()
              .ForMember(dest => dest.AttenddeName, otp => otp.MapFrom(src => src.Attendde!.FullName))
              .ForMember(dest => dest.EventTitel, otp => otp.MapFrom(src => src.Event!.Title));

            CreateMap<CategoryDto, Category>().ReverseMap();


            CreateMap<CreateRegisterDto, Registration>().ReverseMap();


            CreateMap<Registration, RegisterToReturn>()
                    .ForMember(dest => dest.EventTitle, otp => otp.MapFrom(src => src.Event.Title))
                .ForMember(dest => dest.FullName, otp => otp.MapFrom(src => src.Attendee.FullName))
                .ForMember(dest => dest.CategoryName, otp => otp.MapFrom(src => src.Event.Category.Name))
                .ForMember(dest => dest.EventDate, otp => otp.MapFrom(src => src.Event.Data));



        }
    }
}
