using EventManagment.Core.Domain.Entities.Sessions;

namespace EventManagment.Core.Domain.Specifications.Sessions
{
    public class GetAllSesstionSpecification : BaseSpecification<Session, int>
    {
        public GetAllSesstionSpecification(int? eventId, int? speakerid, int pageSize, int pageIndex)
        : base(
        p =>
        (!speakerid.HasValue || p.SpeakerId == speakerid.Value)

               &&
               (!eventId.HasValue || p.EventId == eventId.Value)

               )
        {
            AddIncludes();





            ApplyPagination((pageIndex - 1) * pageSize, pageSize);


        }



        public GetAllSesstionSpecification(int id) : base(id)
        {
            AddIncludes();

        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(p => p.Event!);
            Includes.Add(p => p.Speaker!);
        }
    }
}
