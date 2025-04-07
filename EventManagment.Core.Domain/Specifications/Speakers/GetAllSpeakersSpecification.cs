using EventManagment.Core.Domain.Entities.Speakers;

namespace EventManagment.Core.Domain.Specifications.Speakers
{
    public class GetAllSpeakersSpecification : BaseSpecification<Speaker, int>
    {
        public GetAllSpeakersSpecification(int pagesize, int pageindex) : base()
        {
            ApplyPagination((pageindex - 1) * pagesize, pagesize);


        }

        public GetAllSpeakersSpecification(int id) : base(id)
        {

        }

    }
}
