using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Specifications.Events
{
    public class EventWithCategoryAndOrgnizerSpecification : BaseSpecification<Event, int>
    {


        public EventWithCategoryAndOrgnizerSpecification(int id) : base(id)
        {
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Organizer);
        }
    }
}
