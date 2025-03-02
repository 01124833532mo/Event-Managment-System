namespace EventManagment.Core.Domain.Contracts.Persestence.DbInitializers
{
    public interface IEventManagmentDbInitializer
    {
        Task InitializeAsync();
        Task SeedAsync();
    }
}
