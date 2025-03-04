using System.ComponentModel.DataAnnotations;

namespace EventManagment.Shared.Models._Common.Emails
{
    public class ResetPasswordByEmailDto : ForgetPasswordByEmailDto
    {
        [Required]
        public required string NewPassword { get; set; }
    }
}
