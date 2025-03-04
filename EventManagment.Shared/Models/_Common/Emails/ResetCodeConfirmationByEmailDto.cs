using System.ComponentModel.DataAnnotations;

namespace EventManagment.Shared.Models._Common.Emails
{
    public class ResetCodeConfirmationByEmailDto : ForgetPasswordByEmailDto
    {
        [Required]
        public required int ResetCode { get; set; }
    }
}
