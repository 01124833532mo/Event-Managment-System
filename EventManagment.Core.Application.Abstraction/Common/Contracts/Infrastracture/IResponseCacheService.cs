namespace EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key, object response, TimeSpan timeToLive);
        Task<string?> GetCachedResponseAsync(string key);
    }
}
