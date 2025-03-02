namespace EventManagment.Shared.Errors.Models
{
    public class ValidationExeption : BadRequestExeption
    {
        public required IEnumerable<string> Errors { get; set; }
        public ValidationExeption(string message = "Bad Request") : base(message)
        {


        }
    }
}
