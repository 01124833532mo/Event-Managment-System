using EventManagment.Core.Domain.Entities.Waitlists;

namespace EventManagment.Core.Domain.Specifications.WalitLists
{
    public class WaitListPaginatedSpecification : BaseSpecification<WaitList, int>
    {

        public WaitListPaginatedSpecification(int pagesize, int pageindex) : base(x => x.IsNotified == false)
        {

            ApplyPagination((pageindex - 1) * pagesize, pagesize);

            AddIncludes();


        }
        public WaitListPaginatedSpecification(int id) : base(id)
        {
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(x => x.Event!);
            Includes.Add(x => x.Attendde!);
        }




    }
}
