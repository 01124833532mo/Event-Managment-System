using EventManagment.Core.Domain.Entities.Registrations;

namespace EventManagment.Core.Domain.Specifications.Registrations
{
    public class RegistrationWithEventAndCategorySpecification : BaseSpecification<Registration, int>
    {


        public RegistrationWithEventAndCategorySpecification(string? sort, int? eventid, int? registrationid, int pageSize, int pageIndex)
        : base(


              p =>
              (!eventid.HasValue || p.Eventid == eventid.Value)
                        &&
                            (!registrationid.HasValue || p.Id == registrationid.Value)

              )
        {

            AddIncludes();



            switch (sort)
            {


                case "CreateOnAsec":
                    AddOrderBy(p => p.CreatedOn);
                    break;


                default:
                    AddOrderByDesc(p => p.CreatedOn);
                    break;
            }

            // totalproducts 18 ~ 20
            //page size = 5
            //page index = 3

            ApplyPagination((pageIndex - 1) * pageSize, pageSize);

        }

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
