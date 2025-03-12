using AutoMapper;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Shared.Models.Categories;
using EventManagment.Shared.Models.Events;

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

        }
    }
}
