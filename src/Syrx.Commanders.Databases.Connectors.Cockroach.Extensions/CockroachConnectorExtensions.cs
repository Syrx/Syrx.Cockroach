namespace Syrx.Commanders.Databases.Connectors.Cockroach.Extensions
{
    public static class CockroachConnectorExtensions
    {
        public static SyrxBuilder UseCockroach(
            this SyrxBuilder builder,
            Action<CommanderSettingsBuilder> factory,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var options = CommanderSettingsBuilderExtensions.Build(factory);
            builder.ServiceCollection
                .AddSingleton<ICommanderSettings, CommanderSettings>(a => options)
                .AddReader(lifetime) // add reader
                .AddCockroach(lifetime) // add connector
                .AddDatabaseCommander(lifetime);

            return builder;
        }

    }
}