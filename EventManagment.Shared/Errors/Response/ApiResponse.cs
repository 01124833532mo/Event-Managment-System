using System.Text.Json;

namespace EventManagment.Shared.Errors.Response
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public ApiResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMessageForStatusCode(statuscode);

        }

        private string? GetDefaultMessageForStatusCode(int statuscode)
        {

            return statuscode switch
            {
                400 => "A Bad Request ,you have made",
                401 => "Authorized , you are not",
                404 => "Resource was not Found",
                500 => "Errors are the path to the dark side , Errors lead to anger .Anger lead to hate . Hate lead to career change ",
                _ => null
            };

        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
