using AutoMapper;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Shared.Models.Categories;
using EventManagment.Shared.Models.Events;
using EventManagment.Shared.Models.Registrations;

namespace EventManagment.Core.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventDto, Event>().ReverseMap();

            CreateMap<Event, EventToreturn>()
               .ForMember(dest => dest.CategoryName, otp => otp.MapFrom(src => src.Category.Name))
               .ForMember(dest => dest.OrganizerName, otp => otp.MapFrom(src => src.Organizer.FullName));

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
