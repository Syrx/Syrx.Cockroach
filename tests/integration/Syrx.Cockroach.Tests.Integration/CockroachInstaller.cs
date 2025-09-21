namespace Syrx.Cockroach.Tests.Integration
{
    public class CockroachInstaller
    {
        public SyrxBuilder SyrxBuilder { get; }
        public IServiceProvider Provider { get; }

        public CockroachInstaller(string connectionString)
        {
            var services = new ServiceCollection();
            var builder = new SyrxBuilder(services);
            SyrxBuilder = builder.SetupCockroach(CockroachFixture.Alias, connectionString);

            Provider = services.BuildServiceProvider();
            var commander = Provider.GetService<ICommander<DatabaseBuilder>>();
            var database = new DatabaseBuilder(commander);
            database.Build();
        }
    }
}