using EventManagment.Core.Domain.Entities.Sponsers;

namespace EventManagment.Core.Domain.Specifications.Sponsers
{
    public class GetAllSponserSpecification : BaseSpecification<Sponser, int>
    {
        public GetAllSponserSpecification(int pageSize, int pageIndex)

        {

            ApplyPagination((pageIndex - 1) * pageSize, pageSize);

        }



        public GetAllSponserSpecification(int id) : base(id)
        {

        }


    }
}
