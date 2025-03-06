using EventManagment.Core.Domain.Common;

namespace EventManagment.Core.Domain.Contracts.Persestence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>;


        public Task<int> CompleteAsync();

    }
}
