using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Specifications.Events
{
    public class EvenstWithCategoryAndOrgnizerSpecification : BaseSpecification<Event, int>
    {
        public EvenstWithCategoryAndOrgnizerSpecification(string? sort, int? categoryId, string? orgnizerid, int pageSize, int pageIndex, string? search)
         : base(


               p =>
               (string.IsNullOrEmpty(search) || p.NormalizedTitle.Contains(search))
               &&
               (!categoryId.HasValue || p.Categoryid == categoryId.Value)
                         &&
                     (string.IsNullOrEmpty(orgnizerid) || p.OrganizerId == orgnizerid)
               )
        {
            AddIncludes();



            switch (sort)
            {
                case "TitleDesc":
                    AddOrderByDesc(p => p.Title);
                    break;

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



        public EvenstWithCategoryAndOrgnizerSpecification(int id) : base(id)
        {
            AddIncludes();

        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(p => p.Category!);
            Includes.Add(p => p.Organizer!);
        }
    }
}

