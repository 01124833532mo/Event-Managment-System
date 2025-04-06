using EventManagment.Core.Domain.Entities.FeedBacks;

namespace EventManagment.Core.Domain.Specifications.FeedBacks
{
    public class GetwithEventAndAttenddeSpecification : BaseSpecification<Feedback, int>
    {
        public GetwithEventAndAttenddeSpecification(int? eventid, string? attendeeid, int pageSize, int pageIndex)
                : base(


                      p =>

                      (!eventid.HasValue || p.EventId == eventid.Value)
                                &&
                            (string.IsNullOrEmpty(attendeeid) || p.AttenddeId == attendeeid)
                      )
        {
            AddIncludes();





            ApplyPagination((pageIndex - 1) * pageSize, pageSize);


        }



        public GetwithEventAndAttenddeSpecification(int id) : base(id)
        {
            AddIncludes();

        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(p => p.Event!);
            Includes.Add(p => p.Attendde!);
        }
    }
}
