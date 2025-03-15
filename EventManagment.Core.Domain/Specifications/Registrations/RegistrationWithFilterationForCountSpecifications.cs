using EventManagment.Core.Domain.Entities.Registrations;

namespace EventManagment.Core.Domain.Specifications.Registrations
{
    public class RegistrationWithFilterationForCountSpecifications : BaseSpecification<Registration, int>
    {
        public RegistrationWithFilterationForCountSpecifications(int? eventid, int? registrationid)
          : base(
                 p =>

                 (!eventid.HasValue || p.Eventid == eventid.Value)
                          &&
                  (!registrationid.HasValue || p.Id == registrationid.Value)

               )
        {

        }
    }
}
