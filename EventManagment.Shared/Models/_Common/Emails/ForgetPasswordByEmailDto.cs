using System.ComponentModel.DataAnnotations;

namespace EventManagment.Shared.Models._Common.Emails
{
    public class ForgetPasswordByEmailDto
    {
        [Required]
        public required string Email { get; set; }
    }
}
