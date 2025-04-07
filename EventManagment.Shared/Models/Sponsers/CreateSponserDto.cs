using Microsoft.AspNetCore.Http;

namespace EventManagment.Shared.Models.Sponsers
{
    public class CreateSponserDto
    {
        public required string Name { get; set; }
        public required IFormFile LogoUrl { get; set; }
        public required string Website { get; set; }
        public required string Description { get; set; }
    }
}
