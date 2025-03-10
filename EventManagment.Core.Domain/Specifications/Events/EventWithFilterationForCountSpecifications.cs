using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Specifications.Events
{
    public class EventWithFilterationForCountSpecifications : BaseSpecification<Event, int>
    {
        public EventWithFilterationForCountSpecifications(string? orgnizerid, int? categoryId, string? search)
          : base(
                 p =>
                 //     (string.IsNullOrEmpty(search) || p.NormalizedName.Contains(search))
                 //&&
                 (!categoryId.HasValue || p.Categoryid == categoryId.Value)
                          &&
                      (string.IsNullOrEmpty(search) || p.OrganizerId == orgnizerid)
               )
        {

        }
    }
}
