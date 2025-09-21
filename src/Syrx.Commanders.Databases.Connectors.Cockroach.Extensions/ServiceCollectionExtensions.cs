namespace Syrx.Commanders.Databases.Connectors.Cockroach.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddCockroach(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return services.TryAddToServiceCollection(
                typeof(IDatabaseConnector),
                typeof(CockroachDatabaseConnector),
                lifetime);
        }
    }
}