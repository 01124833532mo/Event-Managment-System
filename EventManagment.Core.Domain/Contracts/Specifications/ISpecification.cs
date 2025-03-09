using EventManagment.Core.Domain.Common;
using System.Linq.Expressions;

namespace EventManagment.Core.Domain.Contracts.Specifications
{
    public interface ISpecification<TEntity, Tkey> where TEntity :
        BaseEntity<Tkey> where Tkey : IEquatable<Tkey>
    {

        Expression<Func<TEntity, bool>>? Criteria { get; set; }
        List<Expression<Func<TEntity, object>>> Includes { get; set; }

        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesending { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }

        public bool IsPaginationEnabled { get; set; }

    }
}
