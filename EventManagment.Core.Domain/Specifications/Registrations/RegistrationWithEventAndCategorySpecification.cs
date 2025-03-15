using EventManagment.Core.Domain.Entities.Registrations;

namespace EventManagment.Core.Domain.Specifications.Registrations
{
    public class RegistrationWithEventAndCategorySpecification : BaseSpecification<Registration, int>
    {
        public RegistrationWithEventAndCategorySpecification(int id) : base(id)
        {
            AddIncludes();
        }



        private protected override void AddIncludes()
        {
            base.AddIncludes();

            Includes.Add(p => p.Event!);
            Includes.Add(p => p.Event!.Category!);
            Includes.Add(p => p.Attendee!);
        }


    }
}
