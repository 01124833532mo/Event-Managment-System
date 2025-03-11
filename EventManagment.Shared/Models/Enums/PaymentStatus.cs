using System.Runtime.Serialization;

namespace EventManagment.Core.Domain.Enums
{
    public enum PaymentStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Received")]

        PaymentReceived,
        [EnumMember(Value = "Payment Failed")]

        PaymentFailed,
    }
}
