using EventManagment.Core.Domain._Identity;
using Microsoft.AspNetCore.Http;

namespace EventManagment.Shared.Models.Auth
{
    public class RegisterDto
    {

        public required string Email { get; set; }


        public required string FullName { get; set; }

        public required string PhoneNumber { get; set; }

        public DateOnly? BirthDate { get; set; }

        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? CompanyName { get; set; }


        public required string Password { get; set; }
        public required Types Types { get; set; }

        public IFormFile? PictureUrl { get; set; }
    }
}
