using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Entities.FeedBacks
{
    public class Feedback : BaseAuditableEntity<int>
    {




        public decimal Rate { get; set; }

        public string Comment { get; set; }






        // navigation Property
        public int? EventId { get; set; }
        public virtual Event? Event { get; set; }
        public string? AttenddeId { get; set; }
        public virtual Attendde? Attendde { get; set; }


    }

}
