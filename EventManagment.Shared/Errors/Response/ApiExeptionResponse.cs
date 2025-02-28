using System.Text.Json;

namespace EventManagment.Shared.Errors.Response
{
    public class ApiExeptionResponse : ApiResponse
    {
        public string? Details { get; set; }

        public ApiExeptionResponse(int statuscode, string? message = null, string? details = null)
            : base(statuscode, message)
        {
            Details = details;

        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
