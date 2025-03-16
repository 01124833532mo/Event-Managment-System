using System.ComponentModel.DataAnnotations;

namespace EventManagment.Shared.Models.Auth
{
    public class ConfirmationEmailCodeDto
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public int ConfirmationCode { get; set; }
    }
}
