using EventManagment.Core.Domain.Entities.Sessions;

namespace EventManagment.Core.Domain.Specifications.Sessions
{
    public class getSessionCountSpecification : BaseSpecification<Session, int>
    {
        public getSessionCountSpecification(int? eventId)
       : base(
       p =>
       (!eventId.HasValue || p.EventId == eventId.Value))


        {



        }

    }
}
