using EventManagment.Core.Domain.Common;

namespace EventManagment.Core.Domain.Entities.Speakers
{
    public class Speaker : BaseEntity<int>
    {

        public required string Name { get; set; }
        public required string Bio { get; set; }
        public required string PhotoUrl { get; set; }

    }
}
