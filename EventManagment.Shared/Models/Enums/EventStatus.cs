using System.Runtime.Serialization;

namespace EventManagment.Core.Domain.Enums
{
    public enum EventStatus
    {
        [EnumMember(Value = "scheduled")]

        scheduled = 1,
        [EnumMember(Value = "completed")]
        completed,
        [EnumMember(Value = "canceled")]

        canceled
    }
}
