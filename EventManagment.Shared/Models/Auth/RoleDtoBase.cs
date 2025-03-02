using System.Text.Json.Serialization;

namespace EventManagment.Shared.Models.Auth
{
    public class RoleDtoBase
    {
        [JsonIgnore]
        public string Id { get; set; }

        public required string Name { get; set; }
        public RoleDtoBase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
