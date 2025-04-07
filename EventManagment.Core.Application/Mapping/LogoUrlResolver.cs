using AutoMapper;
using EventManagment.Core.Domain.Entities.Sponsers;
using EventManagment.Shared.Models.Sponsers;
using Microsoft.Extensions.Configuration;

namespace EventManagment.Core.Application.Mapping
{
    internal class LogoUrlResolver(IConfiguration configuration) : IValueResolver<Sponser, SponserToReturn, string>
    {
        public string Resolve(Sponser source, SponserToReturn destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.LogoUrl))
            {
                return $"{configuration["Urls:ApiBaseUrl"]}/{source.LogoUrl}";
            }
            return string.Empty;

        }
    }

}
