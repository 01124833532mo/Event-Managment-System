using EventManagment.Core.Domain.Entities.Sessions;

namespace EventManagment.Core.Domain.Specifications.Sessions
{
    public class GetAllSesstionSpecification : BaseSpecification<Session, int>
    {
        public GetAllSesstionSpecification(int? eventId, int pageSize, int pageIndex)
        : base(
        p =>
        (!eventId.HasValue || p.EventId == eventId.Value))


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
