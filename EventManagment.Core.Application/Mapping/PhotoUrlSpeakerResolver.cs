using AutoMapper;
using EventManagment.Core.Domain.Entities.Speakers;
using EventManagment.Shared.Models.Speakers;
using Microsoft.Extensions.Configuration;

namespace EventManagment.Core.Application.Mapping
{
    public class PhotoUrlSpeakerResolver(IConfiguration configuration) : IValueResolver<Speaker, SpeakerToReturn, string>
    {
        public string Resolve(Speaker source, SpeakerToReturn destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PhotoUrl))
            {
                return $"{configuration["Urls:ApiBaseUrl"]}/{source.PhotoUrl}";
            }
            return string.Empty;

        }
    }
}
