using EventManagment.Core.Domain._Identity;

namespace EventManagment.Shared.Models.Auth
{
    public class RegisterDto
    {

        public required string Email { get; set; }


        public required string FullName { get; set; }

        public required string PhoneNumber { get; set; }

        //[RegularExpression("(?=^.{6,10}$)(?=.\\d)(?=.[a-z])(?=.[A-Z])(?=.[!@#%^&amp;()_+}{&quot;:;'?/&gt;.&lt;,])(?!.\\s).*$",
        //					ErrorMessage = "Password must have 1 UpperCase,1 LowerCase,1 number , 1 non alphanumberic and at least 6 characters ")]
        public required string Password { get; set; }
        public required Types Types { get; set; }
    }
}
